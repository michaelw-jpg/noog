using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Services.ProjectGroupServices
{
    public class ProjectGroupUserService(IProjectGroupUserRepo groupUserRepo) : IProjectGroupUserService
    {
        private readonly IProjectGroupUserRepo _groupUserRepo = groupUserRepo;

        public async Task<ProjectGroupUser> AddUserToProjectGroupAsync(Guid projectGroupId, string email)
        {
            var result = await _groupUserRepo.AddUserToProjectGroup(projectGroupId, email);
            return result;
        }
        public async Task<bool>IsUserMemberOfProjectGroupAsync(Guid projectGroupId, string email)
        {
            var result = await _groupUserRepo.IsUserMemberOfProjectGroup(projectGroupId, email);
            return result;
        }

        public async Task<bool> IsUserProjectGroupAdminAsync(Guid projectGroupId, Guid userId)
        {
            var result =  await _groupUserRepo.IsUserProjectGroupAdmin(projectGroupId, userId);
            return result;
        }

        public async Task<List<ProjectGroupUser>> GetProjectGroupUsersByCurrentUserAsync()
        {
            var result = await _groupUserRepo.GetProjectGroupUsersByCurrentUserAsync();
            return result;
        }
        public async Task<ProjectGroupUser> CreateProjectGroupUserAsync(ProjectGroupUser projectGroupUser)
        {
            var result = await _groupUserRepo.CreateProjectGroupUserAsync(projectGroupUser);
            return result;
        }

        public async Task<ProjectGroupUser> GetProjectGroupUserAsync(Guid ProjectGroupId, string email)
        {
            var result = await _groupUserRepo.GetProjectGroupUserAsync(ProjectGroupId, email);
            return result;
        }


    }
}
