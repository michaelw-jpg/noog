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

        public async Task<BaseResponseDto<ProjectGroupUser>>AddUserToProjectGroup(AddUserToProjectGroupDto request)
        {
          
            var isAdmin = await _projectGroupUserService.IsUserProjectGroupAdminAsync(request.ProjectGroupId, _currentUser.UserId);
            if (!isAdmin)
            {
                return new BaseResponseDto<ProjectGroupUser>(
                    Enums.StatusCodesEnum.Unauthorized,"Only admins can add users to the project group.",
                    null);

            }

            var isalreadyMember = await _projectGroupUserService.IsUserMemberOfProjectGroupAsync(request.ProjectGroupId, request.Email);
            if (isalreadyMember)
            {
                return new BaseResponseDto<ProjectGroupUser>(
                    Enums.StatusCodesEnum.Conflict,"User is already a member of the project group.",
                    null);
            }

            var result = await _projectGroupUserService.AddUserToProjectGroupAsync(request.ProjectGroupId, request.Email);

            if(result == null)
            {
                return new BaseResponseDto<ProjectGroupUser>(Enums.StatusCodesEnum.NotFound,
                    "User does not exist", null);
            }

            return new BaseResponseDto<ProjectGroupUser>(
                Enums.StatusCodesEnum.Created, "User added to project group successfully", null);

        }

    }
}
