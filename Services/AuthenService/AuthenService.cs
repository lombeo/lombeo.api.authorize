using Lombeo.Api.Authorize.DTO;
using Lombeo.Api.Authorize.DTO.AuthenDTO;
using Lombeo.Api.Authorize.Infra;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Infra.Entities;
using Lombeo.Api.Authorize.Infra.Enums;
using Lombeo.Api.Authorize.Services.CacheService;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Services.AuthenService
{
    public interface IAuthenService
    {
        void InitUserMemory();
        void UpdateUserMemory(int userId);
        void TriggerUpdateUserMemory(int userId);
        Task<bool> SignUp(SignUpDTO model);
        Task<bool> SignIn(SignInDTO model);
        Task<List<User>> List();
    }

    public class AuthenService : IAuthenService
    {
        private readonly LombeoAuthorizeContext _context;
        private readonly IPubSubService _pubSubService;
        private ICacheService _cacheService;
        private readonly ILogger<AuthenService> _logger;

        public AuthenService(LombeoAuthorizeContext context, IPubSubService pubSubService, ILogger<AuthenService> logger, ICacheService cacheService)
        {
            _context = context;
            _pubSubService = pubSubService;
            _logger = logger;
            _cacheService = cacheService;
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

        public Task<bool> SignIn(SignInDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SignUp(SignUpDTO model)
        {
            var account = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = "User"
            };

            await _context.Users.AddAsync(account);
            await _context.SaveChangesAsync();

            return true;
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
