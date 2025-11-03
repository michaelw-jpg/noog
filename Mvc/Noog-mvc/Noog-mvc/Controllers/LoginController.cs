using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Noog_mvc.Controllers
{
    public class LoginController(IHttpClientFactory httpClientFactory, IConfiguration config) : Controller
    {
       
        IConfiguration _config = config;
        HttpClient client = httpClientFactory.CreateClient("NoogApi");

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

            var response = await client.PostAsJsonAsync("auth/login", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Login failed");
                return View(model);
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginResponse!.UserName),
                new Claim("AccessToken", loginResponse.Token)
            };
            
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(loginResponse.Token);
            var roleClaims = jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => new Claim(ClaimTypes.Role, c.Value));

            claims.AddRange(roleClaims);

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = loginResponse.ExpiresAt
                });

            return RedirectToAction("Index", "Home");

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

    public class LoginResponse
    {
        public string UserName { get; set; }
        public required string Token { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }

    }
}
