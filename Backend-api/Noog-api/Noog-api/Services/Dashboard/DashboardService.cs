using Azure;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.Dashboard;
using Noog_api.Mappers;
using Noog_api.Models;
using Noog_api.Models.Application;
using Noog_api.Services.IServices;

namespace Noog_api.Services.Dashboard
{
    public class DashboardService(IRecentGroupActivityService groupActivityService, IUserService<ApplicationUser> userService,
        ICurrentUserService currentUserService, IProjectGroupUserService projectGroupUserService ) : IDashboardService
    {
        private readonly IRecentGroupActivityService _groupActivityService = groupActivityService;
        private readonly IUserService<ApplicationUser> _userService = userService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IProjectGroupUserService _pgUserService = projectGroupUserService;



        public async Task<BaseResponseDto<DashboardDataResponseDto>> GetDashboardDataAsync()
        {
            var recentActivities = await _groupActivityService.GetTopThreeLatestActivitesAsync();
            var userInfo = await _userService.FindByIdAsync(_currentUserService.UserId);

            var response = new BaseResponseDto<DashboardDataResponseDto>();

            if (userInfo == null)
            {
                response.StatusCode = Enums.StatusCodesEnum.NotFound;
                response.Message = "Error getting user";

                return response;
            }

            var dtoRecentActivities = new List<DashboardDataRecentActivity>();

            foreach (var activity in recentActivities)
            {
                var dtoActivity = new DashboardDataRecentActivity
                {
                    ActivityTitle = activity.Title,
                    ProjectGroupName = activity.ProjectGroup?.Name,
                    ProjectGroupImageUrl = activity.ProjectGroup?.ImageUrl,
                    Source = activity.SourceType.ToString()
                };
                dtoRecentActivities.Add(dtoActivity);
            }

            var userDto = new DashboardDataUserInfo
            {
                UserName = userInfo.UserName,
                ProfileImageUrl = userInfo.ImgProfile
            };

            var dashboardData = new DashboardDataResponseDto
            {
                RecentActivities = dtoRecentActivities,
                User = userDto
            };

            response.Data = dashboardData;
            response.StatusCode = Enums.StatusCodesEnum.Success;
            response.Message = "Dashboard Data loaded successfully";
            return response;

        }

        public async Task<BaseResponseDto<List<ProjectGroupResponseDto>>> GetDashboardUserProjectGroupsAsync()
        {
            var result = await _pgUserService.GetProjectGroupUsersByCurrentUserAsync();


            var dtoList = GenericMapper.ToDtoList<ProjectGroupUser, ProjectGroupResponseDto>(result);
            var response = new BaseResponseDto<List<ProjectGroupResponseDto>>(
                Enums.StatusCodesEnum.Success, "ProjectGroups loaded successfully", dtoList);


            return response;
        }
    }
}
