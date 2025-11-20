using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Noog_mvc.Models.ProjectGroup;
using Noog_mvc.Models.ProjectGroup.Dtos;
using Noog_mvc.Services;
using System.Reflection;

namespace Noog_mvc.Controllers
{
    [Authorize]
    [Route("Dashboard/ProjectGroup/{projectGroupId:guid}/{action}")]
    public class ProjectGroupController : ProjectGroupBaseController
    {
        private readonly ProjectGroupService _service;
        private readonly IMemoryCache _cache;
        private readonly CallService _callService;

        public ProjectGroupController(ProjectGroupService service, CallService callService, IMemoryCache cache)
        {
            _service = service;
            _cache = cache;
            _callService = callService;
        }

        public async Task<IActionResult> Index(Guid projectGroupId)
        {
            var response = await _service.GetProjectGroupDataById(projectGroupId);
            
            var vm = new ProjectGroupViewModel
            {
                TopSection = new TopSectionViewModel
                {
                    GroupId = response.ProjectGroup.GroupId,
                    GroupName = response.ProjectGroup.GroupName,
                    GroupImg = response.ProjectGroup.GroupImg
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

        [HttpGet]
        [Route("/Dashboard/ProjectGroup/Create")]
        public async Task<ActionResult> Create(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: pgController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Dashboard/ProjectGroup/Create")]
        public async Task<ActionResult> Create(ProjectGroupCreate model, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ReturnUrl = returnUrl;
                    return View(model);
                }

                var success = await _service.CreateGroupProject(model);

                if (success)
                {
                    // invalidate the sidenavbar's cache so that it refreshes.
                    _cache.Remove("sidebar-projects");

                    if (!string.IsNullOrEmpty(returnUrl)
                        && Url.IsLocalUrl(returnUrl)
                        )
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // shouldnt it return to the previous view showed? since you can click the add groupproject from anywhere
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("ModelOnly", "Failed to Create ProjectGroup");
                    ViewBag.ReturnUrl = returnUrl;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ModelOnly", $"An error occured: {ex.Message}");
                return View(model);
            }
        }

        //Placeholder for functions
        public async Task<IActionResult> Meeting()
        {
           
            var callLink = await _callService.StartCallAsync(ViewBag.ProjectGroupId);
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
