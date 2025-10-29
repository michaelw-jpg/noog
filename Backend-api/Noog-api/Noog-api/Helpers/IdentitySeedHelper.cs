using Microsoft.AspNetCore.Identity;
using Noog_api.Models;

namespace Noog_api.Helpers
{
    public static class IdentitySeedHelper
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            try
            {
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
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Database connection failed due to config issues or missing database.");
                Console.WriteLine(" --> Check your appsettings.json and/or appsettings.Development.json connection strings.");
                Console.WriteLine($" Details: {ex.Message}");
                Console.ResetColor();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("SQL connection failed.");
                Console.WriteLine(" --> If IP adress issue, go to Azure Portal => NoogDB SQL Server => Networking. Add firewall rule/allow azure services. Wait ~5 minutes.");
                Console.WriteLine($" Details: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Unexpected Error during Identity Seeding:");
                Console.WriteLine($" Details: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("If database access isn't required for your changes, feel free to temporarily comment out:");
                Console.WriteLine($"    --> await IdentitySeedHelper.SeedAsync(app.Services);");
                Console.ResetColor();
            }
        }
    }
}