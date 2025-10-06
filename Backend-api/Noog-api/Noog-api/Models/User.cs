using Microsoft.AspNetCore.Identity;

namespace Noog_api.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
