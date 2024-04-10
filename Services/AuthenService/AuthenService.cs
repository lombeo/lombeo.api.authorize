using Lombeo.Api.Authorize.DTO.AuthenDTO;

namespace Lombeo.Api.Authorize.Services.AuthenService
{
    public interface IAuthenService
    {
        Task<bool> SignUp(SignUpDTO model);
    }

    public class AuthenService : IAuthenService
    {
        public Task<bool> SignUp(SignUpDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
