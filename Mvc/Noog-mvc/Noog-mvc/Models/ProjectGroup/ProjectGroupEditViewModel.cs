using System.ComponentModel.DataAnnotations;

namespace Noog_mvc.Models.ProjectGroup
{
    public class ProjectGroupEditViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Group name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        [Display(Name = "Group Image URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string ImageUrl { get; set; }
    }
}
