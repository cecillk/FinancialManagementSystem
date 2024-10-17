using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.DTO.Responses;
using FinancialManagementSystem.api.Data.Entities;


namespace FinancialManagementSystem.api.Business.Interface
{
    public interface ITransactionService
    {
        Task<ServiceResponse<TransactionResponse>> CreateTransactionAsync(AddTransactionRequest request);

        Task<ServiceResponse<Transaction>> GetTransactionbyId(string TransactionId);
    }
}
