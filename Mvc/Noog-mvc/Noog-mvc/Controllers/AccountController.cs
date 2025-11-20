using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.Login;
using Noog_mvc.Models.Register;
using Noog_mvc.Services;
using Noog_mvc.Services.AuthService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Noog_mvc.Controllers
{
    public class AccountController( LoginService loginService, RegisterUserService regiserService, LogoutService logoutService) : Controller
    {
       
        LoginService _loginService = loginService;
        RegisterUserService _registerService = regiserService;
        LogoutService _logoutService = logoutService;

        // GET: LoginController
        public ActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _loginService.LoginAsync(model);

            if (result.Identity != null)
            {

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(result.Identity!),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = result.ExpiresAt
                        });

                var cookies = HttpContext.Response.Headers["Set-Cookie"];
                Console.WriteLine("Set-Cookie header: " + cookies);
                return RedirectToAction("Index", "Dashboard");

            }

            else
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Something went wrong. Please try again later");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }

            var result = await _registerService.RegisterAsync(model);

            if (result.Succsess)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction("Login");
            }
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var result = await _logoutService.LogoutAsync();

            return RedirectToAction("Login");

        }
    }

   
}
