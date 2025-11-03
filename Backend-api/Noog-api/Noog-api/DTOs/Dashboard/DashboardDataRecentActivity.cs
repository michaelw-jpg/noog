namespace Noog_api.DTOs.Dashboard
{
    public class DashboardDataRecentActivity
    {
        public string ProjectGroupName { get; set; }
        public string ProjectGroupImageUrl { get; set; }
        public string ActivityTitle { get; set; }
        public string Source { get; set; } // Chat, Storage(Summary Done), Meeting/Call
    }
}
