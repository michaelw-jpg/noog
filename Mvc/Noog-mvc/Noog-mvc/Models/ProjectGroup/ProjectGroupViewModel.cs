namespace Noog_mvc.Models.ProjectGroup
{
    public class ProjectGroupViewModel
    {
        public bool IsAdmin { get; set; }
        public ProjectGroupByIdViewModel ProjectGroup { get; set; }
        public TopSectionViewModel TopSection { get; set; }
        public MeetingRoomViewModel MeetingRoom { get; set; }
        public ChatViewModel ChatRoom { get; set; }
        public StorageViewModel Storage { get; set; }
    }
}
