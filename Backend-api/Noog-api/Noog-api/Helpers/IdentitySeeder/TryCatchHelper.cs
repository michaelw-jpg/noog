using Microsoft.Extensions.Logging;

namespace Noog_api.Helpers.IdentitySeeder
{
    public static class TryCatchHelper
    {
        public static async Task TryCatchIdentitySeeder(IServiceProvider services, bool isDevelopment)
        {
            // Logging and seeding happens inside the same scope
            using var scope = services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("IdentitySeeder");
            
            try
            {
                using (logger.BeginScope("Running Identity Seeder"))
                {
                    await IdentitySeedHelper.SeedAsync(scopedServices);
                }
                logger.LogInformation("--> Identity Seeding successful");
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex, "--> Database connection failed due to config issues or missing database.");
                logger.LogWarning("Check your appsettings.json and/or appsettings.Development.json connection strings.");
                throw;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                logger.LogError(ex, "--> SQL connection failed.");
                logger.LogError("If IP adress issue, go to Azure Portal => NoogDB SQL Server => Networking.");
                logger.LogError("Add firewall rule/allow azure services. Wait ~5 minutes.");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "--> Unexpected Error during Identity Seeding.");
                throw;
            }
            finally
            {
                if (isDevelopment)
                {
                    logger.LogInformation("--> TIP: If database access isn't required for your changes, feel free to temporarily comment out:");
                    logger.LogInformation("await TryCatchHelper.TryCatchIdentitySeeder(app.Services, app.Environment.IsDevelopment());");
                }
                else
                {
                    // TODO - Production handling only if needed
                }
            }
        }
    }
}
