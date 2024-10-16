using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.DTO.Responses;
using FinancialManagementSystem.api.Data.Entities;

namespace FinancialManagementSystem.api.Business.Interface
{
    public interface IAccountServicecs
    {
        Task<ServiceResponse<AccountResponse>> AddAccount(AddaccountResquest request);

        Task<ServiceResponse<AccountResponse>> UpdateAccount(UpdateaccountRequest request);

        Task<ServiceResponse<AccountResponse>> DeleteAccountbyId(string Id);

        Task<ServiceResponse<AccountResponse>> GetAccountbyId(string Id);

        Task<ServiceResponse<AccountResponse>> GetAllAccountBalance(string Id);
    }
}
