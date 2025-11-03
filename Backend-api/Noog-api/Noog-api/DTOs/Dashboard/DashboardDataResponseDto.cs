namespace Noog_api.DTOs.Dashboard
{
    public class DashboardDataResponseDto
    {
        public DashboardDataUserInfo User { get; set; }
        public List<DashboardDataRecentActivity> RecentActivities { get; set; }
    }
}
