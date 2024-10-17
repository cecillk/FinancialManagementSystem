using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.DTO.Responses;
using FinancialManagementSystem.api.Business.Interface;
using FinancialManagementSystem.api.Data;
using FinancialManagementSystem.api.Data.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagementSystem.api.Business.Service
{
    public class TransactionService(
        ILogger<TransactionService> logger,
        FinancialDbContext dbContext
        ) : ITransactionService
    {
        public async Task<ServiceResponse<TransactionResponse>> CreateTransactionAsync(AddTransactionRequest request)
        {
            try
            {
                logger.LogInformation("Creating Transaction");

                var accountExists = await dbContext.Accounts
               .AsNoTracking()
               .FirstOrDefaultAsync(a => a.AccountId == request.AccountId);

                if (accountExists is null)
                {
                    logger.LogDebug("Account with Id {accountId} does not exist", request.AccountId);

                    return ResponseHelper.BadRequestResponse<TransactionResponse>("Account does not exist");
                }

                if (request.Amount < 0)
                {
                    logger.LogDebug("amount cannot be negative {Deposit}", request.Amount);

                    return ResponseHelper.BadRequestResponse<TransactionResponse>("Amount cannot be less than zero");
                }

                decimal newBalance;

                if (request.TransactionType == TransactionType.Deposit)
                {
                    logger.LogInformation($"Depositing: {request.Amount}");

                    newBalance = accountExists.Balance + request.Amount;

                } else if(request.TransactionType == TransactionType.Withdrawal)
                 { 
                    if (accountExists.Balance < request.Amount)
                    {
                        logger.LogDebug("Insufficient account balance");

                        return ResponseHelper.BadRequestResponse<TransactionResponse>("Insufficient account balance");
                    }

                    logger.LogInformation($"Withdrawing: {request.Amount}");

                    newBalance = accountExists.Balance - request.Amount;
                }

                else {

                    logger.LogDebug("Invalid transaction type");

                    return ResponseHelper.BadRequestResponse<TransactionResponse>("Invalid Transaction Type");
                }

                accountExists.Balance = newBalance;

                var transaction = new Transaction
                {
                    Amount = request.Amount,
                    AccountId = request.AccountId,
                    DateCreated = DateTime.UtcNow,
                    TransactionId = Guid.NewGuid().ToString("N"),
                    TransactionType = CommonConstants.TransactionType.Deposit,
                };

                await dbContext.Transactions.AddAsync(transaction);

                bool isSaved = await dbContext.SaveChangesAsync() > 0;

                if (!isSaved)
                {
                    logger.LogDebug("Could not save Transactions to db");

                    return ResponseHelper.FailedDependencyResponse<TransactionResponse>("Could not create transaction");
                }

                logger.LogInformation("Transaction created successfully");

                return ResponseHelper.OkResponse(transaction.Adapt<TransactionResponse>());

            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occure while creating transaction {Ex}", e.Message);

                return ResponseHelper.InternalServerErrorResponse<TransactionResponse>("Something bad happened. Try again");
            }
        }

        public async Task<ServiceResponse<Transaction>> GetTransactionbyId(string TransactionId)
        {
            try
            {
                logger.LogInformation("Getting Transaction by Id {Id}", TransactionId);

                var existingTransaction = await dbContext.Transactions
                   .AsNoTracking()
                   .FirstOrDefaultAsync(c => c.TransactionId == TransactionId);

                if (existingTransaction is null)
                {
                    logger.LogDebug("Transaction with {TransactionId} does not exist", TransactionId);

                    return ResponseHelper.BadRequestResponse<Transaction>("Transaction does not exist");
                }

                logger.LogInformation("Transaction with Id Found {Id}", TransactionId);

                return ResponseHelper.OkResponse(existingTransaction);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting Transaction by Id {Ex}", e.Message);

                return ResponseHelper.InternalServerErrorResponse<Transaction>("Something Really bad happened! Please try again later");
            }
        }
    }
}
