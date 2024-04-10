namespace Lombeo.Api.Authorize.DTO.AuthenDTO
{
    public class SignUpDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
