using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.ProjectGroup;
using Noog_mvc.Services;

namespace Noog_mvc.Controllers
{
    public class PGDashboardController : Controller
    {
        private readonly PGDashboardService _service;

        public PGDashboardController(PGDashboardService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            TopSectionViewModel vm2 = null;            
            // await _service.GetProjectGroupDataById(id);
            var vm = new PGDashboardViewModel
            {
                TopSection = new TopSectionViewModel
                {
                    GroupName = vm2?.GroupName ?? "Project Alpha",
                    //add placholder img
                    GroupImg = vm2?.GroupImg ?? ""
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

        public async Task <ActionResult> Create()
        {
            return View();
        }

        // POST: pgController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
    }
}
