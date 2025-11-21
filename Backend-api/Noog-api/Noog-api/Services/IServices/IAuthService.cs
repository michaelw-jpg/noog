using Microsoft.AspNetCore.Identity;
using Noog_api.DTOs.Auth;
using Noog_api.DTOs.BaseResponseDtos;

namespace Noog_api.Services.IServices
{
    public interface IAuthService
    {
        Task <BaseResponseDto<LoginResponseDto>> RegisterAsync(RegisterDto dto);
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        Task LogoutAsync();
    }
}
