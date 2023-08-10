using HomeBankingNET6.DTOs;
using System.Collections.Generic;

namespace HomeBankingNET6.Services
{
    public interface IAccountService
    {
        public List<AccountDTO> GetAllAccounts();
        public AccountDTO GetAccountById(long id);
        public AccountDTO CreateAccountForCurrentClient();
        public List<AccountDTO> GetCurrentClientAccounts();
    }
}
