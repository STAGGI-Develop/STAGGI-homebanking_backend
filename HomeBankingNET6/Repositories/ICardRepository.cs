using HomeBankingNET6.Models;
using System.Collections;
using System.Collections.Generic;

namespace HomeBankingNET6.Repositories
{
    public interface ICardRepository
    {
        void Save(Card card);
        Card FindById(long id);
        Card FindByNumber(string number);
        IEnumerable<Card> GetCardsByClient(long clientId);
    }
}
