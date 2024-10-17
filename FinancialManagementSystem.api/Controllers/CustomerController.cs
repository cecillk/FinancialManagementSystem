using FinancialManagementSystem.api.Business.Common;
using FinancialManagementSystem.api.Business.DTO.Requests;
using FinancialManagementSystem.api.Business.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementSystem.api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        [HttpPost("Create-Customer")]

        public async Task<IActionResult> CreateAsync(AddCustomerRequest request)
        {
            var response = await customerService.AddCustomerAsync(request);

            return ActionResultHelper.ToActionResult(response);
        }


        [HttpDelete("Delete-Customer")]

        public async Task<IActionResult> DeleteAsync(string CustomerId)
        {
            var response = await customerService.DeleteCustomerByIdAsync(CustomerId);

            return ActionResultHelper.ToActionResult(response);
        }

        [HttpGet("Get-Customer")]
        public async Task<IActionResult> GetCustomerAsync(string customerId)
        {
            var response = await customerService.GetCustomerbyIdAsync(customerId);

            return ActionResultHelper.ToActionResult(response);
        }

        [HttpGet("Get-All-Customers")]
        public async Task<IActionResult> GetAllCustomersAsync(BaseFilter filter)
        {
            var response = await customerService.GetAllCustomersAsync(filter);

            return ActionResultHelper.ToActionResult(response);
        }

        [HttpPut("Update-Customer")]

        public async Task<IActionResult> UpdateCustomerAsync(UpdateCustomerRequest request)
        {
            var response = await customerService.UpdateCustomerAsync(request);

            return ActionResultHelper.ToActionResult(response);
        }

    }
}
