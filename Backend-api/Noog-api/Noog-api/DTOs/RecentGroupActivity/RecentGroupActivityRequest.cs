using Noog_api.Models.Application.Enums;

namespace Noog_api.DTOs.RecentGroupActivity
{
    public class RecentGroupActivityRequest
    {
        public string Title { get; set; }
        public SourceType SourceType { get; set; }
        public Guid ProjectGroupId { get; set; }
    }
}
