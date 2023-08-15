using HomeBankingNET6.DTOs;
using HomeBankingNET6.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HomeBankingNET6.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("accounts")]
        public IActionResult Get()
        {
            List<AccountDTO> accountsDTO = _accountService.GetAllAccounts();
            return Ok(accountsDTO);
        }

        [HttpGet("accounts/{id}")]
        public IActionResult Get(long id)
        {
            AccountDTO accountDTO = _accountService.GetAccountById(id);
            if (accountDTO == null)
                return NoContent();

            return Ok(accountDTO);
        }

        [HttpPost("clients/current/accounts")]
        public IActionResult CreateAccountToCurrent()
        {
            var result = _accountService.CreateAccountForCurrentClient();

            if (!result.IsSuccess)
            {
                return StatusCode(result.Error.Status, result.Error);
            }

            return StatusCode(201, result.Ok);
        }

        [HttpGet("clients/current/accounts")]
        public IActionResult GetCurrentAccounts()
        {
            List<AccountDTO> userAccountsDTO = _accountService.GetCurrentClientAccounts();
            if (userAccountsDTO == null)
                return Unauthorized();

            return Ok(userAccountsDTO);
        }

    }
}
