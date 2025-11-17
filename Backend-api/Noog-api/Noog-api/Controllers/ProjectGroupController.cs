using Azure;
using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs.ProjectGroup;
using Noog_api.Helpers;
using Noog_api.Models.Application;
using Noog_api.Services;
using Noog_api.Services.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectGroupController(IProjectGroupService projectGroupService) : ControllerBase
    {
        private readonly IProjectGroupService _projectGroupService = projectGroupService;

        // GET: api/<ProjectGroupController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectGroupByIdResponse>> GetById(Guid id)
        {
            var response = await _projectGroupService.GetProjectGroupByIdAsync(id);
            return ApiResponseHelper.ToActionResult(response);
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<ProjectGroupUser>> AddUserToProjectGroup(AddUserToProjectGroupDto request)
        {
            var response = await _projectGroupService.AddUserToProjectGroup(request);
            return ApiResponseHelper.ToActionResult(response);
        }

        // POST api/<ProjectGroupController>
        [HttpPost]
        public async Task<ActionResult<string?>> Create(ProjectGroupCreateDto request)
        {
            var response = await _projectGroupService.Create(request);
            return ApiResponseHelper.ToActionResult(response);
        }

        // PUT api/<ProjectGroupController>/5
        [HttpPatch]
        public async Task<ActionResult<ProjectGroup>> Patch(ProjectGroupPatchDto request)
        {
            var response = await _projectGroupService.Patch(request);
            return ApiResponseHelper.ToActionResult(response);
        }

        // DELETE api/<ProjectGroupController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
