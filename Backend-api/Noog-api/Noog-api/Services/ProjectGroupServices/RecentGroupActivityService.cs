using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.RecentGroupActivity;
using Noog_api.Enums;
using Noog_api.Mappers;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Services.ProjectGroupServices
{
    public class RecentGroupActivityService(IProjectGroupUserService projectGroupUser, IRecentGroupActivityRepo recentGroupRepo) : IRecentGroupActivityService
    {
        private readonly IProjectGroupUserService _projectGroupUser = projectGroupUser;
        private readonly IRecentGroupActivityRepo _recentGroupRepo = recentGroupRepo;


        public async Task<List<RecentGroupActivity>> GetTopThreeLatestActivitesAsync()
        {

            var projects = await _projectGroupUser.GetProjectGroupUsersByCurrentUserAsync();
            var projectIds = projects.Select(p => p.ProjectGroupId).Distinct().ToList();
            var recentActivities = await _recentGroupRepo.GetLatestRecentGroupActivitiesByProjectsAsync(projectIds);

            return recentActivities;

        }

        public async Task<bool> AddNewActivityAsync(CreateRecentSummaryRequest request)
        {
           
            var newActivity = new RecentGroupActivity();
            GenericMapper.ApplyCreate(newActivity, request);
            try
            {
                await _recentGroupRepo.AddRecentGroupActivityAsync(newActivity);
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
    }
}
