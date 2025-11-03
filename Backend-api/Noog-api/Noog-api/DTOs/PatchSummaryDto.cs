using System.ComponentModel.DataAnnotations;

namespace Noog_api.DTOs
{
    public class PatchSummaryDto
    {
        [MaxLength(100)]
        public string? Title { get; set; }

        public string? Content { get; set; }
    }
}
