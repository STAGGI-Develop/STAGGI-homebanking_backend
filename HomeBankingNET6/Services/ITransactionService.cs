using HomeBankingNET6.DTOs;
using HomeBankingNET6.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingNET6.Services
{
    public interface ITransactionService
    {
        public Result<AccountDTO> ProcessTransaction(TransferDTO transferDTO);
    }
}
