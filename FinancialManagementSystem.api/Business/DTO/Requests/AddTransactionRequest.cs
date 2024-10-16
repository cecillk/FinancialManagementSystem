namespace FinancialManagementSystem.api.Business.DTO.Requests
{
    public class AddTransactionRequest
    {
        public decimal Amount { get; set; } = 0;

        public string TransactionType { get; set; }
    }
}
