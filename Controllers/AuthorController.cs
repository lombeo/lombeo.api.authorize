﻿using Lombeo.Api.Authorize.DTO;
using Lombeo.Api.Authorize.DTO.AuthenDTO;
using Lombeo.Api.Authorize.DTO.Common;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Infra.Entities;
using Lombeo.Api.Authorize.Services.AuthenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lombeo.Api.Authorize.Controllers
{
    [ApiController]
    [Route(RouteApiConstant.BASE_PATH + "/authen")]
    [Authorize]
    public class AuthorController : BaseAPIController
    {
        private readonly IAuthenService _authenService;

        public AuthorController(IAuthenService authenService)
        {
            _authenService = authenService;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<ResponseDTO<bool>> SignUp([FromBody] SignUpDTO model)
        {
            return await HandleException(_authenService.SignUp(model));
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<ResponseDTO<ReturnSignInDTO>> SignIn([FromBody] SignInDTO model)
        {
            return await HandleException(_authenService.SignIn(model));
        }

        [HttpGet("list-user")]
        public async Task<ResponseDTO<List<User>>> List()
        {
            return await HandleException(_authenService.List());
        }

        [HttpGet("get-user-profile")]
        public async Task<ResponseDTO<UserDTO>> GetUserByToken()
        {
            return await HandleException(_authenService.GetUserProfile(User.Identity));
        }

        [HttpPost("save-user-profile")]
        public async Task<ResponseDTO<int>> SaveUserProfile([FromBody] SaveProfileDTO model)
        {
            model.ActionBy = UserId;
			return await HandleException(_authenService.SaveUserProfile(model));
		}

        [HttpGet("get-user-by-username")]
        public async Task<ResponseDTO<User>> FindUserByUsername([FromQuery] string username)
        {
            return await HandleException(_authenService.FindUserByUsername(username));
        }
    }
}
