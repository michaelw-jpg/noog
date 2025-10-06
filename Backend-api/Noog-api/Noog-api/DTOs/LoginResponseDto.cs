namespace Noog_api.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = default!;
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
