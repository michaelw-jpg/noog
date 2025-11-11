using Noog_api.Models;
using Noog_api.Models.Application;

namespace Noog_api.Repositories.IRepositories
{
    public interface IProjectGroupRepository
    {
        Task<List<ProjectGroup>> GetAllGroupProjectAsync();
        Task<ProjectGroup?> GetGroupProjectByIdAsync(int id);
        Task<ProjectGroup> CreateGroupProjectsAsync(ProjectGroup projectGroup);
        Task<ProjectGroup?> UpdateGroupProjectsAsync(int id, ProjectGroup updatedGroupProject);
        Task<bool> DeleteGroupProjectAsync(int id);
    }
}
