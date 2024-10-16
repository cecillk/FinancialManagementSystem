namespace FinancialManagementSystem.api.Data.Entities
{
    public class Account
    {
        public string AccountId { get; set; }

        public string Accounttype { get; set; }

        public decimal Balance { get; set; } = 0;   

        public string AccountNumber { get; set; }
        
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

    }
}
