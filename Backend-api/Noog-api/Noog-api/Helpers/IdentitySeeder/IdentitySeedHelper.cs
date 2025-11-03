using Microsoft.AspNetCore.Identity;
using Noog_api.Models;

namespace Noog_api.Helpers.IdentitySeeder
{
    public static class IdentitySeedHelper
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { Roles.Admin, Roles.Manager, Roles.User };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole(role));
                }
            }
            var adminEmail = "admin@goon.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(admin, "GoonKing!");

                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {
                    foreach (var error in createAdmin.Errors)
                    {
                        Console.WriteLine($"Error creating admin: {error.Code} - {error.Description}");
                    }
                }
            }
        }
    }
}