using Noog_api.DTOs.RecentGroupActivity;
using Noog_api.Models.Application;

namespace Noog_api.Services.IServices
{
    public interface IRecentGroupActivityService
    {
        Task<List<RecentGroupActivity>> GetTopThreeLatestActivitesAsync();

        Task<bool> AddNewActivityAsync(CreateRecentSummaryRequest request);
    }
}
