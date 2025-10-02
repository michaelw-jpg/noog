using Microsoft.AspNetCore.Mvc;
using Noog_api.Services;
using Noog_api.Models.AssemblyAi;

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientReceiverController : ControllerBase
    {
        private readonly AssemblyAiService _assemblyAiService;

        public ClientReceiverController(AssemblyAiService assemblyAiService)
        {
            _assemblyAiService = assemblyAiService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AudioUrlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
                return BadRequest("Audio URL cannot be empty.");

            try
            {
                string uploadedUrl = await _assemblyAiService.UploadFileAsync(request.Url);

                var transcript = await _assemblyAiService.CreateTranscriptAsync(uploadedUrl, request.Language);

                var completed = await _assemblyAiService.WaitForTranscriptToProcess(transcript);

                return Ok(completed);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
