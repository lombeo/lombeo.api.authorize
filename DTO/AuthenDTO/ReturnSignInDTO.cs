using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.DTO.AuthenDTO
{
    public class ReturnSignInDTO
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public UserProfile User { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
