namespace Noog_api.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string UserName { get; set; }
        public string Token { get; set; } = default!;
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
