namespace FinancialManagementSystem.api.Data.Entities
{
    public class Transaction
    {
        public string TransactionId { get; set; }

        public DateTime DateCreated { get; set; }

        public decimal Amount { get; set; } = 0;

        public string TransactionType { get; set; }

        public string AccountId { get; set; }
        public Account Account { get; set; }
    }
}
