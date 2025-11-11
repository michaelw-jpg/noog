using Noog_mvc.Models.ProjectGroup;

namespace Noog_mvc.Models.Storage
{
    public class StorageViewModel
    {
        public TopSectionViewModel ProjectGroup { get; set; }
        public ICollection<StorageSummary> SummaryList { get; set; }
    }
}
