using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.DTO.Responses;

namespace FinancialManagementSystem.api.Business.Interface
{
    public interface IAccountService
    {
        Task<ServiceResponse<AccountResponse>> CreateAccountAsync(AddAccountResquest request);

        Task<ServiceResponse<AccountResponse>> UpdateAccountAsync(UpdateAccountRequest request);

        Task<ServiceResponse<AccountResponse>> DeleteAccountByIdAsync(string accountId);

        Task<ServiceResponse<AccountResponse>> GetAccountByCustomerIdAsync(string customerId);

        Task<ServiceResponse<PagedResult<AccountResponse>>> GetAllAccountsAsync(BaseFilter filter);
    }
}
