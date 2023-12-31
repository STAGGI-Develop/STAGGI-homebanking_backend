﻿using HomeBankingNET6.DTOs;
using HomeBankingNET6.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HomeBankingNET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private ILoanService _loanService;
        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            List<LoanDTO> loansDTO = _loanService.GetAllLoans();
            return Ok(loansDTO);
        }

        [HttpPost()]
        public IActionResult CreateLoanToCurrent([FromBody] LoanApplicationDTO loanApplication)
        {
            ClientLoanDTO newClientLoan = _loanService.CreateClientLoanForCurrent(loanApplication);
            if (newClientLoan == null)
                return Unauthorized();

            return Created("Préstamo creado", newClientLoan);

        }

    }
}
