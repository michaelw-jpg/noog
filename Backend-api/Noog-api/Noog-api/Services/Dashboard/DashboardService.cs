using Azure;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.Dashboard;
using Noog_api.Mappers;

namespace Noog_api.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly DashboardDependenciesMocker _mocker = new DashboardDependenciesMocker();
        // private readonly IUserService _userService;
        // private readonly IRecentProjectActivityService _activityService;
        // private readonly IProjectGroupService _pgService;

        // ctor *Tab*
        public async Task<BaseResponseDto<DashboardDataResponseDto>> GetDashboardDataAsync()
        {
            // var userId = User.GetUserId();
            // var recentActivities = await _activityService.GetRecentActivitiesAsync(userId: userId);
            // var userInfo = await _userService.GetUserInfoAsync(userId: userId);

            // TODO - Discuss tables like users and groupprojects
            // TODO - This responseDto isn't flat and won't probably reflect a model. 



            var result = _mocker.GetMockedDashboardData();

            var response = new BaseResponseDto<DashboardDataResponseDto>();

            if (result == null)
            {
                response.StatusCode = Enums.StatusCodesEnum.NotFound;
                response.Message = "Dashboard Data not found";

                return await Task.FromResult(response);
            }

            // var dtoList = GenericMapper.ToDto<DashboardData, DashboardDataResponseDto>(result);
            response.Data = result;
            response.StatusCode = Enums.StatusCodesEnum.Success;
            response.Message = "Dashboard Data loaded successfully";


            return await Task.FromResult(response);
        }

        public async Task<BaseResponseDto<List<ProjectGroupResponseDto>>> GetDashboardUserProjectGroupsAsync()
        {
            // var userId = User.GetUserId();
            // var result = await _pgService.GetAllProjectGroupsByUserAsync(userId: userId);

            var result = _mocker.GetMockedGroupProjects();

            var response = new BaseResponseDto<List<ProjectGroupResponseDto>>();

            if (result == null || !result.Any())
            {
                response.Data = new List<ProjectGroupResponseDto>();
                response.StatusCode = Enums.StatusCodesEnum.Success;
                response.Message = "No ProjectGroups found";
            }
            else
            {
                // var dtoList = GenericMapper.ToDtoList<ProjectGroup, ProjectGroupResponseDto>(result);
                var dtoList = result;

                response.Data = dtoList;
                response.StatusCode = Enums.StatusCodesEnum.Success;
                response.Message = "ProjectGroups loaded successfully";
            }

            return await Task.FromResult(response);
        }
    }
}
