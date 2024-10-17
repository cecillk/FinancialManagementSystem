using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementSystem.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController(ITransactionService transactionService) : ControllerBase
    {
        [HttpPost("Create-Transaction")]

        public async Task<IActionResult> CreateAsync(AddTransactionRequest request)
        {
            var response = await transactionService.CreateTransactionAsync(request);

            return ActionResultHelper.ToActionResult(response);
        }

        [HttpGet("Get-Transaction")]
        public async Task<IActionResult> GetTransactionAsync(string TransactionId)
        {
            var response = await transactionService.GetTransactionbyId(TransactionId);

            return ActionResultHelper.ToActionResult(response);
        }

    }
}
