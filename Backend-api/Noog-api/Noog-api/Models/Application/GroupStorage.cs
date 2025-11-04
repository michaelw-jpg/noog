namespace Noog_api.Models.Application
{
    public class GroupStorage
    {
        public Guid Id { get; set; }
        public ICollection<Summary> Summaries { get; set; }
    }
}
