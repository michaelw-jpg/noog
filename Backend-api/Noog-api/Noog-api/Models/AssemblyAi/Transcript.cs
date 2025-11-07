using Noog_api.Models.Application;

namespace Noog_api.Models.AssemblyAi
{
    public class Transcript : BaseEntity
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }
        public string Error { get; set; }
        public Guid? GroupMeetingId { get; set; } // Change to not null once everything goes well
        public GroupMeeting GroupMeeting { get; set; }
    }
}
