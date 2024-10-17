using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementSystem.api.Business.Common
{
    public static class ActionResultHelper
    {
        public static IActionResult ToActionResult<T>(ServiceResponse<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.Code
            };
        }
    }
}
