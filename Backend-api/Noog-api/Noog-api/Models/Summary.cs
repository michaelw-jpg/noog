using Noog_api.Models.Application;

namespace Noog_api.Models
{
    public class Summary
    {
        public int SummaryId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        public Guid GroupStorageId { get; set; }
        public GroupStorage GroupStorage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
