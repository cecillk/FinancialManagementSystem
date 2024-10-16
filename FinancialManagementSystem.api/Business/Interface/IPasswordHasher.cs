namespace FinancialManagementSystem.api.Business.Interface
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string passwordHash, string inputPassword);
    }
}
