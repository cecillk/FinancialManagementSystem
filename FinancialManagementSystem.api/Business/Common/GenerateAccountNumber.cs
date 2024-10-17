using FinancialManagementSystem.api.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagementSystem.api.Business.Common
{
    public class GenerateAccountNumber (FinancialDbContext dbContext)
    {
        public string GenerateUniqueAccountNumber()
        {
            // Example simple account number generation logic (can be replaced with more sophisticated logic)
            var random = new Random();
            var accountNumber = random.Next(10000000, 99999999).ToString();

            // Check if account number is unique
            while (dbContext.Accounts.Any(a => a.AccountNumber == accountNumber))
            {
                accountNumber = random.Next(10000000, 99999999).ToString();
            }

            return accountNumber;
        }
    }
}
