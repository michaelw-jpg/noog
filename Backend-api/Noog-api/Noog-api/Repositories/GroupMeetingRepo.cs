using Noog_api.Data;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;

namespace Noog_api.Repositories
{
    public class GroupMeetingRepo(NoogDbContext dbContext) : IGroupMeetingRepo
    {
        private readonly NoogDbContext _dbContext = dbContext;

        public async Task<GroupMeeting>CreateGroupMeetingAsync(GroupMeeting groupMeeting)
        {
            _dbContext.GroupMeetings.Add(groupMeeting);
            await _dbContext.SaveChangesAsync();
            return groupMeeting;
        }
    }
}
