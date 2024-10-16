using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.DTO.Responses;
using FinancialManagementSystem.api.Data.Entities;

namespace FinancialManagementSystem.api.Business.Interface
{
    public interface ICustomerServices
    {
        Task<ServiceResponse<CustomerResponse>> AddCustomer(AddCustomerRequest request);

        Task<ServiceResponse<CustomerResponse>> UpdateCustomer(AddCustomerRequest request);

        Task<ServiceResponse<CustomerResponse>> DeleteCustomer();

        Task<ServiceResponse<CustomerResponse>> GetCustomerbyId(string Id);

        Task<ServiceResponse<CustomerResponse>> GetAllCustomers();

    }
}
