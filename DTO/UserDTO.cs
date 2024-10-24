using Lombeo.Api.Authorize.Infra.Entities;

namespace Lombeo.Api.Authorize.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string PicProfile { get; set; }
        public DateTime Dob { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
