using HomeBankingNET6.DTOs;
using HomeBankingNET6.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingNET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransferDTO transferDTO)
        {
            AccountDTO fromAccountsDTO = _transactionService.ProcessTransaction(transferDTO);
            if (fromAccountsDTO == null)
                return Unauthorized();

            return Created("Transacción exitosa", fromAccountsDTO);
        }
    }
}
