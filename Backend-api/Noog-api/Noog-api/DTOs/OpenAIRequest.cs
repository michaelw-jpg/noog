using System.ComponentModel.DataAnnotations;

namespace Noog_api.DTOs
{
    public class OpenAIRequest
    {
        [Required]
        public string Prompt { get; set; }
     }
}
