using HomeBankingNET6.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using HomeBankingNET6.Models;
using HomeBankingNET6.Helpers;
﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HomeBankingNET6.DTOs;
using HomeBankingNET6.Services;

namespace HomeBankingNET6.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPasswordHasher _passwordHasher;
        public AuthController(IAuthService authService,  IPasswordHasher passwordHasher)
        {
            _authService = authService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ClientAuthDTO loginRequest)
        {
        // bool result = _passwordHasher.Verify(user.Password, clientAuthDTO.Password);

            bool isAuthenticated = await _authService.Login(loginRequest.Email, loginRequest.Password);
            if (isAuthenticated)
                return Ok();
            else
                return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok();
        }

    }
}
