namespace Noog_api.Models.Application
{
    public class ProjectGroup : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<ProjectGroupUser> ProjectGroupUsers { get; set; } = new List<ProjectGroupUser>();
        public ICollection<RecentGroupActivity> RecentGroupActivities { get; set; } = new List<RecentGroupActivity>();
        public Guid MeetingId { get; set; }
        public GroupMeeting GroupMeeting { get; set; }
        public Guid ChatId { get; set; }
        public GroupChat GroupChat { get; set; }
        public Guid StorageId { get; set; }
        public GroupStorage GroupStorage { get; set; }

    }
}
