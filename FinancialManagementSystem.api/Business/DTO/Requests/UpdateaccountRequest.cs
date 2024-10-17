namespace FinancialManagementSystem.api.Business.DTO.Requests
{
    public class UpdateAccountRequest
    {
        public string AccountId { get; set; }
        public string AccountType { get; set; }

        public decimal Balance { get; set; } = 0;
    }
}
