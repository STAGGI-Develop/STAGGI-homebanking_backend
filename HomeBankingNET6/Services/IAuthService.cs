using System.Threading.Tasks;

namespace HomeBankingNET6.Services
{
    public interface IAuthService
    {
        Task<bool> Login(string email, string password);
        Task Logout();
        string UserAuthenticated();
    }
}
