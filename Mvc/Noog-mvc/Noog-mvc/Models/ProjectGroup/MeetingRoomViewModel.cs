namespace Noog_mvc.Models.ProjectGroup
{
    public class MeetingRoomViewModel
    {
        public bool IsCallOngoing { get; set; } // Todo - If time exists
        public string Title { get; init; } = "Meeting room";

        //Maybe add MeetingRoom id and Url, also timestamp?
    }
}
