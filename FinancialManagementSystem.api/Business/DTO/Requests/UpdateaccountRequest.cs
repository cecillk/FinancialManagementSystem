namespace FinancialManagementSystem.api.Business.DTO.Requests
{
    public class UpdateAccountRequest
    {
        public string AccountId { get; set; }
        
        public decimal Balance { get; set; }

        public AccountType AccountType { get; set; }
    }


}
