using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.Summary;
using Noog_api.Helpers;
using Noog_api.Services.IServices;

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrchestrateController(IOrchestrateService orchestrate) : ControllerBase
    {
        private readonly IOrchestrateService _orchestrate = orchestrate;


        [HttpGet]
        public async Task<ActionResult<SummaryResponseDto>> Orchestrate(string audioUrl,Guid projectGroupId, string? language = null)
           
        {
            if (string.IsNullOrWhiteSpace(audioUrl))
                return BadRequest("Audio URL cannot be empty.");
            var response = await _orchestrate.OrchestrateAsync(audioUrl, projectGroupId, language);

            return ApiResponseHelper.ToActionResult(response);


        }
    }
}
