using Noog_api.Models.Application;

namespace Noog_api.Services.IServices
{
    public interface IRecentGroupActivityService
    {
        Task<List<RecentGroupActivity>> GetTopThreeLatestActivitesAsync();
    }
}
