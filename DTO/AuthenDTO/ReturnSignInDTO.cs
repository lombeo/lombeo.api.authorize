﻿namespace Lombeo.Api.Authorize.DTO.AuthenDTO
{
    public class ReturnSignInDTO
    {
        public string Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}