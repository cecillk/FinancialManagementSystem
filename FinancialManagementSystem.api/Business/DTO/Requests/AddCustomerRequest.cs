using FinancialManagementSystem.api.Business.Common;
using System.ComponentModel.DataAnnotations;

namespace FinancialManagementSystem.api.Business.DTO.Requests
{
    public class AddCustomerRequest
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public decimal DepositAmount { get; set; } = 0;

        public AccountType AccountType { get; set; }
    }


    public enum AccountType
    {
        Savings = 1,
        Checkings,
        MoneyMarkets,
        Certificate
    }
}
