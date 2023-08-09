﻿using System.Collections;
using System.Collections.Generic;

namespace HomeBankingNET6.Models
{
    public class Loan
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Payments { get; set; }

        public ICollection<ClientLoan> ClientLoans { get; set; }
    }
}
