using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementSystem.api.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
