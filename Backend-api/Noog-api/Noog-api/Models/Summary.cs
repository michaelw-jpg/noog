using Noog_api.Models.Application;

namespace Noog_api.Models
{
    public class Summary : BaseEntity
    {
        public int SummaryId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Guid GroupStorageId { get; set; } // TODO - Change to not null once everything goes well
        public GroupStorage GroupStorage { get; set; }
    }
}
