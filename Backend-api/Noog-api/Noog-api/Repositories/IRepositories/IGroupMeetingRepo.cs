using Noog_api.Models.Application;

namespace Noog_api.Repositories.IRepositories
{
    public interface IGroupMeetingRepo
    {
        Task<GroupMeeting> CreateGroupMeetingAsync(GroupMeeting groupMeeting);
    }
}
