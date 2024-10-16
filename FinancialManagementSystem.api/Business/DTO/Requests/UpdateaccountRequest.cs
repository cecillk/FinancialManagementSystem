namespace FinancialManagementSystem.api.Business.DTO.Requests
{
    public class UpdateaccountRequest
    {
        public string Accounttype { get; set; }

        public decimal Balance { get; set; } = 0;
    }
}
