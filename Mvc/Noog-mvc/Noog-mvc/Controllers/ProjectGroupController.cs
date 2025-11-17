using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.ProjectGroup;
using Noog_mvc.Services;

namespace Noog_mvc.Controllers
{
    [Authorize]
    [Route("Dashboard/ProjectGroup/{projectGroupId:guid}/[action]")]
    public class ProjectGroupController : ProjectGroupBaseController
    {
        private readonly ProjectGroupService _service;
        private readonly CallService _callService;

        public ProjectGroupController(ProjectGroupService service, CallService callService)
        {
            _callService = callService;
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
                    GroupId = projectGroupId,
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

        public async Task<ActionResult> AddUser()
        {
           return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(AddUserToProjectGroup model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    
                    return View(model);
                }
                var success = await _service.AddUserToProjectGroup(model);
                if(success) //success
                {
                    return RedirectToAction("Index", new { projectGroupId = model.ProjectGroupId });
                }
                else
                {
                    ModelState.AddModelError("","failed to add user");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
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
           
            var callLink = _callService.StartCallAsync(ViewBag.ProjectGroupId);
            ViewBag.CallLink = callLink; //maybe work?
            return View();
        }
        public IActionResult Chat()
        {
            //TODO add function
            return View();
        }
    }
}
