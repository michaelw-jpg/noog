using System.ComponentModel.DataAnnotations;

namespace Noog_api.DTOs.Summary
{
    public class CreateSummaryRequestDto
    {
        [MaxLength(100)]
        public required string Title { get; set; }

        public Guid ProjectGroupId { get; set; }
        public required string Content { get; set; }
    }
}
