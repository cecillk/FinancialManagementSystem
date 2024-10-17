namespace FinancialManagementSystem.api.Business.Common
{
    public abstract class CommonConstants
    {
        public static class AccountType
        {
            public const string Savings = "Savings Account";
            public const string Checkings = "Checkings Account";
            public const string MoneyMarkets = "Money Markets Account";
            public const string  Certificate = "Certificate of Deposit Account";
        }


        public static class TransactionType
        {
            public const string Deposit = "Deposit";
            public const string Withdraw = "Withdrawal";
        }
    }
}
