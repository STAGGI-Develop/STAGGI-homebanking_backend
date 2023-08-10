using HomeBankingNET6.DTOs;
using HomeBankingNET6.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeBankingNET6.Services
{
    public interface ICardService
    {
        public CardDTO CreateCardForCurrentClient(string cardType, string cardColor);
        public List<CardDTO> GetCurrentClientCards();
    }
}
