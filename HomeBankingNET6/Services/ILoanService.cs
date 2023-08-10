using HomeBankingNET6.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HomeBankingNET6.Services
{
    public interface ILoanService
    {
        public List<LoanDTO> GetAllLoans();
        public ClientLoanDTO CreateClientLoanForCurrent(LoanApplicationDTO loanApplication);
    }
}
