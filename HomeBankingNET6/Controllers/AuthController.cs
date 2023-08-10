using Microsoft.AspNetCore.Mvc;
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
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ClientAuthDTO loginRequest)
        {
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
