﻿using HomeBankingNET6.Models;
using System.Collections.Generic;

namespace HomeBankingNET6.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);
        IEnumerable<Account> GetAccountsByClient(long accountId); //Obtener cuentas por cliente.
        Account FindByNumber(string number); //SELECT * FROM <Accounts> Where Number = number.
    }
}
