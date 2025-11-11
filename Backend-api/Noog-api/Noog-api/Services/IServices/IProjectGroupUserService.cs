using Noog_api.Models.Application;

namespace Noog_api.Services.IServices
{
    public interface IProjectGroupUserService 
    {
        Task<List<ProjectGroupUser>> GetProjectGroupUsersByCurrentUserAsync();
        Task<ProjectGroupUser> CreateProjectGroupUserAsync(ProjectGroupUser projectGroupUser);
    }
}
