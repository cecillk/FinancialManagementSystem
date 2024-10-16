using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.DTO.Responses;
using FinancialManagementSystem.api.Data.Entities;

namespace FinancialManagementSystem.api.Business.Interface
{
    public interface ITransactionServices
    {
        Task<ServiceResponse<TransactionResponse>> AddTransaction(AddTransactionRequest request);

        Task<ServiceResponse<TransactionResponse>> GetTransactionbyId(string Id);
    }
}
