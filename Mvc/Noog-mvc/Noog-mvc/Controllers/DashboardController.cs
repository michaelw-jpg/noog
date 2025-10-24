using Microsoft.AspNetCore.Mvc;

namespace Noog_mvc.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
