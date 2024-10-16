using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.DTO.Responses;
using FinancialManagementSystem.api.Business.Interface;
using FinancialManagementSystem.api.Data;
using FinancialManagementSystem.api.Data.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;

public class CustomerService(
    ILogger<CustomerService> logger,
    FinancialDbContext dbContext,
    IPasswordHasher passwordHasher
    ) : ICustomerService
{
    public async Task<ServiceResponse<CustomerResponse>> AddCustomerAsync(AddCustomerRequest request)
    {
        try
        {
            logger.LogInformation("Creating a new customer");

            var existingCustomer = await dbContext.Customers.AsNoTracking().AnyAsync(c => c.PhoneNumber == request.PhoneNumber && c.Email == request.Email);

            if (existingCustomer)
            {
                logger.LogDebug("Customer with {PhoneNumber} already exists", request.PhoneNumber);

                return ResponseHelper.BadRequestResponse<CustomerResponse>("Customer with PhoneNumber already exists");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                logger.LogDebug("Password is required.\r\nAddCustomerRequest: {Request}", request.Password);

                return ResponseHelper.BadRequestResponse<CustomerResponse>("Password is required");
            }

            var passwordHash = passwordHasher.HashPassword(request.Password);

            var newCustomer = new Customer
            {
                CustomerId = Guid.NewGuid().ToString("N"),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = passwordHash,
                DateOfBirth = request.DateOfBirth,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            string accountType = GetAccountType(request.AccountType);

            var account = new Account
            {
                AccountId = Guid.NewGuid().ToString("N"),
                AccountNumber = GenerateUniqueAccountNumber(),
                Balance = 0,
                AccountType = accountType,
                CustomerId = newCustomer.CustomerId,
                CreatedAt = DateTime.UtcNow,    
            };

            if (request.DepositAmount > 0)
            {
                var initialTransaction = new Transaction
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    Amount = request.DepositAmount,
                    DateCreated = DateTime.UtcNow,
                    TransactionType = "Deposit",
                    AccountId = account.AccountId
                };

                account.Transactions = new List<Transaction> { initialTransaction };
            }

            newCustomer.Accounts = new List<Account> { account };

            await dbContext.Customers.AddAsync(newCustomer);
            var isSaved = await dbContext.SaveChangesAsync() > 0;

            if (!isSaved) {
                logger.LogDebug("Error Saving customer to database");

                return ResponseHelper.FailedDependencyResponse<CustomerResponse>("Failed to create customer");
            }

            logger.LogInformation("Customer Created succesfully");

            return ResponseHelper.OkResponse<CustomerResponse>(newCustomer.Adapt<CustomerResponse>());

        }
        catch (Exception e)
        {
            logger.LogError(e, "Error adding new  customer {Ex}", e.Message);

            return ResponseHelper.InternalServerErrorResponse<CustomerResponse>("Something really bad happened! Try again later");
        }
    }

    public async Task<ServiceResponse<CustomerResponse>> DeleteCustomerByIdAsync(string customerId)
    {
        try
        {
            logger.LogInformation("About to delete customer with {Id}", customerId);

            var deleteCustomer = await dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerId == customerId);


            if (deleteCustomer is null)
            {
                logger.LogDebug("Customer with {customerId} does not exist", customerId);

                return ResponseHelper.BadRequestResponse<CustomerResponse>("Customer does not exist");
            }

            dbContext.Customers.Remove(deleteCustomer);

            bool isSaved = await dbContext.SaveChangesAsync() > 0;

            if (!isSaved)
            {
                logger.LogDebug("Failed to delete customer");

                return ResponseHelper.FailedDependencyResponse<CustomerResponse>("Failed to delete customer");
            }

            logger.LogInformation("Customer deleted successfully");

            return ResponseHelper.OkResponse(deleteCustomer.Adapt<CustomerResponse>());

        }
        catch (Exception e)
        {

            logger.LogError(e, "Error deleting customer. {Ex}", e.Message);

            return ResponseHelper.InternalServerErrorResponse<CustomerResponse>("Something really bad happened! Try again later");
        }
    }

    public async Task<ServiceResponse<PagedResult<CustomerResponse>>> GetAllCustomersAsync(BaseFilter filter)
    {
        try
        {
            logger.LogInformation("Getting all customers from the db");

            var query = dbContext.Customers.AsNoTracking();

            if (filter.CreatedAt.HasValue)
            {
                query = query.Where(c=> c.CreatedAt == filter.CreatedAt.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(c=> c.Equals(filter.Search));
            }

            var totalCount = await query.CountAsync();

            var customers = await query
                .OrderByDescending(c => c.CustomerId)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(c => new CustomerResponse
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    DateOfBirth = c.DateOfBirth
                })
                .ToListAsync();

            var response = new PagedResult<CustomerResponse>
            {
                Page = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                Payload = customers
            };

            return ResponseHelper.OkResponse(response);
        }
        catch(Exception e) 
        {
            logger.LogError(e, "Error getting customers {Ex}", e.Message);

            return ResponseHelper.InternalServerErrorResponse<PagedResult<CustomerResponse>>("Something Really bad happened! Please try again later");
        }
    }

    public async Task<ServiceResponse<Customer>> GetCustomerbyIdAsync(string customerId)
    {
        try
        {
            logger.LogInformation("Getting customer by Id {Id}", customerId);

             var existingCustomer = await dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (existingCustomer is null)
            {
                logger.LogDebug("Customer with {customerId} does not exist", customerId);

                return ResponseHelper.BadRequestResponse<Customer>("Customer does not exist");
            }

            logger.LogInformation("Customer with Id Found {Id}", customerId);

            return ResponseHelper.OkResponse(existingCustomer);
        }
        catch(Exception e)
        {
            logger.LogError(e, "Error getting customer by Id {Ex}", e.Message);

            return ResponseHelper.InternalServerErrorResponse<Customer>("Something Really bad happened! Please try again later");
        }
    }

    public async Task<ServiceResponse<CustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request)
    {
        try
        {
            logger.LogInformation("Updating Customer by Id {Id}", request.CustomerId);

            var updateCustomer = await dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId && c.IsActive);

            if (updateCustomer is null)
            {
                logger.LogDebug("Cutomer does not exist or is not active {Request}", request.CustomerId)
;
                return ResponseHelper.NotFoundResponse<CustomerResponse>("customer not active or is not found");
            }

            updateCustomer.PhoneNumber = request.PhoneNumber ?? updateCustomer.PhoneNumber;
            updateCustomer.DateOfBirth = request.DateOfBirth ?? updateCustomer.DateOfBirth;
            updateCustomer.FirstName = request.FirstName ?? updateCustomer.FirstName;
            updateCustomer.LastName = request.LastName ?? updateCustomer.LastName;
            updateCustomer.Email = request.Email ?? updateCustomer.Email;

            dbContext.Customers.Update(updateCustomer);
            bool isSaved = await dbContext.SaveChangesAsync() > 0;

            if (!isSaved)
            {
                logger.LogDebug("failed to saved customers with update requests");

                return ResponseHelper.FailedDependencyResponse<CustomerResponse>("Failed to update customer");
            }

            logger.LogInformation("Customer Updated Sucessfully");

            return ResponseHelper.OkResponse(updateCustomer.Adapt<CustomerResponse>());
        }
        catch(Exception e)
        {
            logger.LogError(e, "Error updating customer {Ex}", e.Message);

            return ResponseHelper.InternalServerErrorResponse<CustomerResponse>("Something really bad happened! Try again later");
        }
    }


    private string GenerateUniqueAccountNumber()
    {
        // Example simple account number generation logic (can be replaced with more sophisticated logic)
        var random = new Random();
        var accountNumber = random.Next(10000000, 99999999).ToString();

        // Check if account number is unique
        while (dbContext.Accounts.Any(a => a.AccountNumber == accountNumber))
        {
            accountNumber = random.Next(10000000, 99999999).ToString();
        }

        return accountNumber;
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
