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
    public class AccountService(FinancialDbContext dbContext,
        GenerateAccountNumber generateAccountNumber,
        ILogger<AccountService> logger) : IAccountService
    {
        public async Task<ServiceResponse<AccountResponse>> CreateAccountAsync(AddAccountResquest request)
        {
            try
            {
                logger.LogInformation("Creating a new account for customer with Id {Id}", request.CustomerId);

                var existingCustomer = await dbContext.Customers
               .AsNoTracking()
               .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId);

                if (existingCustomer is null)
                {
                    logger.LogDebug("Customer with {customerId} does not exist", request.CustomerId);

                    return ResponseHelper.BadRequestResponse<AccountResponse>("Customer does not exist");
                }

                string accountType = GetAccountType(request.AccountType);

                if (request.InitialDeposit < 0)
                {
                    logger.LogDebug("Initial deposit amount cannot be negative {Deposit}", request.InitialDeposit);

                    return ResponseHelper.BadRequestResponse<AccountResponse>("Amount cannot be less than zero");
                }

                var balance = request.InitialDeposit > 0 ? request.InitialDeposit : 0;

                var newAccount = new Account
                {
                    AccountId = Guid.NewGuid().ToString("N"),
                    AccountNumber = generateAccountNumber.GenerateUniqueAccountNumber(),
                    Balance = balance,
                    AccountType = accountType,
                    CustomerId = request.CustomerId,
                };

                await dbContext.Accounts.AddAsync(newAccount);
                var isSaved = await dbContext.SaveChangesAsync() > 0;

                if (!isSaved)
                {
                    logger.LogDebug("Failed to create account for customer");

                    return ResponseHelper.FailedDependencyResponse<AccountResponse>("Failed to create account for customer");
                }

                logger.LogInformation("Customer deleted successfully");

                return ResponseHelper.OkResponse(newAccount.Adapt<AccountResponse>());
            }
            catch (Exception e)
            {
                logger.LogError(e, "An Error occured while creating account {Ex}", e.Message);

                return ResponseHelper.InternalServerErrorResponse<AccountResponse>("Something really bad happened!. Please try agian later");
            }
        }

        public async Task<ServiceResponse<AccountResponse>> DeleteAccountByIdAsync(string accountId)
        {
            try
            {
                logger.LogInformation("Deleting account with Id {Id}", accountId);

                var accountExists = await dbContext.Accounts
               .AsNoTracking()
               .FirstOrDefaultAsync(a => a.AccountId == accountId);

                if (accountExists is null)
                {
                    logger.LogDebug("Account with Id {accountId} does not exist", accountId);

                    return ResponseHelper.BadRequestResponse<AccountResponse>("Account does not exist");
                }

                dbContext.Accounts.Remove(accountExists);
                bool isSaved = await dbContext.SaveChangesAsync() > 0;

                if (!isSaved) 
                {
                    logger.LogDebug("Failed to delete account");

                    return ResponseHelper.FailedDependencyResponse<AccountResponse>("Failed to delete account");
                }

                logger.LogInformation("Account successfuly deleted");

                return ResponseHelper.OkResponse(accountExists.Adapt<AccountResponse>());
            }
            catch (Exception e)
            {
                logger.LogError(e, "An Error occured while delrting account {Ex}", e.Message);

                return ResponseHelper.InternalServerErrorResponse<AccountResponse>("Something really bad happened!. Please try agian later");
            }
        }

        public async Task<ServiceResponse<AccountResponse>> GetAccountByCustomerIdAsync(string customerId)
        {
            try
            {
                logger.LogInformation("Getting customer by Id {Id}", customerId);

                var accountExists = await dbContext.Accounts
              .AsNoTracking()
              .FirstOrDefaultAsync(a => a.CustomerId == customerId);

                if (accountExists is null)
                {
                    logger.LogDebug("Account with customerId  {customerId} does not exist", customerId);

                    return ResponseHelper.BadRequestResponse<AccountResponse>("Account does not exist");
                }

                logger.LogInformation("Customer with accountId Found {Id}", customerId);

                return ResponseHelper.OkResponse(accountExists.Adapt<AccountResponse>());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting customer by Id {Ex}", e.Message);

                return ResponseHelper.InternalServerErrorResponse<AccountResponse>("Something Really bad happened! Please try again later");
            }
        }

        public async Task<ServiceResponse<PagedResult<AccountResponse>>> GetAllAccountsAsync(BaseFilter filter)
        {
            try
            {
                logger.LogInformation("Getting all accounts from the db");

                var query = dbContext.Accounts.AsNoTracking();

                if (filter.CreatedAt.HasValue)
                {
                    query = query.Where(a => a.CreatedAt == filter.CreatedAt.Value);
                }

                if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                    query = query.Where(a => a.Equals(filter.Search));
                }

                var totalCount = await query.CountAsync();

                var accounts = await query
                    .OrderByDescending(a => a.AccountId)
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(a => new AccountResponse
                    {
                        CustomerId = a.CustomerId,
                        AccountId = a.AccountId,
                        Balance = a.Balance,
                        AccountNumber = a.AccountNumber,
                        AccountType = a.AccountType,
                        CreatedAt = a.CreatedAt,
                    })
                    .ToListAsync();

                var response = new PagedResult<AccountResponse>
                {
                    Page = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalCount = totalCount,
                    Payload = accounts
                };

                return ResponseHelper.OkResponse(response);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting accounts {Ex}", e.Message);

                return ResponseHelper.InternalServerErrorResponse<PagedResult<AccountResponse>>("Something Really bad happened! Please try again later");
            }
        }

        public async Task<ServiceResponse<AccountResponse>> UpdateAccountAsync(UpdateAccountRequest request)
        {
            try
            {
                logger.LogInformation("Updating account by Id {Id}", request.AccountId);

                var updateAccount = await dbContext.Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.AccountId == request.AccountId);

                if (updateAccount is null)
                {
                    logger.LogDebug("Cutomer does not exist or is not active {Request}", request.AccountId)
    ;
                    return ResponseHelper.NotFoundResponse<AccountResponse>("customer not active or is not found");
                }

                updateAccount.AccountType = request.AccountType ?? updateAccount.AccountType;
                

                dbContext.Accounts.Update(updateAccount);
                bool isSaved = await dbContext.SaveChangesAsync() > 0;

                if (!isSaved)
                {
                    logger.LogDebug("failed to saved accounts with update requests");

                    return ResponseHelper.FailedDependencyResponse<AccountResponse>("Failed to update account");
                }

                logger.LogInformation("Account Updated Sucessfully");

                return ResponseHelper.OkResponse(updateAccount.Adapt<AccountResponse>());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error updating account {Ex}", e.Message);

                return ResponseHelper.InternalServerErrorResponse<AccountResponse>("Something really bad happened! Try again later");
            }
        }

        private static string GetAccountType(AccountType accountType)
        {
            return accountType switch
            {
                AccountType.Savings => CommonConstants.AccountType.Savings,
                AccountType.Checkings => CommonConstants.AccountType.Checkings,
                AccountType.MoneyMarkets => CommonConstants.AccountType.MoneyMarkets,
                AccountType.Certificate => CommonConstants.AccountType.Certificate,
                _ => throw new ArgumentException("Invalid Account Type")
            };
        }
    }
}
