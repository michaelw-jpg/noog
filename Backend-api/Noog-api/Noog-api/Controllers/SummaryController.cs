using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.Summary;
using Noog_api.Helpers;
using Noog_api.Services.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryController (ISummaryService summaryService) : ControllerBase

    {
       private readonly ISummaryService _summaryService = summaryService;

        // GET: /summary/projectgroup/{pgId}
        [HttpGet("projectgroup/{pgId}")]
        public async Task <ActionResult<List<SummaryResponseDto>>> GetAllSummaries(string pgId)
        {
            var response = await _summaryService.GetAllSummariesAsync(pgId);

            return ApiResponseHelper.ToActionResult(response);

        }

        // GET: /summary/{id}/projectgroup/{pgId}
        [HttpGet("{id}/projectgroup/{pgId}")]
        public async Task<ActionResult<SummaryResponseDto>> GetByID(int id, string pgId)
        {
            var response = await _summaryService.GetSummaryByIdAsync(id, pgId);
            return ApiResponseHelper.ToActionResult(response);
        }

        // POST api/<SummaryController>
        [HttpPost]
        public async Task <ActionResult<SummaryResponseDto>> Post([FromBody] CreateSummaryRequestDto request)
        {
            var response = await _summaryService.CreateSummaryAsync(request);
            return ApiResponseHelper.ToActionResult(response);
        }

        // DELETE api/<SummaryController>/5
        [HttpDelete("{id}")]
        public async Task <ActionResult<int>> Delete(int id)
        {
            var response = await _summaryService.DeleteSummaryAsync(id);
            return ApiResponseHelper.ToActionResult(response);
        }
    }
}
