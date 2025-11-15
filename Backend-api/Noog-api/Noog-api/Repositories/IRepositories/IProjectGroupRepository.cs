using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.ProjectGroup;
using Noog_api.Models;
using Noog_api.Models.Application;

namespace Noog_api.Repositories.IRepositories
{
    public interface IProjectGroupRepository
    {
        Task<List<ProjectGroup>> GetAllGroupProjectAsync();
        Task<ProjectGroup?> GetGroupProjectByIdAsync(Guid id);
        Task<ProjectGroup> CreateGroupProjectsAsync(ProjectGroup projectGroup);
        Task<ProjectGroup> PatchGroupProjectsAsync(ProjectGroup request);
        Task<bool> DeleteGroupProjectAsync(Guid id);
    }
}
