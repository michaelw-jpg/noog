using System;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.ProjectGroup;
using Noog_api.Enums;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services;
using Noog_api.Services.IServices;
using Noog_api.Services.ProjectGroupServices;
using NSubstitute;
using Xunit;

namespace BackendApi.Tests.Unit
{
    public class ProjectServiceTests
    {
        private readonly IProjectGroupService _projectGroupService;
        private readonly IProjectGroupRepository _projectRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly IProjectGroupUserService _projectGroupUserService;
        private readonly IGroupMeetingService _groupMeetingService;
        private readonly IGroupStorageService _groupStorageService;
        private readonly StreamIOService _streamIOService;
        private readonly ProjectGroupService _service;


        public ProjectServiceTests()
        {
            _projectGroupService = Substitute.For<IProjectGroupService>();
            _projectRepo = Substitute.For<IProjectGroupRepository>();
            _currentUser = Substitute.For<ICurrentUserService>();
            _projectGroupUserService = Substitute.For<IProjectGroupUserService>();
            _groupMeetingService = Substitute.For<IGroupMeetingService>();
            _groupStorageService = Substitute.For<IGroupStorageService>();
            _streamIOService = Substitute.For<StreamIOService>();

            _service = new ProjectGroupService(
                _projectRepo,
                _currentUser,
                _projectGroupUserService,
                _groupMeetingService,
                _groupStorageService,
                _streamIOService
            );
        }

        #region CreateProjectGroupAsync
        [Fact]
        public async Task Create_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new ProjectGroupCreateDto
            {
                Name = "My Project",
                ImageUrl = "https://example.com/image.png"
            };

            var createdGroup = new ProjectGroup { Id = Guid.NewGuid(), Name = "My Project" };

            _projectRepo.CreateGroupProjectsAsync(Arg.Any<ProjectGroup>())
                .Returns(createdGroup);

            _currentUser.UserId.Returns(Guid.NewGuid());

            _projectGroupUserService.CreateProjectGroupUserAsync(Arg.Any<ProjectGroupUser>())
                .Returns(new ProjectGroupUser());

            _groupMeetingService.CreateGroupMeetingAsync(Arg.Any<GroupMeeting>())
                .Returns(new GroupMeeting());

            _groupStorageService.CreateGroupStorageAsync(Arg.Any<GroupStorage>())
                .Returns(new GroupStorage());

            // Act
            var result = await _service.Create(request);

            // Assert
            Assert.Equal(StatusCodesEnum.Created, result.StatusCode);
            Assert.Equal("Project group created", result.Message);

            // Verify correct calls
            await _projectRepo.Received(1).CreateGroupProjectsAsync(Arg.Any<ProjectGroup>());
            await _projectGroupUserService.Received(1).CreateProjectGroupUserAsync(Arg.Any<ProjectGroupUser>());
            await _groupMeetingService.Received(1).CreateGroupMeetingAsync(Arg.Any<GroupMeeting>());
            await _groupStorageService.Received(1).CreateGroupStorageAsync(Arg.Any<GroupStorage>());
        }

        [Fact]
        public async Task CreateProjectGroupAsync_NullRequest_ReturnsBadRequest()
        {
            // Arrange
            ProjectGroupCreateDto request = null;

            var expectedResponse = new BaseResponseDto<string>
            {
                StatusCode = StatusCodesEnum.BadRequest,
                Message = "Invalid request",
                Data = null
            };

            _projectGroupService.Create(null).Returns(expectedResponse);

            // Act
            var result = await _projectGroupService.Create(request);

            // Assert
            Assert.Equal(StatusCodesEnum.BadRequest, result.StatusCode);
            Assert.Equal("Invalid request", result.Message);
        }

        [Fact]
        public async Task CreateProjectGroupAsync_EmptyName_ReturnsValidationError()
        {
            // Arrange
            var request = new ProjectGroupCreateDto
            {
                Name = "",
                ImageUrl = "https://image.com"
            };

            var expectedResponse = new BaseResponseDto<string>
            {
                StatusCode = StatusCodesEnum.BadRequest,
                Message = "Name is required",
                Data = null
            };

            _projectGroupService.Create(Arg.Any<ProjectGroupCreateDto>())
                .Returns(expectedResponse);

            // Act
            var result = await _projectGroupService.Create(request);

            // Assert
            Assert.Equal(StatusCodesEnum.BadRequest, result.StatusCode);
            Assert.Equal("Name is required", result.Message);
        }

        [Fact]
        public async Task CreateProjectGroupAsync_ServiceThrows_ReturnsInternalError()
        {
            // Arrange
            var request = new ProjectGroupCreateDto
            {
                Name = "Test",
                ImageUrl = "https://image.com"
            };

            _projectGroupService
                .When(s => s.Create(Arg.Any<ProjectGroupCreateDto>()))
                .Do(x => throw new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _projectGroupService.Create(request));
        }

    }
}
#endregion