using HomeBankingNET6.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingNET6.Services
{
    public interface ITransactionService
    {
        public AccountDTO ProcessTransaction(TransferDTO transferDTO);
    }
}
