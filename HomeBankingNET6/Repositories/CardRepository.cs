using HomeBankingNET6.Data;
using HomeBankingNET6.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingNET6.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext) 
        { }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }

        public Card FindById(long id) 
        {
            return FindByCondition(card => card.Id == id).FirstOrDefault();
        }
        public Card FindByNumber(string number)
        {
            return FindByCondition(card => card.Number == number).FirstOrDefault();
        }

        public IEnumerable<Card> GetCardsByClient(long clientId) 
        {
            return FindByCondition(card => card.ClientId == clientId).ToList();
        }
    }
}