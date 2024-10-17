using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementSystem.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        [HttpPost("Create-Account")]

        public async Task<IActionResult> CreateAsync(AddAccountResquest request)
        {
            var response = await accountService.CreateAccountAsync(request);

            return ActionResultHelper.ToActionResult(response);
        }


        [HttpDelete("Delete-Account")]

        public async Task<IActionResult> DeleteAsync(string AccountId)
        {
            var response = await accountService.DeleteAccountByIdAsync(AccountId);

            return ActionResultHelper.ToActionResult(response);
        }

        [HttpGet("Get-Account")]
        public async Task<IActionResult> GetAccountAsync(string CustomerId)
        {
            var response = await accountService.GetAccountByCustomerIdAsync(CustomerId);

            return ActionResultHelper.ToActionResult(response);
        }

        [HttpGet("Get-All-Accounts")]
        public async Task<IActionResult> GetAllAccountsAsync(BaseFilter filter)
        {
            var response = await accountService.GetAllAccountsAsync(filter);

            return ActionResultHelper.ToActionResult(response);
        }

        [HttpPut("Update-Account")]

        public async Task<IActionResult> UpdateAccountAsync(UpdateAccountRequest request)
        {
            var response = await accountService.UpdateAccountAsync(request);

            return ActionResultHelper.ToActionResult(response);
        }

    }
}
