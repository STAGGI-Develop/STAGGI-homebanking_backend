﻿using HomeBankingNET6.DTOs;
using HomeBankingNET6.Models;
using HomeBankingNET6.Repositories;
using System.Collections.Generic;
using System;
using System.Linq;
using static HomeBankingNET6.Services.ServiceExceptions;
using HomeBankingNET6.Helpers;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using HomeBankingNET6.Enums;

namespace HomeBankingNET6.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthService _authService;
        private readonly IPasswordHasher _passwordHasher;

        public ClientService(IClientRepository clientRepository, IAccountRepository accountRepository, IAuthService authService, IPasswordHasher passwordHasher)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _authService = authService;
            _passwordHasher = passwordHasher;
        }

        public List<ClientDTO> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();
            var clientsDTO = new List<ClientDTO>();

            foreach (Client client in clients)
            {
                var newClientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Type = c.Type,
                        Color = c.Color,
                        Number = c.Number,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        ThruDate = c.ThruDate,
                    }).ToList(),
                };
                clientsDTO.Add(newClientDTO);
            }

            return clientsDTO;
        }

        public ClientDTO GetClientById(long id)
        {
            var client = _clientRepository.FindById(id);
            if (client == null)
                return null;
            ClientDTO clientDTO = new ClientDTO
            {
                Id = client.Id,
                Email = client.Email,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Accounts = client.Accounts.Select(ac => new AccountDTO
                {
                    Id = ac.Id,
                    Balance = ac.Balance,
                    CreationDate = ac.CreationDate,
                    Number = ac.Number
                }).ToList(),
                Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                {
                    Id = cl.Id,
                    LoanId = cl.LoanId,
                    Name = cl.Loan.Name,
                    Amount = cl.Amount,
                    Payments = int.Parse(cl.Payments)
                }).ToList(),
                Cards = client.Cards.Select(c => new CardDTO
                {
                    Id = c.Id,
                    CardHolder = c.CardHolder,
                    Type = c.Type,
                    Color = c.Color,
                    Number = c.Number,
                    Cvv = c.Cvv,
                    FromDate = c.FromDate,
                    ThruDate = c.ThruDate,
                }).ToList(),
            };

            return clientDTO;
        }

        public Result<ClientDTO> CreateClient(CreateClientRequestDTO client)
        {
            try
            {
                bool isNameValid = Regex.IsMatch(client.FirstName, @"^[A-Za-z]{3,}$");
                bool isLastNameValid = Regex.IsMatch(client.LastName, @"^[A-Za-z ]{3,}$");
                if (!isNameValid || !isLastNameValid)
                {
                    return Result<ClientDTO>.Failure(new ErrorResponseDTO
                    {
                        Status = 403, Error = "Forbidden", Message = $"Formato de nombre o apellido no válidos"
                    });
                }

                bool isPasswordValid = Regex.IsMatch(client.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
                if (!isPasswordValid)
                {
                    return Result<ClientDTO>.Failure(new ErrorResponseDTO
                    {
                        Status = 403,
                        Error = "Forbidden",
                        Message = $"Formato de contraseña no válido"
                    });
                }

                var isEmailValid = new EmailAddressAttribute().IsValid(client.Email);
                if (isEmailValid)
                {
                    return Result<ClientDTO>.Failure(new ErrorResponseDTO
                    {
                        Status = 403,
                        Error = "Forbidden",
                        Message = $"Formato de email no válido"
                    });
                }

                ////validamos datos antes
                //if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                //    throw new InvalidRequest();

                ////buscamos si ya existe el usuario
                //if (_clientRepository.FindByEmail(client.Email) != null)
                //    throw new EmailAlreadyUsed();

                string passwordHashed = _passwordHasher.Hash(client.Password);
                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = passwordHashed,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };
                _clientRepository.Save(newClient);
                Client createdClient = _clientRepository.FindByEmail(client.Email);

                ///pending check existing account numbers
                Random random = new Random();
                string randomAccountNumber = $"VIN-{random.Next(100000, 1000000)}";
                Account newAccount = new Account
                {
                    Number = randomAccountNumber,
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = createdClient.Id,
                };
                _accountRepository.Save(newAccount);

                ClientDTO createdClientDTO = GetClientById(createdClient.Id);
                return Result<ClientDTO>.Success(createdClientDTO);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ClientDTO GetCurrentClient()
        {
            try
            {
                string UserAuthenticatedEmail = _authService.UserAuthenticated();
                if (UserAuthenticatedEmail == null) return null;

                Client client = _clientRepository.FindByEmail(UserAuthenticatedEmail);
                if (client == null) throw new ClientNotFoundException();

                ClientDTO clientDTO = GetClientById(client.Id);
                return clientDTO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
