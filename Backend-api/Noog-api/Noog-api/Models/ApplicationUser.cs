using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static System.Net.WebRequestMethods;

namespace Noog_api.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        private string _ImgProfile;

        [Url]
        [StringLength(300)]
        public string? ImgProfile { get => string.IsNullOrEmpty(_ImgProfile) ? "https://t3.ftcdn.net/jpg/06/33/54/78/240_F_633547842_AugYzexTpMJ9z1YcpTKUBoqBF0CUCk10.jpg" : _ImgProfile; set => _ImgProfile = value; }  
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
