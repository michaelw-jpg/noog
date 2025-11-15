using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.GroupMeeting;
using Noog_api.DTOs.ProjectGroup;
using Noog_api.Mappers;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class ProjectGroupService(IProjectGroupRepository projectGroupRepository, ICurrentUserService currentUser,
        IProjectGroupUserService projectGroupUserService, IGroupMeetingService groupMeetingService, IGroupStorageService groupStorageService) : IProjectGroupService
    {
        private readonly IProjectGroupRepository _projectGroupRepository = projectGroupRepository;
        private readonly ICurrentUserService _currentUser = currentUser;
        private readonly IProjectGroupUserService _projectGroupUserService = projectGroupUserService;
        private readonly IGroupMeetingService _groupMeetingService = groupMeetingService;
        private readonly IGroupStorageService _groupStorageService = groupStorageService;
        // Implement service methods here

        public async Task<BaseResponseDto<string>> Create(ProjectGroupCreateDto request)
        {
            var projectGroupRequest = new ProjectGroup();
            GenericMapper.ApplyCreate(projectGroupRequest, request);

            var projectGroupResult = await _projectGroupRepository.CreateGroupProjectsAsync(projectGroupRequest);

            var groupProjectUser = new ProjectGroupUser
            {
                ProjectGroupId = projectGroupResult.Id,
                ApplicationUserId = _currentUser.UserId,
                IsAdmin = true
            };
            var groupProjectUserResult = await _projectGroupUserService.CreateProjectGroupUserAsync(groupProjectUser);
            
            //get callid from microservice
            var GroupMeeting = new GroupMeeting
            {
                CallId = "placeholder-call-id",
                ProjectGroupId = projectGroupResult.Id
            };
            
            //maybe add groupchat at one point
            var groupMeetingResult = await _groupMeetingService.CreateGroupMeetingAsync(GroupMeeting);
            var groupStorage = new GroupStorage
            {
                ProjectGroupId = projectGroupResult.Id
            };

            var groupStorageResult = await _groupStorageService.CreateGroupStorageAsync(groupStorage);

            //maybe add groupchat at one point

            var finalResult = new BaseResponseDto<string>(
                Enums.StatusCodesEnum.Created, "Project group created", "");

            return finalResult;
        }

        public async Task <BaseResponseDto<ProjectGroup>> Patch(ProjectGroupPatchDto request)
        {
            var projectGroup = await _projectGroupRepository.GetGroupProjectByIdAsync(request.Id);
            if (projectGroup is null)
            {
                 return new BaseResponseDto<ProjectGroup>(
                    Enums.StatusCodesEnum.NotFound, "Project group not found", null);
            }

            GenericMapper.ApplyPatch(projectGroup, request);
            var result = await _projectGroupRepository.PatchGroupProjectsAsync(projectGroup);

            var response = new BaseResponseDto<ProjectGroup>(
                Enums.StatusCodesEnum.Success, "Project group updated successfully", result);
            return response;
        }

        public async Task <BaseResponseDto<bool>> AddUserToProjectGroupAsync(Guid projectGroupId, string email)
        {
            var doesExist = await _projectGroupUserService.GetProjectGroupUserAsync(projectGroupId, email);
            if (doesExist is not null)
            {
                return new BaseResponseDto<bool>(
                    Enums.StatusCodesEnum.Conflict, "User already in project group", false);
            }



            var projectGroupUser = new ProjectGroupUser
            {
                ProjectGroupId = projectGroupId,
               // ApplicationUserId = userId,
                IsAdmin = false
            };
            var result = await _projectGroupUserService.CreateProjectGroupUserAsync(projectGroupUser);
            var response = new BaseResponseDto<bool>(
                Enums.StatusCodesEnum.Success, "User added to project group successfully", true);
            return response;
        }

        public async Task<BaseResponseDto<ProjectGroupByIdResponse>> GetProjectGroupByIdAsync(Guid id)
        {
            var response = await _projectGroupRepository.GetGroupProjectByIdAsync(id);

            if (response == null)
            {
                return new BaseResponseDto<ProjectGroupByIdResponse>(
                    Enums.StatusCodesEnum.NotFound, "Project group not found", null);
            }

            var data = new ProjectGroupByIdResponse
            {
                ProjectGroup = GenericMapper.ToDto<ProjectGroup,ProjectGroupById>(response),
                GroupMeeting = GenericMapper.ToDto<GroupMeeting, GroupMeetingInfo>(response.GroupMeeting)
            };

            var result = new BaseResponseDto<ProjectGroupByIdResponse>(
                Enums.StatusCodesEnum.Success, "Project group retrieved successfully", data);

            return result;


        }

      
    }
}
