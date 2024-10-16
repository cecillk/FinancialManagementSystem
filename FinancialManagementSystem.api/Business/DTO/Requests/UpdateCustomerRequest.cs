using System.ComponentModel.DataAnnotations;

namespace FinancialManagementSystem.api.Business.DTO.Requests
{
    public class UpdateCustomerRequest
    {
        public string? CustomerId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

    }
}
