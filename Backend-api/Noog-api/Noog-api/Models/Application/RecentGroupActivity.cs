using Noog_api.Models.Application.Enums;

namespace Noog_api.Models.Application
{
    public class RecentGroupActivity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public SourceType SourceType { get; set; }
        public Guid ProjectGroupId { get; set; }
        public ProjectGroup ProjectGroup { get; set; }

    }
}
