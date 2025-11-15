using Noog_api.Models.Application;

namespace Noog_api.Repositories.IRepositories
{
    public interface IProjectGroupUserRepo
    {
        Task<List<ProjectGroupUser>> GetProjectGroupUsersByCurrentUserAsync();
        Task<ProjectGroupUser> CreateProjectGroupUserAsync(ProjectGroupUser projectGroupUser);

        Task<ProjectGroupUser> GetProjectGroupUserAsync(Guid ProjectGroupId, string email);
    }
}
