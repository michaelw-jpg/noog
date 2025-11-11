using Microsoft.EntityFrameworkCore;
using Noog_api.Data;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;

namespace Noog_api.Repositories
{
    public class RecentGroupActivityRepo(NoogDbContext context) : IRecentGroupActivityRepo
    {
        private readonly NoogDbContext _context = context;

        public async Task<List<RecentGroupActivity>> GetLatestRecentGroupActivitiesByProjectsAsync(List<Guid>ProjectIds)
        {
            
          var result = await _context.RecentGroupActivities
                .Include(rga => rga.ProjectGroup)
                .Where(rga => ProjectIds.Contains(rga.ProjectGroupId))
                .OrderByDescending(rga => rga.CreatedAt)
                .Take(3)
                .ToListAsync();
            return result;
        }
    }
}
