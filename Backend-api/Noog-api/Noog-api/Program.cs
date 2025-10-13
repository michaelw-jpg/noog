
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Noog_api.Data;
using Noog_api.Extensions;
using Noog_api.Helpers;
using Noog_api.Models;
using Noog_api.Repositories;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services;
using Noog_api.Services.IServices;
using System;
using System.Text;
using System.Threading.Tasks;

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
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            }).AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<NoogDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints()
            .AddSignInManager();


            builder.Services.JwtAuth(builder.Configuration);
            builder.Services.RolePolicy();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<NoogDbContext>();
            builder.Services.AddScoped<ISummaryRepository,SummaryRepository>();
            builder.Services.AddScoped<ISummaryService, SummaryService>();
            builder.Services.AddScoped<IUserService<ApplicationUser>, UserService<ApplicationUser>>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<TokenService>();



            var app = builder.Build();

            await IdentitySeedHelper.SeedAsync(app.Services);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapIdentityApi<ApplicationUser>();
            app.MapControllers();

            app.Run();
        }
    }
}
