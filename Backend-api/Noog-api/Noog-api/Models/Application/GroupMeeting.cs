using Noog_api.Models.AssemblyAi;

namespace Noog_api.Models.Application
{
    public class GroupMeeting
    {
        public Guid Id { get; set; }
        public string CallId { get; set; }
        public ICollection<Transcript> Transcripts { get; set; }
    }
}
