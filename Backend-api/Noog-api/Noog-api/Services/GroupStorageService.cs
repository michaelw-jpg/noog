using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class GroupStorageService(IGroupStorageRepo groupStorageRepo) : IGroupStorageService
    {
        private readonly IGroupStorageRepo _groupStorageRepo = groupStorageRepo;

        public async Task<GroupStorage> CreateGroupStorageAsync(GroupStorage groupStorage)
        {
            var result = await _groupStorageRepo.CreateGroupStorageAsync(groupStorage);
            return result;
        }
    }
}
