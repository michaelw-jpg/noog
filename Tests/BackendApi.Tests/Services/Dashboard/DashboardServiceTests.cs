using FluentAssertions;
using Noog_api.Enums;
using Noog_api.Models;
using Noog_api.Models.Application;
using Noog_api.Models.Application.Enums;
using Noog_api.Services.Dashboard;
using Noog_api.Services.IServices;
using Noog_api.Services.ProjectGroupServices;
using Noog_api.Services.UserServices;
using NSubstitute;

namespace BackendApi.Tests.Services.Dashboard
{
    /// <summary>
    /// Simons tester
    /// NSubstitute & FluentAssertations
    /// </summary>
    public class DashboardServiceTests
    {
        private readonly IRecentGroupActivityService _activityService = Substitute.For<IRecentGroupActivityService>();
        private readonly IUserService<ApplicationUser> _userService = Substitute.For<IUserService<ApplicationUser>>();
        private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
        private readonly IProjectGroupUserService _pgUserService = Substitute.For<IProjectGroupUserService>();

        private DashboardService CreateService()
        {
            return new DashboardService(
                _activityService,
                _userService,
                _currentUserService,
                _pgUserService
            );
        }


        [Fact]
        public async Task GetDashboardDataAsync_ShouldReturnNotFound_WhenUserIsNull()
        {
            // Arrange
            var service = CreateService();
            var userId = Guid.NewGuid();

            _currentUserService.UserId.Returns(userId);
            _userService.FindByIdAsync(userId).Returns((ApplicationUser?)null);

            // Act
            var result = await service.GetDashboardDataAsync();

            // Assert
            result.StatusCode.Should().Be(StatusCodesEnum.NotFound);
            result.Message.Should().Be("Error getting user");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetDashboardDataAsync_ShouldReturnSuccess_WhenUserExists()
        {
            // Arrange
            var service = CreateService();
            var userId = Guid.NewGuid();

            _currentUserService.UserId.Returns(userId);

            _userService.FindByIdAsync(userId).Returns(new ApplicationUser
            {
                UserName = "SimonEke",
                ImgProfile = "profil.png"
            });

            _activityService.GetTopThreeLatestActivitesAsync().Returns(new List<RecentGroupActivity>
            {
                new RecentGroupActivity
                {
                    Title = "New Summary Created",
                    ProjectGroup = new ProjectGroup { Name = "Test Group 1", ImageUrl = "TestGroup.png" },
                    SourceType = SourceType.Meeting
                }
            });

            // Act
            var result = await service.GetDashboardDataAsync();

            // Assert
            result.StatusCode.Should().Be(StatusCodesEnum.Success);
            result.Message.Should().Be("Dashboard Data loaded successfully");
            result.Data.Should().NotBeNull();

            result.Data.User.UserName.Should().Be("SimonEke");
            result.Data.User.ProfileImageUrl.Should().Be("profil.png");

            result.Data.RecentActivities.Should().HaveCount(1);
            var activity = result.Data.RecentActivities.First();

            activity.ActivityTitle.Should().Be("New Summary Created");
            activity.ProjectGroupName.Should().Be("Test Group 1");
            activity.ProjectGroupImageUrl.Should().Be("TestGroup.png");
            activity.Source.Should().Be(SourceType.Meeting.ToString());
        }

        [Fact]
        public async Task GetDashboardDataAsync_ShouldReturnSuccess_WhenActivityListIsEmpty()
        {
            // Arrange
            var service = CreateService();
            var userId = Guid.NewGuid();

            _currentUserService.UserId.Returns(userId);

            _userService.FindByIdAsync(userId).Returns(new ApplicationUser
            {
                UserName = "Simon",
                ImgProfile = "ProfilBild.png"
            });

            _activityService.GetTopThreeLatestActivitesAsync()
                .Returns(new List<RecentGroupActivity>());

            // Act
            var result = await service.GetDashboardDataAsync();

            // Assert
            result.StatusCode.Should().Be(StatusCodesEnum.Success);
            result.Data.RecentActivities.Should().BeEmpty();
            result.Data.User.UserName.Should().Be("Simon");
        }
        /*
        [Fact]
        public async Task GetDashboardUserProjectGroupsAsync_ShouldReturnProjectGroups()
        {

        } */
    }
}
