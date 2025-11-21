using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs;
using Noog_api.DTOs.Dashboard;
using Noog_api.Helpers;
using Noog_api.Services.Dashboard;

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    // TODO - Reenable when a user can log in through mvc
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: api/dashboard
        [HttpGet]
        public async Task<ActionResult<DashboardDataResponseDto>> GetDashboardData()
        {
            Console.WriteLine(HttpContext.User.Identity.Name);
            var response = await _dashboardService.GetDashboardDataAsync();

            return ApiResponseHelper.ToActionResult(response);
        }

        // GET: api/dashboard/projectgroups
        [HttpGet("projectgroups")]
        public async Task<ActionResult<List<ProjectGroupResponseDto>>> GetDashboardUserProjectGroups()
        {
            var response = await _dashboardService.GetDashboardUserProjectGroupsAsync();

            return ApiResponseHelper.ToActionResult(response);
        }
    }
}
