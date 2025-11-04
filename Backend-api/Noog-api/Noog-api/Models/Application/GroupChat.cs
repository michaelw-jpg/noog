namespace Noog_api.Models.Application
{
    public class GroupChat : BaseEntity
    {
        public Guid Id { get; set; }

        // To be continued...

        public Guid ProjectGroupId { get; set; }
        public ProjectGroup ProjectGroup { get; set; }
    }
}
