using Noog_api.Models.Application;

namespace Noog_api.Services.IServices
{
    public interface IGroupMeetingService
    {
        Task<GroupMeeting> CreateGroupMeetingAsync(GroupMeeting groupMeeting);
    }
}
