namespace Noog_mvc.Models.Dashboard
{
    public class RecentActivityDto
    {
        public string ProjectGroupName { get; set; }
        public string ProjectGroupImageUrl { get; set; }
        public string ActivityTitle { get; set; }
        public string Source { get; set; } // Chat, Storage(Summary Done), Meeting/Call
    }
}
