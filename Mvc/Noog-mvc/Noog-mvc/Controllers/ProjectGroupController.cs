using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.ProjectGroup;
using Noog_mvc.Services;

namespace Noog_mvc.Controllers
{
    public class ProjectGroupController : Controller
    {
        private readonly DashboardService _service;

        public ProjectGroupController(DashboardService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var vm = new PGDashboardViewModel
            {
                //add logic and data
            };
            return View(vm);
        }
    }
}
