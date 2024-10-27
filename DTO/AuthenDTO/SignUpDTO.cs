﻿namespace Lombeo.Api.Authorize.DTO.AuthenDTO
{
    public class SignUpDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime birthDate { get; set; }
        public string School { get; set; }
    }
}
