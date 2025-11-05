namespace Noog_api.Models.Application
{
    public class GroupStorage : BaseEntity
    {
        public Guid Id { get; set; }
        public ICollection<Summary> Summaries { get; set; } = new List<Summary>();

        public Guid ProjectGroupId { get; set; }
        public ProjectGroup ProjectGroup { get; set; }
    }
}
