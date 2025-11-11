using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Noog_api.DTOs.Dashboard
{
    public class ProjectGroupResponseDto
    {
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}
