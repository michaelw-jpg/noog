
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Noog_api.Data;
using Noog_api.Extensions;
using Noog_api.Models;
using Noog_api.Repositories;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services;
using Noog_api.Services.IServices;
using System;
using System.Text;

namespace Noog_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<NoogDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<NoogDbContext>()
            .AddApiEndpoints()
            .AddSignInManager();


            builder.Services.JwtAuth(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<NoogDbContext>();
            builder.Services.AddScoped<ISummaryRepository,SummaryRepository>();
            builder.Services.AddScoped<ISummaryService, SummaryService>();
            builder.Services.AddScoped<IUserService<User>, UserService<User>>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<TokenService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapIdentityApi<User>();
            app.MapControllers();

            app.Run();
        }
    }
}
