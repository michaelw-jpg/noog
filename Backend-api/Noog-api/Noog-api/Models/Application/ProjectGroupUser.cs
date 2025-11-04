namespace Noog_api.Models.Application
{
    public class ProjectGroupUser : BaseEntity
    {
        public Guid ProjectGroupId { get; set; }
        public ProjectGroup ProjectGroup { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public bool IsAdmin { get; set; } // Might use Application Role "User" and "Admin" instead
    }
}
