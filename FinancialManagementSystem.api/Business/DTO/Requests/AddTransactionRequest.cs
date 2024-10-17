using System.ComponentModel.DataAnnotations;

namespace FinancialManagementSystem.api.Business.DTO.Requests
{
    public class AddTransactionRequest
    {
        [Required]
        public string AccountId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; } 
    }

    public enum TransactionType
    {
        Deposit = 1,
        Withdrawal
    }
}
