using HomeBankingNET6.Models;

namespace HomeBankingNET6.Repositories
{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        Transaction FindByNumber(long id);
    }
}
