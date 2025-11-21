using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Helpers;
using Noog_mvc.Services;

namespace Noog_mvc.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardService _service;

        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = await _service.GetDashboardDataAsync();

            return View(model);
        }
    }
}
