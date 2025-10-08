using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
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

        // GET: api/<SummaryController>
        [HttpGet]
        public async Task <ActionResult<List<SummaryResponseDto>>> GetAllSummeries()
        {
            var response = await _summaryService.GetAllSummariesAsync();

            return ApiResponseHelper.ToActionResult(response);

        }

        // GET api/<SummaryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SummaryResponseDto>> GetByID(int id)
        {
            var response = await _summaryService.GetSummaryByIdAsync(id);
            return ApiResponseHelper.ToActionResult(response);
        }

        // POST api/<SummaryController>
        [HttpPost]
        public async Task <ActionResult<SummaryResponseDto>> Post([FromBody] CreateSummaryDto request)
        {
            var response = await _summaryService.CreateSummaryAsync(request);
            return ApiResponseHelper.ToActionResult(response);
        }

        // PUT api/<SummaryController>/5
        [HttpPatch("{id}")]
        public async Task <ActionResult<SummaryResponseDto>> Patch(int id, [FromBody] PatchSummaryDto request)
        {
            var response = await _summaryService.UpdateSummaryAsync(id, request);
            return ApiResponseHelper.ToActionResult(response);
        }

        // DELETE api/<SummaryController>/5
        [HttpDelete("{id}")]
        public async Task <ActionResult<int>> Delete(int id)
        {
            var response = await _summaryService.DeleteSummaryAsync(id);
            return ApiResponseHelper.ToActionResult(response);
            {

            }
        }
    }
}
