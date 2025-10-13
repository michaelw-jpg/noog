using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noog_api.Services.IServices;

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController(IOpenAiService openAiService) : ControllerBase
    {
        private readonly IOpenAiService _openAiService = openAiService;
        [HttpGet("summary")]
        public async Task<IActionResult> GetChatResponse([FromQuery] string prompt)
        {
            var response = await _openAiService.GetChatResponseAsync(prompt);
            return Ok(response);
        }

    }
}
