using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementSystem.api.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
