using Microsoft.AspNetCore.Identity;
using Noog_api.DTOs.Auth;

namespace Noog_api.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDto> RegisterAsync(RegisterDto dto);
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        Task LogoutAsync();
    }
}
