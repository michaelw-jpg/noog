using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.ProjectGroup;
using Noog_api.Models.Application;

namespace Noog_api.Services.IServices
{
    public interface IProjectGroupService
    {
        Task<BaseResponseDto<string?>> Create(ProjectGroupCreateDto request);

        Task<BaseResponseDto<ProjectGroup>> Patch(ProjectGroupPatchDto request);
    }
}
