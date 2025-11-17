using Noog_api.Models.Application;

namespace Noog_api.Repositories.IRepositories
{
    public interface IProjectGroupUserRepo
    {
        Task<ProjectGroupUser> AddUserToProjectGroup(Guid projectGroupId, string email);
        Task<bool> IsUserMemberOfProjectGroup(Guid projectGroupId, string email);
        Task<bool> IsUserProjectGroupAdmin(Guid projectGroupId, Guid userId);
        Task<List<ProjectGroupUser>> GetProjectGroupUsersByCurrentUserAsync();
        Task<ProjectGroupUser> CreateProjectGroupUserAsync(ProjectGroupUser projectGroupUser);

        Task<ProjectGroupUser> GetProjectGroupUserAsync(Guid ProjectGroupId, string email);
    }
}
