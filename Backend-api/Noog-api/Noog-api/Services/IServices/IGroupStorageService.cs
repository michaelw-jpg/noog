using Noog_api.Models.Application;

namespace Noog_api.Services.IServices
{
    public interface IGroupStorageService
    {
        Task<GroupStorage> CreateGroupStorageAsync(GroupStorage groupStorage);

        Task<GroupStorage?> GetGroupStorageById(Guid id);
    }
}
