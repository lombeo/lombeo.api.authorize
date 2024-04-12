using Lombeo.Api.Authorize.DTO.AuthenDTO;
using Lombeo.Api.Authorize.Infra;
using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.Services.AuthenService
{
    public interface IAuthenService
    {
        Task<bool> SignUp(SignUpDTO model);
        Task<bool> SignIn(SignInDTO model);
    }

    public class AuthenService : IAuthenService
    {
        private readonly LombeoAuthorizeContext _context;

        public AuthenService(LombeoAuthorizeContext context)
        {
            _context = context;
        }

        public Task<bool> SignIn(SignInDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SignUp(SignUpDTO model)
        {
            var account = new UserAuthen
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = "User"
            };

            await _context.UserAuthens.AddAsync(account);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
