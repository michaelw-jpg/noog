using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Services;

namespace Noog_mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<DashboardService>();
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddScoped<RegisterUserService>();
            builder.Services.AddScoped<StorageService>();
            builder.Services.AddScoped<ProjectGroupService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied"; // placeholder for access denied path
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                });

            builder.Services.AddAuthorization();

            builder.Services.AddHttpClient("NoogApi", client =>
            {
            client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            /*  ---- To shorten Guids in url ----

                - Use short GUIDs (e.g., base64url encoded) for the URLs.
                - Map them back to full GUIDs in your controller.
            */

            // Tips: consider attribute routing for your multi-level routes — avoids this ambiguity entirely.
            // [Route("Dashboard/ProjectGroup/{projectGroupId:guid}/[controller]/[action]")]
            // public class ProjectGroupController : Controller

            // Achieves proper URL:s like:
            // /Dashboard/ProjectGroup/9b1deb4d-5b14-4886-9af0-1f7c3e0f3d00/Storage/Summary/2

            // Dashboard-level (user context)
            app.MapControllerRoute(
                name: "dashboard",
                pattern: "Dashboard/{controller=Dashboard}/{action=Index}/{id?}");

            // ProjectGroup context routes
            app.MapControllerRoute(
                name: "projectgroup",
                pattern: "Dashboard/ProjectGroup/{projectGroupId:guid}/{controller=ProjectGroup}/{action=Index}/{id?}");

            // Public Routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
