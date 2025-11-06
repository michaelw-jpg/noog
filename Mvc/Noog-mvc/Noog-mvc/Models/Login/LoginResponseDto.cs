namespace Noog_mvc.Models.Login
{
    public class LoginResponseDto
    {
        public string UserName { get; set; }
        public required string Token { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }

    }
}
