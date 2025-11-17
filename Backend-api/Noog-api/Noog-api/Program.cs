
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Noog_api.Data;
using Noog_api.Extensions;
using Noog_api.Helpers.IdentitySeeder;
using Noog_api.Models;
using Noog_api.Middlewares;
using Noog_api.Models;
using Noog_api.Repositories;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services;
using Noog_api.Services.IServices;
using StreamChat.Clients;
using System;
using System.Text;
using System.Threading.Tasks;
using Noog_api.Services.Dashboard;

namespace Noog_api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<NoogDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            }).AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<NoogDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints()
            .AddSignInManager();
            builder.Services.AddScoped<StreamIOService>();

            builder.Services.Configure<StreamIOService>(builder.Configuration.GetSection("StreamIO"));

            //Add singleton StreamClientFactory 
            builder.Services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();

                var StreamIOApiKey = config["StreamIo:ApiKey"];

                var streamApiSecret = config["StreamIo:ApiSecret"];

                return new StreamClientFactory(StreamIOApiKey, streamApiSecret);
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.SlidingExpiration = true;
            });

            builder.Services.JwtAuth(builder.Configuration);
            builder.Services.RolePolicy();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<NoogDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IOpenAiService, OpenAiService>();
            builder.Services.AddScoped<IOpenAiPromptService, OpenAiPromptService>();
            builder.Services.AddScoped<ISummaryRepository,SummaryRepository>();
            builder.Services.AddScoped<ISummaryService, SummaryService>();
            builder.Services.AddScoped<IUserService<ApplicationUser>, UserService<ApplicationUser>>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IRecentGroupActivityService, RecentGroupActivityService>();
            builder.Services.AddScoped<IProjectGroupUserService, ProjectGroupUserService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<IProjectGroupUserRepo, ProjectGroupUserRepo>();
            builder.Services.AddScoped<IRecentGroupActivityRepo, RecentGroupActivityRepo>();
            builder.Services.AddScoped<IProjectGroupService, ProjectGroupService>();
            builder.Services.AddScoped<IProjectGroupRepository, ProjectGroupRepository>();

            builder.Services.AddScoped<IGroupMeetingService, GroupMeetingService>();
            builder.Services.AddScoped<IGroupMeetingRepo, GroupMeetingRepo>();

            builder.Services.AddScoped<IGroupStorageService, GroupStorageService>();
            builder.Services.AddScoped<IGroupStorageRepo, GroupStorageRepo>();
            
            builder.Services.AddScoped<IProjectGroupService, ProjectGroupService>();
            builder.Services.AddScoped<IProjectGroupRepository, ProjectGroupRepository>();


            builder.Services.AddHttpClient<AssemblyAiService>();



            var app = builder.Build();

            // Identity Seeder
            // Writes helpful messages to the console if failings occur
            await TryCatchHelper.TryCatchIdentitySeeder(app.Services, app.Environment.IsDevelopment());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(options => { });

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<UserIdMiddleware>();

            app.MapIdentityApi<ApplicationUser>();
            app.MapControllers();

            app.Run();
        }
    }
}
