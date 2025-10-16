using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs;
using Noog_api.Helpers;
using Noog_api.Services.IServices;

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrchestrateController(IAssemblyAiService assemblyService, IOpenAiService openAiService) : ControllerBase
    {
        private readonly IAssemblyAiService _assemblyService = assemblyService;
        private readonly IOpenAiService _openAiService = openAiService;

        [HttpGet("orchestrate")]
        public async Task<ActionResult<OpenAIResponseDto>> Orchestrate(string audioUrl, string? language = null)
        {
            if (string.IsNullOrWhiteSpace(audioUrl))
                return BadRequest("Audio URL cannot be empty.");

            // Step 1: Upload the audio file to AssemblyAI
            string uploadedUrl = await _assemblyService.UploadFileAsync(audioUrl);
            // Step 2: Create a transcript request
            var transcript = await _assemblyService.CreateTranscriptAsync(uploadedUrl, language);
            // Step 3: Wait for the transcription to complete
            var completedTranscript = await _assemblyService.WaitForTranscriptToProcess(transcript);

            // Step 4: Get summary from OpenAI
            var summaryResponse = await _openAiService.GetChatResponseAsync(completedTranscript.Text);

            return ApiResponseHelper.ToActionResult(summaryResponse);


        }
    }
}
