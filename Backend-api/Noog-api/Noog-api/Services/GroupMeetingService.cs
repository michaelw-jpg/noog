using Noog_api.Models.Application;
using Noog_api.Repositories;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class GroupMeetingService(IGroupMeetingRepo groupMeetingRepo) : IGroupMeetingService
    {
        private readonly IGroupMeetingRepo _groupMeetingRepo = groupMeetingRepo;

        public async Task<GroupMeeting> CreateGroupMeetingAsync(GroupMeeting groupMeeting)
        {
            var result = await _groupMeetingRepo.CreateGroupMeetingAsync(groupMeeting);
            return result;
        }
    }
}
