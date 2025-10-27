using System.ComponentModel.DataAnnotations;

namespace Noog_mvc.Models
{
    public class LoginViewModel
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
