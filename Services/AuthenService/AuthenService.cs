﻿using Lombeo.Api.Authorize.DTO.AuthenDTO;
using Lombeo.Api.Authorize.Infra;
using Lombeo.Api.Authorize.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Services.AuthenService
{
    public interface IAuthenService
    {
        Task<bool> SignUp(SignUpDTO model);
        Task<bool> SignIn(SignInDTO model);
        Task<List<User>> List();
    }

    public class AuthenService : IAuthenService
    {
        private readonly LombeoAuthorizeContext _context;

        public AuthenService(LombeoAuthorizeContext context)
        {
            _context = context;
        }

        public async Task<List<User>> List()
        {
            return await _context.Users.ToListAsync();
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
    }
}
