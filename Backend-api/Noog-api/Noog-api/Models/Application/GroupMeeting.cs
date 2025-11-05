using Noog_api.Models.AssemblyAi;

namespace Noog_api.Models.Application
{
    public class GroupMeeting : BaseEntity
    {
        public Guid Id { get; set; }
        public string CallId { get; set; }
        public ICollection<Transcript> Transcripts { get; set; } = new List<Transcript>();

        public Guid ProjectGroupId { get; set; }
        public ProjectGroup ProjectGroup { get; set; }
    }
}
