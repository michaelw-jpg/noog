using Noog_api.DTOs;
using Noog_api.Models;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService<User> _users;
        private readonly TokenService _tokens;

        public AuthService(IUserService<User> users, TokenService tokens)
        {
            _users = users;
            _tokens = tokens;
        }

        public async Task<LoginResponseDto> RegisterAsync(RegisterDto dto)
        {
            var exist = await _users.FindByEmailAsync(dto.Email);
            if (exist != null)
                throw new InvalidOperationException("Email already exists");

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName ?? dto.Email
            };
            if (user is null)
                throw new NullReferenceException("User can not be null");

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
            if (!success)
                throw new ArgumentException("Email or Password was incorrect");

            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(15);

            var token = await _tokens.CreateToken(user);
            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt
            };
        }
    }
}
