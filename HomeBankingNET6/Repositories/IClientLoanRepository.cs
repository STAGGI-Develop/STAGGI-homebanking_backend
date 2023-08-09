using HomeBankingNET6.Models;

namespace HomeBankingNET6.Repositories
{
    public interface IClientLoanRepository
    {
        void Save(ClientLoan clientLoan);
    }
}