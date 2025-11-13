using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.ProjectGroup;
using Noog_mvc.Services;

namespace Noog_mvc.Controllers
{
    [Route("Dashboard/ProjectGroup/{projectGroupId:guid}/[action]")]
    public class ProjectGroupController : ProjectGroupBaseController
    {
        private readonly ProjectGroupService _service;

        public ProjectGroupController(ProjectGroupService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(Guid projectGroupId)
        {
            TopSectionViewModel vm2 = null;
            var response = await _service.GetProjectGroupDataById(projectGroupId);
            var vm = new ProjectGroupViewModel
            {
                TopSection = new TopSectionViewModel
                {
                    GroupName = vm2?.GroupName ?? "Project Alpha",
                    GroupId = id,
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
