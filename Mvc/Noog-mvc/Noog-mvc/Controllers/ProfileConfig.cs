using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Services;
using Noog_mvc.Models.User;

namespace Noog_mvc.Controllers
{
    public class ProfileConfig : Controller
    {
        private readonly ProfileConfigService _service;

        public async Task<IActionResult> Index()
        {
            //TODO: Implement ProfileConfigService and GetUserProfileAsync method
            //var user = await _service.GetUserProfileByLogin();

            var user = new UserViewModel
            {
                Username = "Oliver",
                Email = "meow",
                FirstName = "John",
                LastName = "Doe",
                Password="123456SimonEke"
            };

            return View(user);
        }

        public async Task<IActionResult> UpdateProfile(UserPatchDto userPatch)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateUserProfileAsync(userPatch);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Failed to update profile.");
            }
            return View("Index", userPatch);
        }


    }
}
