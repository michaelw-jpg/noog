using Noog_api.DTOs.GroupMeeting;

namespace Noog_api.DTOs.ProjectGroup
{
    public class ProjectGroupByIdResponse
    {
        public bool IsAdmin { get; set; }
        public ProjectGroupById ProjectGroup { get; set; }

        public GroupMeetingInfo GroupMeeting { get; set; }

    }
}
