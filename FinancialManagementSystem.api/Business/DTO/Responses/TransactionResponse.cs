namespace FinancialManagementSystem.api.Business.DTO.Responses
{
    public class TransactionResponse
    {
        public DateTime DateCreated { get; set; }

        public decimal Amount { get; set; } = 0;

        public string TransactionType { get; set; }

    }
}
