using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Helpers;
using Noog_api.Services.IServices;

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController(IOpenAiService openAiService) : ControllerBase
    {
        private readonly IOpenAiService _openAiService = openAiService;
        [HttpPost("summary")]
        public async Task<ActionResult<OpenAIResponseDto>> GetChatResponse([FromBody] string prompt)
        {
            var response = await _openAiService.GetChatResponseAsync(prompt);
            return ApiResponseHelper.ToActionResult(response);
        }

    }
}
