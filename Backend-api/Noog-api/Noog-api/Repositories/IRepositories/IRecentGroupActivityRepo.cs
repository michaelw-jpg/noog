using Noog_api.Models.Application;

namespace Noog_api.Repositories.IRepositories
{
    public interface IRecentGroupActivityRepo
    {
        Task<List<RecentGroupActivity>> GetLatestRecentGroupActivitiesByProjectsAsync(List<Guid> ProjectIds);
    }
}
