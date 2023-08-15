using HomeBankingNET6.DTOs;
using HomeBankingNET6.Enums;
using HomeBankingNET6.Helpers;
using HomeBankingNET6.Models;
using HomeBankingNET6.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeBankingNET6.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;

        public TransactionService(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository, IAuthService authService, IAccountService accountService)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _authService = authService;
            _accountService = accountService;
        }

        public Result<AccountDTO> ProcessTransaction(TransferDTO transferDTO)
        {
            #region Validations
            string userAuthenticatedEmail = _authService.UserAuthenticated();
            if (userAuthenticatedEmail == null) return Result<AccountDTO>.Unauthorized();

            string errorMessage = null;

            if (errorMessage == null && transferDTO.FromAccountNumber == string.Empty || transferDTO.ToAccountNumber == string.Empty)
                errorMessage = "Cuenta de origen o cuenta de destino no proporcionada.";

            if (errorMessage == null && transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
                errorMessage = "No se permite la transferencia a la misma cuenta.";

            if (errorMessage == null && transferDTO.Amount == 0 || transferDTO.Description == string.Empty)
                errorMessage = "Monto o descripción no proporcionados.";

            if (errorMessage != null)
            {
                return Result<AccountDTO>.Failure(new ErrorResponseDTO
                {
                    Status = 403,
                    Error = "Forbidden",
                    Message = errorMessage
                });
            }

            Account fromAccount = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);

            if (errorMessage == null && fromAccount == null)
            { 
                errorMessage = "Cuenta de origen no existe";
            }
            if (errorMessage == null && fromAccount != null)
            {
                Client currentClient = _clientRepository.FindById(fromAccount.ClientId);
                if (currentClient.Email != userAuthenticatedEmail)
                    errorMessage = "Cuenta de origen no pertenece al usuario autenticado";
            }
            if (errorMessage == null && fromAccount?.Balance < transferDTO.Amount) errorMessage = "Fondos insuficientes";

            Account toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);
            if (errorMessage == null && toAccount == null) errorMessage = "Cuenta de destino no existe";

            if (errorMessage != null)
            {
                return Result<AccountDTO>.Failure(new ErrorResponseDTO
                {
                    Status = 403,
                    Error = "Forbidden",
                    Message = errorMessage
                });
            }
            #endregion

            fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;
            toAccount.Balance = toAccount.Balance + transferDTO.Amount;
            _accountRepository.Save(fromAccount);
            _accountRepository.Save(toAccount);

            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.DEBIT.ToString(),
                Amount = transferDTO.Amount * -1,
                Description = $"{transferDTO.Description} {toAccount.Number}",
                AccountId = fromAccount.Id,
                Date = DateTime.Now,
            });

            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.CREDIT.ToString(),
                Amount = transferDTO.Amount,
                Description = $"{transferDTO.Description} {fromAccount.Number}",
                AccountId = toAccount.Id,
                Date = DateTime.Now,
            });

            AccountDTO fromAccountDTO = _accountService.GetAccountById(fromAccount.Id);
            return Result<AccountDTO>.Success(fromAccountDTO);
        }
    }
}
