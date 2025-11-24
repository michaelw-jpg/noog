using Microsoft.EntityFrameworkCore;
using Noog_api.Data;
using Noog_api.Models;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services;
using Noog_api.Services.IServices;

namespace Noog_api.Repositories.GroupRepos
{
    public class ProjectGroupUserRepo(NoogDbContext context, ICurrentUserService currentUserService,
        IUserService<ApplicationUser> userService) : IProjectGroupUserRepo
    {
        private readonly IUserService<ApplicationUser> _userService = userService;
        private readonly NoogDbContext _context = context;
        private readonly ICurrentUserService _currentUserService = currentUserService;


        public async Task<ProjectGroupUser> AddUserToProjectGroup(Guid projectGroupId, string email)
        { 
            var user = await _userService.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var projectGroupUser = new ProjectGroupUser
            {
                ProjectGroupId = projectGroupId,
                ApplicationUserId = user.Id,
                IsAdmin = false
            };
            _context.ProjectGroupUsers.Add(projectGroupUser);
            await _context.SaveChangesAsync();
            return projectGroupUser;
        }
        public async Task<bool> IsUserMemberOfProjectGroup(Guid projectGroupId, string email)
        {
            var projectGroupUser = await _context.ProjectGroupUsers
                .Include(pgu=> pgu.ApplicationUser)
                .FirstOrDefaultAsync(pgu => pgu.ProjectGroupId == projectGroupId && pgu.ApplicationUser.Email == email);

            var result = projectGroupUser != null;
            return result;
        }
        public async Task<bool> IsUserProjectGroupAdmin(Guid projectGroupId, Guid userId)
        {
            var projectGroupUser = await _context.ProjectGroupUsers
                .FirstOrDefaultAsync(pgu => pgu.ProjectGroupId == projectGroupId && pgu.ApplicationUserId == userId);

            var result = projectGroupUser != null && projectGroupUser.IsAdmin;
            return result;
        }
        public async Task<List<ProjectGroupUser>> GetProjectGroupUsersByCurrentUserAsync()
        {
            var currentUserId = _currentUserService.UserId;

            var result = await _context.ProjectGroupUsers
                .Include(pgu => pgu.ProjectGroup)
                .Where(pgu => pgu.ApplicationUserId == currentUserId)
                .ToListAsync();
            return result;
        }

        public async Task<ProjectGroupUser>CreateProjectGroupUserAsync(ProjectGroupUser projectGroupUser)
        {
            _context.ProjectGroupUsers.Add(projectGroupUser);
            await _context.SaveChangesAsync();
            return projectGroupUser;
        }

        public async Task<ProjectGroupUser> GetProjectGroupUserAsync(Guid ProjectGroupId, string email)
        {
            var result = await _context.ProjectGroupUsers
                .Include(pgu => pgu.ApplicationUser)
                .FirstOrDefaultAsync(pgu => pgu.ProjectGroupId == ProjectGroupId && pgu.ApplicationUser.Email == email);
            return result;
        }
    }
}
