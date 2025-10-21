using Microsoft.AspNetCore.Mvc;

namespace Noog_mvc.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
