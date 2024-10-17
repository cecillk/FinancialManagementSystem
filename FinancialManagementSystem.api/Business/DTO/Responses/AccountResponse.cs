namespace FinancialManagementSystem.api.Business.DTO.Responses
{
    public class AccountResponse
    {
        public string AccountId { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; } = 0;
        public string AccountNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerId { get; set; }
    }
}
