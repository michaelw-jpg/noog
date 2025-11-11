using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.Login;
using Noog_mvc.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Noog_mvc.Controllers
{
    public class AccountController( LoginService loginService) : Controller
    {
       
        LoginService _loginService = loginService;

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

            switch (result.Status)
            {
                case LoginStatus.Success:
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(result.Identity!),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = result.ExpiresAt
                        });
                    return RedirectToAction("Index", "Dashboard");

                case LoginStatus.InvalidCredentials:
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    break;

                case LoginStatus.ServerError:
                    ModelState.AddModelError(string.Empty, "Something went wrong. Please try again later.");
                    break;
            }

            return View(model);
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }

   
}
