using System.Security.Claims;

namespace Noog_mvc.Models.Login
{

    public class LoginResult
    {
        public ClaimsIdentity? Identity { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public LoginStatus Status { get; set; }
        public string? ErrorMessage { get; set; } // optional for display
    }

    public enum LoginStatus
    {
        Success,
        InvalidCredentials,
        ServerError
    }
}
