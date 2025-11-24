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

            if (string.IsNullOrWhiteSpace(dto.audioUrl))
                return BadRequest("Audio URL cannot be empty.");

            Console.WriteLine(dto.projectGroupId);

            string guidToParse = dto.projectGroupId;

            Console.WriteLine(guidToParse);

            foreach (var item in guidToParse)
            {
                Console.Write($"{item},");
            }

            bool isGuid = Guid.TryParse(guidToParse, out Guid parsedGuid);

            Console.WriteLine(parsedGuid);

            var response = await _orchestrate.OrchestrateAsync(dto.audioUrl, parsedGuid, dto.language);
            return ApiResponseHelper.ToActionResult(response);

        }
    }
}
