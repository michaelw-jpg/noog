using Noog_api.DTOs.StreamIODtos;
using Noog_api.Enums;
using Noog_api.Models;


namespace Noog_api.Services.Outbound_Services.IOutbound_Services
{
    public interface IStreamIOService
    {
        Task<StreamIODTO> CreateStreamIOUser(ApplicationUser user);

        Task<bool> CreateStreamIOCallId(Guid id);
    }
}
