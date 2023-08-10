namespace HomeBankingNET6.Helpers
{
    public interface IPasswordHasher
    {
        bool Verify(string passwordHashed, string inputPassword);
        string Hash(string password);
    }
}
