﻿using System.Net.Sockets;

namespace HomeBankingNET6.DTOs
{
    public class LoanDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Payments { get; set; }
    }
}
