using Noog_api.DTOs.Dashboard;

namespace Noog_api.Services.Dashboard
{
    public class DashboardDependenciesMocker
    {
        public DashboardDataResponseDto GetMockedDashboardData()
        {
            var response = new DashboardDataResponseDto();

            response.User = new DashboardDataUserInfo
            {
                UserName = "OliverApi"
            };

            response.RecentActivities = new List<DashboardDataRecentActivity>
            {
                new DashboardDataRecentActivity
                {
                    ProjectGroupName = "Project Alpha Api",
                    ProjectGroupImageUrl = "",
                    ActivityTitle = "Meeting from yesterday at 12:45",
                    Source = "Meeting"
                },
                new DashboardDataRecentActivity
                {
                    ProjectGroupName = "Project Beta Api",
                    ProjectGroupImageUrl = "",
                    ActivityTitle = "Summary done",
                    Source = "Summary"
                }
            };

            return response;
        }

        //public List<ProjectGroupResponseDto> GetMockedGroupProjects()
        //{
        //    var response = new List<ProjectGroupResponseDto>
        //    {
        //        new ProjectGroupResponseDto { Id = 1, ImageUrl = "", Title = "Project Noog (Api)"},
        //        new ProjectGroupResponseDto { Id = 2, ImageUrl = "", Title = "Project Omega (Api)"}
        //    };

        //    return response;
        //}
    }
}
