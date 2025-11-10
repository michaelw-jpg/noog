using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs.Auth;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Helpers;
using Noog_api.Models;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;
using System.Data;

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

        public async Task<BaseResponseDto<LoginResponseDto>> RegisterAsync(RegisterDto dto)
        {
            var finalResponse = new BaseResponseDto<LoginResponseDto>();
            var exist = await _users.FindByEmailAsync(dto.Email);
            if (exist != null)
            {
                finalResponse.StatusCode = Enums.StatusCodesEnum.BadRequest;
                finalResponse.Message = "Email already exist";
                return finalResponse;
            }

            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.UserName ?? dto.Email
            };

            var result = await _users.CreateAsync(user, dto.Password);
            if(!result.Succeeded)
            {
                finalResponse.StatusCode = Enums.StatusCodesEnum.BadRequest;
                finalResponse.Message = result.ToString() ?? "";
                return finalResponse;

            }

            var roleResult = await _users.AddToRoleAsync(user, Roles.User);
            if (!roleResult.Succeeded)
                throw new InvalidOperationException("User created, but failed to assign default role.");

            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(15);
            var token = await _tokens.CreateToken(user);
            var loginResponse = new LoginResponseDto
            {
                ExpiresAt = expiresAt,
                Token = token
            };

            finalResponse.StatusCode = Enums.StatusCodesEnum.Success;
            finalResponse.Data = loginResponse;
            return finalResponse;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _users.FindByEmailAsync(dto.Email);

            if (user is null)
            {
                return new LoginResponseDto
                {
                    Message = "Email or password is incorrect."
                };

            }

            var success = await _users.CheckPasswordAsync(user, dto.Password, lockoutOnFailure: true);

            if (success.IsLockedOut)
            {
                return new LoginResponseDto
                {
                    Message = "This account is locked due to repeated failed sign-ins. Try again later."
                };
            }

            if (!success.Succeeded)
            {

                return new LoginResponseDto { Message = "Email or password is incorrect." };
            }

            var expiresAt = DateTimeOffset.UtcNow.AddHours(2);
            var token = await _tokens.CreateToken(user);

            return new LoginResponseDto
            {
                UserName = user.UserName,
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
