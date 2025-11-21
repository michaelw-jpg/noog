using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Helpers;
using Noog_api.Models.AssemblyAi;
using Noog_api.Services.IServices;
using static Noog_api.Services.OpenAiPromptService;

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController(IOpenAiService openAiService) : ControllerBase
    {
        private readonly IOpenAiService _openAiService = openAiService;
        [HttpPost("summary")]
        public async Task<ActionResult<OpenAIResponseDto>> GetChatResponse([FromBody] OpenAIRequest prompt)
        {
            var response = await _openAiService.GetChatResponseAsync(prompt.Prompt);
            return ApiResponseHelper.ToActionResult(response);
        }

        [HttpPost("summary/prompt")]
        public async Task<ActionResult<OpenAIResponseDto>> GetChatResponse([FromBody] PromptType type)
        {
            var resposne = await _openAiService.GetChatResponseAsync(type.ToString());
            return ApiResponseHelper.ToActionResult(resposne);

        }

    }
}
