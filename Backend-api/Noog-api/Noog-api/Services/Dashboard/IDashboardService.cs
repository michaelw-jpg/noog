using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.Dashboard;

namespace Noog_api.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<BaseResponseDto<DashboardDataResponseDto>> GetDashboardDataAsync();
        Task<BaseResponseDto<List<ProjectGroupResponseDto>>> GetDashboardUserProjectGroupsAsync();
    }
}
