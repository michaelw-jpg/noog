using Microsoft.EntityFrameworkCore;
using Noog_api.Data;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Repositories
{
    public class ProjectGroupUserRepo(NoogDbContext context, ICurrentUserService currentUserService) : IProjectGroupUserRepo
    {
        private readonly NoogDbContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<List<ProjectGroupUser>> GetProjectGroupUsersByCurrentUserAsync()
        {
            var result = await _context.ProjectGroupUsers
                .Include(pgu => pgu.ProjectGroup)
                .Where(pgu => pgu.ApplicationUserId == _currentUserService.UserId)
                .ToListAsync();
            return result;
        }

        public async Task<ProjectGroupUser>CreateProjectGroupUserAsync(ProjectGroupUser projectGroupUser)
        {
            _context.ProjectGroupUsers.Add(projectGroupUser);
            await _context.SaveChangesAsync();
            return projectGroupUser;
        }
    }
}
