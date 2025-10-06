using System.Text.Json.Serialization;
namespace Noog_api.Models.AssemblyAi
{
    public class UploadResponse
    {
        [JsonPropertyName("upload_url")]
        public string UploadUrl { get; set; }
        [JsonPropertyName("language_code")]
        public string LanguageCode { get; set; }
    }
}
