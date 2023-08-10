using HomeBankingNET6.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HomeBankingNET6.Models;
using HomeBankingNET6.dtos;
using HomeBankingNET6.Helpers;

namespace HomeBankingNET6.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private readonly IPasswordHasher _passwordHasher;
        public AuthController(IClientRepository clientRepository, IPasswordHasher passwordHasher)
        {
            _clientRepository = clientRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ClientAuthDTO clientAuthDTO)
        {
            try
            { 
                Client user = _clientRepository.FindByEmail(clientAuthDTO.Email);
                bool result = _passwordHasher.Verify(user.Password, clientAuthDTO.Password);

                if (user == null || !result)
                {
                    return Unauthorized();
                }
                
                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            { 
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

                return Ok();
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
