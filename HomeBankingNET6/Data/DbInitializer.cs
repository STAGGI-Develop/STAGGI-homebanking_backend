using System;
using System.Linq;
using HomeBankingNET6.Helpers;
using HomeBankingNET6.Models;
using HomeBankingNET6.Enums;

namespace HomeBankingNET6.Data
{
    public class DbInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            var hasher = new PasswordHasher().Hash;
            Random random = new Random();

            //Carga de datos de prueba en nuestra entidad Clients
            //#################################################################################################
            if (!context.Clients.Any())
            {
                Client[] clients = new Client[]
                {
                    new Client { FirstName="Sebastian", LastName="Fabretti", Email = "sf@mail.com", Password=hasher("123456")},
                    new Client { FirstName="Tatiana", LastName="Quarin", Email = "tq@mail.com", Password=hasher("123456")},
                    new Client { FirstName="Andres", LastName="Riveros", Email = "ar@mail.com", Password=hasher("123456")},
                    new Client { FirstName="Gonzalo", LastName="Coradello", Email = "gc@mail.com", Password=hasher("123456")},
                    new Client { FirstName="Gaston", LastName="Rios", Email = "gr@mail.com", Password=hasher("123456")},
                    new Client { FirstName="Ignacio", LastName="Di Bella", Email = "id@mail.com", Password=hasher("123456")}
                };
                foreach (var client in clients)
                {
                    context.Clients.Add(client);
                }
                context.SaveChanges();
            }

            //Carga de datos de prueba en nuestra entidad Accounts:
            //#################################################################################################
            if (!context.Accounts.Any())
            {
                context.Clients.ToList().ForEach(c =>
                {
                    Account newAccount = new Account
                    {
                        ClientId = c.Id,
                        CreationDate = DateTime.Now,
                        Number = $"VIN-{random.Next(100000, 1000000)}",
                        Balance = 1000
                    };
                    context.Accounts.Add(newAccount);
                });
                context.SaveChanges();
            }

            //Carga de datos de prueba en nuestra entidad Transactions:
            //#################################################################################################
            if (!context.Transactions.Any())
            {
                context.Accounts.ToList().ForEach(ac =>
                {
                    context.Transactions.Add(new Transaction
                    {
                        AccountId = ac.Id,
                        Amount = 1500,
                        Date = DateTime.Now.AddHours(-7),
                        Description = "Transferencia Recibida",
                        Type = TransactionType.CREDIT.ToString()
                    }
                    );
                    context.Transactions.Add(new Transaction
                    {
                        AccountId = ac.Id,
                        Amount = -500,
                        Date = DateTime.Now.AddHours(-6),
                        Description = "Compra",
                        Type = TransactionType.DEBIT.ToString()
                    }
                    );
                });
                context.SaveChanges();
            }

            //Carga de datos de prueba en nuestra entidad Loans:
            //#################################################################################################
            if (!context.ClientLoans.Any())
            {
                if (!context.Loans.Any())
                {
                    var loans = new Loan[]
                    {
                        new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                        new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                        new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                    };
                    foreach (Loan loan in loans)
                    {
                        context.Loans.Add(loan);
                    };
                    context.SaveChanges();
                }
                var personalLoan = context.Loans.FirstOrDefault(l => l.Name == "Personal");

                //Carga de datos de prueba en nuestra entidad Client Loans:
                //#################################################################################################
                context.Clients.ToList().ForEach(c =>
                {
                    context.ClientLoans.Add(
                        new ClientLoan
                        {
                            Amount = random.Next(100) * 1000,
                            ClientId = c.Id,
                            LoanId = personalLoan.Id,
                            Payments = "24"
                        }
                    );
                });
                context.SaveChanges();
            }
            
            //Carga de datos de prueba en nuestra entidad Cards:
            //#################################################################################################
            if (!context.Cards.Any())
            {
                context.Clients.ToList().ForEach(c =>
                {
                    (string number, int cvv) = CardDataGeneratorHelper.Generate();
                    context.Cards.Add(
                        new Card
                        {
                            ClientId = c.Id,
                            CardHolder = $"{c.FirstName} {c.LastName}",
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.SILVER.ToString(),
                            Number = number,
                            Cvv = cvv,
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(4),
                        }
                    );
                });
                context.SaveChanges();
            }
        }
    }
}
