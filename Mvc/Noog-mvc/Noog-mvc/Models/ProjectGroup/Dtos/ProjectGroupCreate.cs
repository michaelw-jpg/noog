using System.ComponentModel.DataAnnotations;

namespace Noog_mvc.Models.ProjectGroup.Dtos
{
    public class ProjectGroupCreate
    {
        [Required]
        [MaxLength(50, ErrorMessage = "The Name of the ProjectGroup can't be more than 50 characters.")]
        public string Name { get; set; }
        [Required]
        [Url(ErrorMessage = "The Image Url has to be a valid Url")]
        [MaxLength(250, ErrorMessage = "The Image Url can't be more than 250 characters.")]
        public string ImageUrl { get; set; }
    }
}
