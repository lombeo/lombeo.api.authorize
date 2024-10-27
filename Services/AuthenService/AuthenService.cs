using DocumentFormat.OpenXml.VariantTypes;
using Lombeo.Api.Authorize.DTO;
using Lombeo.Api.Authorize.DTO.AuthenDTO;
using Lombeo.Api.Authorize.Infra;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Infra.Entities;
using Lombeo.Api.Authorize.Infra.Enums;
using Lombeo.Api.Authorize.Services.CacheService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Lombeo.Api.Authorize.Services.AuthenService
{
	public interface IAuthenService
	{
		void InitUserMemory();
		void UpdateUserMemory(int userId);
		void TriggerUpdateUserMemory(int userId);
		Task<bool> SignUp(SignUpDTO model);
		Task<ReturnSignInDTO> SignIn(SignInDTO model);
		Task<List<User>> List();
		Task<UserDTO> GetUserProfile(System.Security.Principal.IIdentity? identity);
		Task<int> SaveUserProfile(SaveProfileDTO userProfile);
		Task<User> FindUserByUsername(string username);
	}

	public class AuthenService : IAuthenService
	{
		private readonly LombeoAuthorizeContext _context;
		private readonly IPubSubService _pubSubService;
		private readonly ILogger<AuthenService> _logger;
		private readonly X509Certificate2 _certificate;
		private readonly string _validIssuer;
		private readonly string _validAudience;

		public AuthenService(LombeoAuthorizeContext context, IPubSubService pubSubService, ILogger<AuthenService> logger)
		{
			_context = context;
			_pubSubService = pubSubService;
			_logger = logger;
			_certificate = new X509Certificate2(StaticVariable.JwtValidation.CertificatePath, StaticVariable.JwtValidation.CertificatePassword);
			_validIssuer = StaticVariable.JwtValidation.ValidIssuer;
			_validAudience = StaticVariable.JwtValidation.ValidAudience;
		}

		public void InitUserMemory()
		{
			if (!StaticVariable.IsInitializedUser)
			{
				_logger.LogError($"InitUserMemory Started: {DateTime.UtcNow}");

				var data = _context.Users.AsNoTracking().Where(t => !t.Deleted)
											.OrderByDescending(t => t.UpdatedAt).OrderByDescending(t => t.CreatedAt).ToList();
				StaticVariable.UserMemory = data;
				StaticVariable.IsInitializedUser = true;

				_logger.LogError($"InitUserMemory Finished: {DateTime.UtcNow}");
			}
		}

		public void UpdateUserMemory(int userId)
		{
			var list = StaticVariable.UserMemory.ToList();
			list = list ?? new List<User>();
			if (list.Any(t => t.Id == userId))
			{
				list = list.Where(t => t.Id != userId).OrderByDescending(t => t.CreatedAt).ToList();
			}

			var user = _context.Users.AsNoTracking().FirstOrDefault(t => !t.Deleted && t.Id == userId);
			if (user != null)
			{
				list.Add(user);
			}

			StaticVariable.UserMemory = list;
		}

		public void TriggerUpdateUserMemory(int userId)
		{
			UpdateUserMemory(userId);

			_pubSubService.PublishSystem(new PubSubMessage
			{
				PubSubEnum = PubSubEnum.UpdateUserMemory,
				Data = userId.ToString()
			});
		}

		public async Task<List<User>> List()
		{
			IEnumerable<User> allUsers = StaticVariable.UserMemory.ToList();
			return allUsers.ToList();
		}

		public async Task<ReturnSignInDTO> SignIn(SignInDTO model)
		{
			var user = await IsValidLogin(model);
			var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(t =>  t.UserId == user.Id);
			if (user != null)
			{
				// Tạo các claim chứa thông tin của người dùng
				var claims = new[]
				{
					new Claim(ClaimTypes.Name, user.Username),
					new Claim(ClaimTypes.Role, user.Role),
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Email, user.Email)
				};

				// Tạo các token descriptor
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(claims),
					Expires = DateTime.UtcNow.AddDays(7), // Thời gian hết hạn của token
					Issuer = _validIssuer,
					Audience = _validAudience,
					SigningCredentials = new SigningCredentials(new X509SecurityKey(_certificate), SecurityAlgorithms.RsaSha256Signature)
				};

				// Tạo JWT token sử dụng JwtSecurityTokenHandler
				var tokenHandler = new JwtSecurityTokenHandler();
				var token = tokenHandler.CreateToken(tokenDescriptor);

				// Trả về token dưới dạng string
				var tokenString = tokenHandler.WriteToken(token);

				// Tạo các thông tin đăng nhập cho người dùng
				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
				identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
				identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

				return new ReturnSignInDTO()
				{
                    Token = tokenString,
					UserName = user.Username,
					User = userProfile,
					Email = user.Email,
					Role = user.Role
                };
			}
			else
			{
				throw new ApplicationException(Message.AuthenMessage.INVALID_LOGIN);
			}
		}

		private async Task<User> IsValidLogin(SignInDTO model)
		{
			var data = StaticVariable.UserMemory.ToList();
			var password = HashPassword(model.Password);

			if (data.Any(t => t.Username.Equals(model.Username) && t.PasswordHash.Equals(password)))
			{
				return await FindUserByUsername(model.Username);
			}
			else
			{
				return null;
			}
		}

		public async Task<User> FindUserByUsername(string username)
		{
			if (string.IsNullOrWhiteSpace(username))
			{
				throw new ApplicationException(Message.CommonMessage.MISSING_PARAM);
			}
			var data = StaticVariable.UserMemory.ToList();
			var result = data.FirstOrDefault(t => t.Username == username);

			return result;
		}

		public async Task<bool> SignUp(SignUpDTO model)
		{
			ValidateSignUp(model);

			var password = HashPassword(model.Password);

			var account = new User
			{
				Username = model.Username,
				Email = model.Email,
				PasswordHash = password,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Role = "User"
			};

			
			await _context.Users.AddAsync(account);

            await _context.SaveChangesAsync();

            var userProfile = new UserProfile
            {
                UserId = _context.Users.FirstOrDefault(t => t.Username == account.Username).Id,
                FullName = model.Username,
                PhoneNumber = model.Phone,
                Gender = bool.Parse(model.Gender),
                Address = model.Address,
                Dob = model.birthDate,
                School = model.School,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _context.UserProfiles.AddAsync(userProfile);

            TriggerUpdateUserMemory(account.Id);

			return true;
		}

		private void ValidateSignUp(SignUpDTO model)
		{
			var data = StaticVariable.UserMemory.ToList();

			if (data.Any(t => t.Email.ToLower().Equals(model.Email.ToLower())))
			{
				throw new ApplicationException(Message.AuthenMessage.EXIST_EMAIL);
			}

			if (data.Any(t => t.Username.ToLower().Equals(model.Username.ToLower())))
			{
				throw new ApplicationException(Message.AuthenMessage.EXIST_USERNAME);
			}

			if (model.Username.Contains(" "))
			{
				throw new ApplicationException(Message.AuthenMessage.INVALID_USERNAME);
			}

			if (!Regex.IsMatch(model.Email, PatternConst.EMAIL_PATTERN))
			{
				throw new ApplicationException(Message.AuthenMessage.INVALID_EMAIL);
			}

			if (!Regex.IsMatch(model.PasswordHash, PatternConst.PASSWORD_PATTERN))
			{
				throw new ApplicationException(Message.AuthenMessage.INVALID_PASSWORD);
			}
		}

		public string HashPassword(string password)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}

		public async Task<UserDTO> GetUserProfile(System.Security.Principal.IIdentity? identity)
		{
			var claimsIdentity = identity as ClaimsIdentity;
			if (claimsIdentity != null)
			{
				var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

				IEnumerable<User> allUsers = StaticVariable.UserMemory.ToList();
				var user = allUsers.FirstOrDefault(t => t.Username == userName);
				var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(t => t.UserId == user.Id);
				if (userProfile == null)
				{
					throw new ApplicationException(Message.AuthenMessage.INVALID_USER);
				}
				UserDTO result = new UserDTO()
				{
					UserId = user.Id,
					FullName = userProfile.FullName,
					PhoneNumber = userProfile.PhoneNumber,
					Gender = userProfile.Gender,
					Address = userProfile.Address,
					PicProfile = userProfile.PicProfile,
					Dob = userProfile.Dob,
					Email = user.Email,
					Username = user.Username,
					Role = user.Role
				};

				return result;
			}

			throw new ApplicationException(Message.CommonMessage.NOT_AUTHEN);
		}

		public async Task<int> SaveUserProfile(SaveProfileDTO model)
		{
			var data = await _context.UserProfiles.FirstOrDefaultAsync(t => t.UserId == model.UserId);

			if(model.ActionBy != model.UserId)
			{
				if(!IsManager(model.ActionBy))
				{
					throw new ApplicationException(Message.CommonMessage.NOT_ALLOWED);
				}
			}

			if (data == null)
			{
				data = new UserProfile()
				{
					UserId = model.UserId,
					FullName = model.FullName,
					PhoneNumber = model.PhoneNumber,
					Gender = model.Gender,
					Address = model.Address,
					PicProfile = model.PicProfile,
					Dob = model.Dob,
					School = model.School,
					WorkAt = model.WorkAt
				};
				await _context.AddAsync(data);
			}
			else
			{
				data.FullName = model.FullName;
				data.PhoneNumber = model.PhoneNumber;
				data.Gender = model.Gender;
				data.Address = model.Address;
				data.PicProfile = model.PicProfile;
				data.Dob = model.Dob;
				data.School = model.School;
				data.WorkAt = model.WorkAt;

				_context.Update(data);
			}

			await _context.SaveChangesAsync();

			return data.Id;
		}

		public bool IsManager(int userId)
		{
			IEnumerable<User> allUsers = StaticVariable.UserMemory.ToList();
			var user = allUsers.FirstOrDefault(t => t.Id == userId);
			if (user != null)
			{
				if(user.Role == RoleConstValue.CONTENT_MANAGER)
				{
					return true;
				}
			}
			return false;
		}
		//public async Task<List<User>> ListUserWithCache()
		//{
		//    string cacheKey = RedisCacheKey.LIST_USER;

		//    var data = await _cacheService.GetAsync<List<User>>(cacheKey);

		//    if (data == null)
		//    {
		//        data = await _context.Users.Where(t => !t.Deleted).ToListAsync();
		//        _ = _cacheService.SetAsync(cacheKey, data);
		//    }
		//    return data;
		//}
	}
}
