using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Noog_mvc.Models.Register
{
    public class RegisterUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Password must match")]
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
