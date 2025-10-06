using Noog_api.DTOs;

namespace Noog_api.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDto> RegisterAsync(RegisterDto dto);
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
    }
}
