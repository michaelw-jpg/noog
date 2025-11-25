using Microsoft.EntityFrameworkCore;
using Noog_api.Data;
using Noog_api.DTOs.ProjectGroup;
using Noog_api.Models;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using System.Runtime.InteropServices;

namespace Noog_api.Repositories.GroupRepos
{
    public class ProjectGroupRepository(NoogDbContext context) : IProjectGroupRepository
    {
        private readonly NoogDbContext _context = context;

        public async Task<ProjectGroup> CreateGroupProjectsAsync(ProjectGroup projectGroup)
        {
            _context.ProjectGroups.Add(projectGroup);
            await _context.SaveChangesAsync();
            return projectGroup;
        }

        public async Task<bool> DeleteGroupProjectAsync(Guid id)
        {
            var groupProject = await _context.ProjectGroups.FindAsync(id);
            if (groupProject != null)
            {
                return false;
            }
            _context.ProjectGroups.Remove(groupProject);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProjectGroup>> GetAllGroupProjectAsync()
        {
            return await _context.ProjectGroups.ToListAsync();
        }

        public async Task<ProjectGroup?> GetGroupProjectByIdAsync(Guid id, Guid currentUserId)
        {
            var result = await _context.ProjectGroups
                .Include(pg => pg.GroupMeeting)
                .Include(pg => pg.ProjectGroupUsers)
                .FirstOrDefaultAsync(pg => pg.Id == id &&
                pg.ProjectGroupUsers.Any(pgu => pgu.ApplicationUserId == currentUserId));


            return result;
        }

        public async Task<ProjectGroup> PatchGroupProjectsAsync(ProjectGroup request)
        {

            await _context.SaveChangesAsync();
            return request;
        }

    }
}
