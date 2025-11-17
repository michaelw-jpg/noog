using Noog_api.Models.Application;

namespace Noog_api.Services.IServices
{
    public interface IProjectGroupUserService 
    {
        Task<ProjectGroupUser> AddUserToProjectGroupAsync(Guid projectGroupId, string email);
        Task<bool> IsUserMemberOfProjectGroupAsync(Guid projectGroupId, string email);
        Task<bool> IsUserProjectGroupAdminAsync(Guid projectGroupId, Guid userId);
        Task<List<ProjectGroupUser>> GetProjectGroupUsersByCurrentUserAsync();
        Task<ProjectGroupUser> CreateProjectGroupUserAsync(ProjectGroupUser projectGroupUser);

        Task<ProjectGroupUser> GetProjectGroupUserAsync(Guid ProjectGroupId, string email);
    }
}
