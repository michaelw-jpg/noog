using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Services;
using Noog_mvc.Models.User;

namespace Noog_mvc.Controllers
{
    public class ProfileConfigController : Controller
    {
        private readonly ProfileConfigService _service;

        public ProfileConfigController( ProfileConfigService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            //TODO: Implement ProfileConfigService and GetUserProfileAsync method
            //var user = await _service.GetUserProfileByLogin();

            //Temp
            var getCurrentUserLogin = HttpContext.User.Identity.Name;
            

            return View(getCurrentUserLogin);
        }

        public async Task<IActionResult> UpdateProfile(UserPatchDto userPatch)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateUserProfileAsync(userPatch);
                if (result)
                {
                    TempData["SuccessMessage"] = "Your profile has been updated successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Failed to update profile.");
            }
            return View("Index", userPatch);
        }


    }
}
