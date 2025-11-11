using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class ProjectGroupUserService(IProjectGroupUserRepo groupUserRepo) : IProjectGroupUserService
    {
        private readonly IProjectGroupUserRepo _groupUserRepo = groupUserRepo;
        public async Task<List<ProjectGroupUser>> GetProjectGroupUsersByCurrentUserAsync()
        {
            var result = await _groupUserRepo.GetProjectGroupUsersByCurrentUserAsync();
            return result;
        }
    }
}
