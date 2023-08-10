using HomeBankingNET6.Models;
using HomeBankingNET6.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeBankingNET6.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(string email, string password)
        {
            Client user = _clientRepository.FindByEmail(email);
            if (user == null || user.Password != password)
                return false;

            var claims = new List<Claim>
            {
                new Claim("Client", user.Email),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            return true;
        }

        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public string UserAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst("Client")?.Value;
        }
    }
}
