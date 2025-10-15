using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs.Auth;
using Noog_api.Models;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService<ApplicationUser> _users;
        private readonly TokenService _tokens;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(IUserService<ApplicationUser> users, TokenService tokens, SignInManager<ApplicationUser> signInManager)
        {
            _users = users;
            _tokens = tokens;
            _signInManager = signInManager;
        }

        public async Task<LoginResponseDto> RegisterAsync(RegisterDto dto)
        {
            var exist = await _users.FindByEmailAsync(dto.Email);
            if (exist != null)
                throw new InvalidOperationException("Email already exists");

            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.UserName ?? dto.Email
            };

            var result = await _users.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException("User could not be created");


            var token = await _tokens.CreateToken(user);
            return new LoginResponseDto
            {
                Token = token
            };
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _users.FindByEmailAsync(dto.Email);
            if (user is null)
                throw new NullReferenceException("USer can not be null");

            var success = await _users.CheckPasswordAsync(user, dto.Password, lockoutOnFailure: true);
            if (!success.Succeeded)
                throw new ArgumentException("Email or Password was incorrect");

            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(15);

            var token = await _tokens.CreateToken(user);
            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt
            };
        }
        
        public async Task LogoutAsync()
        {
           await _signInManager.SignOutAsync();
        }
    }
}
