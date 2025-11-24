using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs;
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


        [HttpPost]
        public async Task<ActionResult<SummaryResponseDto>> Orchestrate([FromBody] OrchestrateDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is missing.");

            Console.WriteLine("Raw projectGroupId:" + dto.projectGroupId);

            Console.WriteLine("Raw DTO JSON:");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(dto));

            if (string.IsNullOrWhiteSpace(dto.projectGroupId))
                return BadRequest("ProjectGroupId cannot be empty.");

            var trimmedId = dto.projectGroupId.Trim();
            if (!Guid.TryParse(trimmedId, out var projectGuid))
                return BadRequest($"Invalid ProjectGroupId format: {trimmedId}");

            if (string.IsNullOrWhiteSpace(dto.audioUrl))
                return BadRequest("Audio URL cannot be empty.");

            var response = await _orchestrate.OrchestrateAsync(dto.audioUrl, projectGuid, dto.language);
            return ApiResponseHelper.ToActionResult(response);

        }
    }
}
