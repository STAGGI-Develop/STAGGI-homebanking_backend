﻿using HomeBankingNET6.Models;
using System;
using System.Collections.Generic;

namespace HomeBankingNET6.DTOs
{
    public class AccountDTO //Por ahora = a Account.cs (por modificar)...
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }

        public ICollection<TransactionDTO> Transactions { get; set; } 
    }
}
