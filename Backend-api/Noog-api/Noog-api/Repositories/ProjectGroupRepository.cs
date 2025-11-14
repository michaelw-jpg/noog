using Microsoft.EntityFrameworkCore;
using Noog_api.Data;
using Noog_api.Models;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using System.Runtime.InteropServices;

namespace Noog_api.Repositories
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

        public async Task<bool> DeleteGroupProjectAsync(int id)
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

        public async Task<ProjectGroup?> GetGroupProjectByIdAsync(Guid id)
        {
            var result = await _context.ProjectGroups
                .AsNoTracking()
                .Include(pg => pg.GroupMeeting)
                .FirstOrDefaultAsync(pg => pg.Id == id);


            return result;
        }

        public async Task<ProjectGroup?> UpdateGroupProjectsAsync(Guid id, ProjectGroup updatedGroupProject)
        {
            var existProjectGroup = await _context.ProjectGroups.FindAsync(id);
            if (existProjectGroup != null)
            {
                return null;
            }
            existProjectGroup.Name = updatedGroupProject.Name;
            existProjectGroup.ImageUrl = updatedGroupProject.ImageUrl;
            //TODO:Add group ProjectUsers

            await _context.SaveChangesAsync();
            return existProjectGroup;
        }

    }
}
