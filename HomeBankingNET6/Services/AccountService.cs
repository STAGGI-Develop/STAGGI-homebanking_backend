using HomeBankingNET6.DTOs;
using HomeBankingNET6.Helpers;
using HomeBankingNET6.Models;
using HomeBankingNET6.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using static HomeBankingNET6.Services.ServiceExceptions;

namespace HomeBankingNET6.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAuthService _authService;

        public AccountService(IClientRepository clientRepository, IAccountRepository accountRepository, IAuthService authService)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _authService = authService;
        }
        public List<AccountDTO> GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            var accountsDTO = new List<AccountDTO>();

            foreach (Account account in accounts)
            {
                var newAccountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(tr => new TransactionDTO
                    {
                        Id = tr.Id,
                        Type = tr.Type,
                        Amount = tr.Amount,
                        Description = tr.Description,
                        Date = tr.Date,
                    }).ToList()
                };
                accountsDTO.Add(newAccountDTO);
            }
            return accountsDTO;
        }
        public AccountDTO GetAccountById(long id)
        {
            var account = _accountRepository.FindById(id);
            if (account == null)
                return null;
            var accountDTO = new AccountDTO
            {
                Id = account.Id,
                Number = account.Number,
                CreationDate = account.CreationDate,
                Balance = account.Balance,
                Transactions = account.Transactions.Select(tr => new TransactionDTO
                {
                    Id = tr.Id,
                    Type = tr.Type,
                    Amount = tr.Amount,
                    Description = tr.Description,
                    Date = tr.Date,
                }).ToList()
            };
            return accountDTO;
        }
        //[DebuggerNonUserCode]
        public Result<AccountDTO> CreateAccountForCurrentClient()
        {
            string userAuthenticatedEmail = _authService.UserAuthenticated();
            if (userAuthenticatedEmail == null)
            {
                return Result<AccountDTO>.Unauthorized();
            }

            Client currentClient = _clientRepository.FindByEmail(userAuthenticatedEmail);
            if (currentClient == null)
            {
                return Result<AccountDTO>.Failure(new ErrorResponseDTO
                {
                    Status = 404,
                    Error = "Not Found",
                    Message = "No se encontró el cliente"
                });
            }

            if (currentClient.Accounts.Count >= 3)
            {
                return Result<AccountDTO>.Failure(new ErrorResponseDTO
                {
                    Status = 400,
                    Error = "Bad Request",
                    Message = "El cliente alcanzó el máximo de cuentas"
                });
            }

            ///pending check existing accounts number
            Random random = new Random();
            string randomAccountNumber = $"VIN-{random.Next(100000, 1000000)}";

            Account newAccount = new Account
            {
                Number = randomAccountNumber,
                CreationDate = DateTime.Now,
                Balance = 0,
                ClientId = currentClient.Id,
            };
            _accountRepository.Save(newAccount);
                
            var accountDTO = new AccountDTO
            {
                Number = newAccount.Number,
                CreationDate = newAccount.CreationDate,
                Balance = newAccount.Balance,
            };

            return Result<AccountDTO>.Success(accountDTO);
        }
        public List<AccountDTO> GetCurrentClientAccounts()
        {
            string UserAuthenticatedEmail = _authService.UserAuthenticated();
            if (UserAuthenticatedEmail == null) return null;

            Client currentClient = _clientRepository.FindByEmail(UserAuthenticatedEmail);
            if (currentClient == null) throw new ClientNotFoundException();

            var userAccountsDTO = currentClient.Accounts.Select(ac => new AccountDTO
            {
                Id = ac.Id,
                Balance = ac.Balance,
                CreationDate = ac.CreationDate,
                Number = ac.Number
            }).ToList();

            return userAccountsDTO;
        }
    }
}
