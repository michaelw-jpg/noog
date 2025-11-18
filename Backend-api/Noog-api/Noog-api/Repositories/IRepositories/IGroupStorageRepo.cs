using Noog_api.Models.Application;

namespace Noog_api.Repositories.IRepositories
{
    public interface IGroupStorageRepo
    {
        Task<GroupStorage> CreateGroupStorageAsync(GroupStorage groupStorage);

        Task<GroupStorage?> GetGroupStorageByIdAsync(Guid id);
    }
}
