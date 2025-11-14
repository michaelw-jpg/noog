using System.Text.Json.Serialization;

namespace Noog_api.DTOs.ProjectGroup
{
    public class ProjectGroupById
    {

        public Guid Id { get; set; }

        [JsonPropertyName("GroupName")]
        public string Name { get; set; }

        [JsonPropertyName("GroupImg")]
        public string ImageUrl { get; set; }


    }
}
