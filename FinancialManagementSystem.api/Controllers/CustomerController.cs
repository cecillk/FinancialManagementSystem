using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementSystem.api.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
