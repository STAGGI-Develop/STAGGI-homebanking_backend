using HomeBankingNET6.Models;
using System.Collections.Generic;

namespace HomeBankingNET6.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAll();
        Loan FindById(long id);
    }
}
