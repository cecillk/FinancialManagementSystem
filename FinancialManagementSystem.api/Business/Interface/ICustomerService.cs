using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.DTO.Responses;
using FinancialManagementSystem.api.Data.Entities;

namespace FinancialManagementSystem.api.Business.Interface
{
    public interface ICustomerService
    {
        Task<ServiceResponse<CustomerResponse>> AddCustomerAsync (AddCustomerRequest request);

        Task<ServiceResponse<CustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request);

        Task<ServiceResponse<CustomerResponse>> DeleteCustomerByIdAsync(string customerId);

        Task<ServiceResponse<CustomerResponse>> GetCustomerbyIdAsync(string customerId);

        Task<ServiceResponse<PagedResult<CustomerResponse>>> GetAllCustomersAsync(BaseFilter filter);
    }
}
