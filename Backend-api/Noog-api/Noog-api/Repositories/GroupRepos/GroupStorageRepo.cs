using Microsoft.EntityFrameworkCore;
using Noog_api.Data;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;

namespace Noog_api.Repositories.GroupRepos
{
    public class GroupStorageRepo(NoogDbContext dbContext) : IGroupStorageRepo
    {
        private readonly NoogDbContext _dbContext = dbContext;
        public async Task<GroupStorage>CreateGroupStorageAsync(GroupStorage groupStorage)
        {
            _dbContext.GroupStorages.Add(groupStorage);
            await _dbContext.SaveChangesAsync();
            return groupStorage;
        }

        public async Task<GroupStorage?> GetGroupStorageByIdAsync(Guid id)
        {
            var groupStorage = await _dbContext.GroupStorages
                .Where(gs => gs.ProjectGroupId == id)
                .FirstOrDefaultAsync();

            return groupStorage;
        }
    }
}
