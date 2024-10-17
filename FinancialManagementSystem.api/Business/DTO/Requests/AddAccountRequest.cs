using System.ComponentModel.DataAnnotations;

namespace FinancialManagementSystem.api.Business.DTO.Requests
{
    public class AddAccountResquest
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public decimal InitialDeposit { get; set; }

        public AccountType AccountType { get; set; }
    }
}
