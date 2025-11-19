using System.ComponentModel.DataAnnotations;

namespace Noog_api.DTOs.Summary
{
    public class CreateSummaryDto
    {
        [MaxLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }

        public Guid? GroupStorageId { get; set; }

    }
}
