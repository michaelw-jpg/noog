using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.ProjectGroup;
using Noog_mvc.Services;

namespace Noog_mvc.Controllers
{
    public class PGDashboardController : Controller
    {
        private readonly DashboardService _service;

        public PGDashboardController(DashboardService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var vm = new PGDashboardViewModel
            {
                TopSection = new TopSectionViewModel
                {
                    GroupName = "Noog-app",
                    //add placholder img
                    GroupImg = ""
                },
                MeetingRoom = new MeetingRoomViewModel
                {
                    IsCallOngoing = true
                },
                ChatRoom = new ChatViewModel
                {
                    UnreadMessage = true
                },
                Storage = new StorageViewModel
                {
                    HasNewItem = false
                }
            };

            return View(vm);
        }

        //Placeholder for functions
        public IActionResult Meeting()
        {
            //TODO add function
            return View();
        }
        public IActionResult Chat()
        {
            //TODO add function
            return View();
        }
        public IActionResult Storage()
        {
            //TODO add function
            return View();
        }
    }
}
