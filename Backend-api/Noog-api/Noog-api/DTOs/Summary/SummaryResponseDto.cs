using System.Text.Json.Serialization;

namespace Noog_api.DTOs.Summary
{
    public class SummaryResponseDto
    {
        [JsonPropertyName("Id")]
        public int SummaryId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
