using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.RecentGroupActivity;
using Noog_api.DTOs.Summary;
using Noog_api.Enums;
using Noog_api.Models;
using Noog_api.Models.Application;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services.IServices;
using Noog_api.Services.ProjectGroupServices;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BackendApi.Tests.Unit
{
    public class SummaryServiceTests
    {
        private readonly ISummaryRepository _mockRepo;
        private readonly SummaryService _summaryService;
        private readonly IRecentGroupActivityService _activityService;
        private readonly IGroupStorageService _groupStorageService;

        public SummaryServiceTests()
        {
            _mockRepo = Substitute.For<ISummaryRepository>();
            _activityService = Substitute.For<IRecentGroupActivityService>();
            _groupStorageService = Substitute.For<IGroupStorageService>();

            _summaryService = new SummaryService(
                _mockRepo,
                _activityService,
                _groupStorageService
            );
        }

        #region GetAllSummariesAsync
        [Fact]
        public async Task GetAllSummariesAsync_whenNoSummaries_ReturnsEmptyList()
        {
            // Arrange
            var pgId = Guid.NewGuid().ToString();
            _mockRepo.GetAllSummariesAsync(Arg.Any<Guid>())
                .Returns(new List<Summary>());
            // Act
            var result = await _summaryService.GetAllSummariesAsync(pgId);
            // Assert
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.Equal(StatusCodesEnum.Success, result.StatusCode);
            Assert.Equal("No summaries found", result.Message);
        }

        [Fact]
        public async Task GetAllSummariesAsync_whenSummariesExist_ReturnsSummaries()
        {
            // Arrange
            var pgId = Guid.NewGuid();
            var storageId = Guid.NewGuid();


            var summaries = new List<Summary>
            {
                new Summary { SummaryId = 1, Title = "A", Content = "X", GroupStorageId = storageId },
                new Summary { SummaryId = 2, Title = "B", Content = "Y", GroupStorageId = storageId}
            };
            var groupStorage = new GroupStorage
            {
                Id = storageId,
                Summaries = summaries,
                ProjectGroupId = pgId
            };
            _mockRepo.GetAllSummariesAsync(Arg.Any<Guid>())
                .Returns(summaries);
            // Act
            var result = await _summaryService.GetAllSummariesAsync(pgId.ToString());
            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(StatusCodesEnum.Success, result.StatusCode);
            Assert.Equal("A", result.Data[0].Title);
            Assert.Equal("Y", result.Data[1].Content);
        }

        [Fact]
        public async Task GetAllSummariesAsync_WhenInvalidGuid_ReturnBadRequest()
        {
            // Arrange
            string invalidPgId = "not-a-guid";
            // Act
            var result = await _summaryService.GetAllSummariesAsync(invalidPgId);
            //Assert 
            Assert.Equal(StatusCodesEnum.BadRequest, result.StatusCode);
            Assert.Equal("Invalid request due to incorrect project group id", result.Message);
            Assert.Null(result.Data);
        }
        #endregion

        #region GetSummaryByIdAsync
        [Fact]
        public async Task GetSummaryByIdAsync_WhenSummaryExists_ReturnsSummary()
        {
            //Arrange
            var pgId = Guid.NewGuid();
            var summary = new Summary { SummaryId = 1, Title = "Test Title", Content = "Test Summary" };
            _mockRepo.GetSummaryByIdAsync(1, pgId)
                .Returns(Task.FromResult<Summary?>(summary));

            //act
            var result = await _summaryService.GetSummaryByIdAsync(1, pgId.ToString());

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.SummaryId);
            Assert.Equal(StatusCodesEnum.Success, result.StatusCode);
            Assert.Equal("Summary loaded successfully", result.Message);

        }

        [Fact]
        public async Task GetSummaryByIdAsync_WHenSummaryDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var pgId = Guid.NewGuid().ToString();
            _mockRepo.GetSummaryByIdAsync(99, Arg.Any<Guid>())
                .Returns(Task.FromResult<Summary?>(null));
            //Act
            var result = await _summaryService.GetSummaryByIdAsync(99, pgId);
            //Assert
            Assert.Null(result.Data);
            Assert.Equal(StatusCodesEnum.NotFound, result.StatusCode);
            Assert.Equal("Summary not found", result.Message);
        }

        [Fact]
        public async Task GetSummaryByIdAsync_InvalidGuid_ReturnsBadRequest()
        {
            // Act
            var result = await _summaryService.GetSummaryByIdAsync(1, "invalid-guid");

            // Assert
            Assert.Equal(StatusCodesEnum.BadRequest, result.StatusCode);
            Assert.Equal("Invalid request due to incorrect project group id", result.Message);
        }
        #endregion

        #region CreateSummaryAsync
        [Fact]
        public async Task CreateSummaryAsync_WhenRepoSucceds_returnsCreated()
        {
            //arrange
            var dto = new CreateSummaryRequestDto { Title = "New Title", Content = "New Content", ProjectGroupId = Guid.NewGuid() };
            _activityService.AddNewActivityAsync(Arg.Any<CreateRecentSummaryRequest>())
                .Returns(Task.FromResult(true));
            _groupStorageService.GetGroupStorageById(dto.ProjectGroupId)
                .Returns(Task.FromResult(new GroupStorage { Id = Guid.NewGuid(), ProjectGroupId = dto.ProjectGroupId }));


            //Act
            var result = await _summaryService.CreateSummaryAsync(dto);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodesEnum.Created, result.StatusCode);
            Assert.Equal("Summary and recent activity created successfully ", result.Message);
            await _mockRepo.Received(1).CreateSummaryAsync(Arg.Any<Summary>());
            await _activityService.Received(1).AddNewActivityAsync(Arg.Any<CreateRecentSummaryRequest>());
        }

        [Fact]
        public async Task CreateSummaryAsync_WhenRepoThrows_ReturnsServerError()
        {
            //arrange
            var dto = new CreateSummaryRequestDto { Title = "New Title", Content = "New Content", ProjectGroupId = Guid.NewGuid() };

            _groupStorageService.GetGroupStorageById(dto.ProjectGroupId)
                .Returns(Task.FromResult(new GroupStorage
                {
                    Id = Guid.NewGuid(),
                    ProjectGroupId = dto.ProjectGroupId
                }));

            _mockRepo.When(x => x.CreateSummaryAsync(Arg.Any<Summary>()))
                     .Do(x => { throw new Exception("Database error"); });


            //Act
            var result = await _summaryService.CreateSummaryAsync(dto);
            //Assert
            Assert.Equal(StatusCodesEnum.ServerError, result.StatusCode);
            Assert.Equal("Database error", result.Message);
        }

        [Fact]
        public async Task CreateSummaryAsync_WhenTitleisNullOrEmpty_ReturnsBadRequest()
        {
            //arrange
            var dto = new CreateSummaryRequestDto {Title = "" ,Content = "New Content" , ProjectGroupId = Guid.NewGuid() };
            //Act
            var result = await _summaryService.CreateSummaryAsync(dto);
            //Assert
            Assert.Equal(StatusCodesEnum.BadRequest, result.StatusCode);
            Assert.Equal("Title and Content are required", result.Message);
        }

        #endregion
        

        #region DeleteSummaryAsync
        [Fact]
        public async Task DeleteSummaryAsync_WhenSummaryNotFound_ReturnsNotFound()
        {
            //Arrange
            _mockRepo.GetSummaryByIdAsync(99, Arg.Any<Guid>())
                .Returns(Task.FromResult<Summary?>(null));
            //Act
            var result = await _summaryService.DeleteSummaryAsync(99);
            //Assert

            Assert.Equal(StatusCodesEnum.NotFound, result.StatusCode);
            Assert.Equal("Summary not found", result.Message);

        }

        [Fact]
        public async Task DeleteSummaryAsync_WhenSuccess_ReturnsSuccess()
        {
            //Arrange
            var summary = new Summary { SummaryId = 1, Title = "Title", Content = "Content" };
            _mockRepo.GetSummaryByIdAsync(1, Arg.Any<Guid>()).Returns(Task.FromResult<Summary?>(summary));
            _mockRepo.DeleteSummaryAsync(1).Returns(Task.FromResult(true));

            //Act
            var result = await _summaryService.DeleteSummaryAsync(1);
            //Assert
            Assert.Equal(StatusCodesEnum.Success, result.StatusCode);
            Assert.Equal("Summary deleted successfully", result.Message);
 
        }
        [Fact]
        public async Task DeleteSummaryAsync_WhenRepoThrows_ReturnsServerError()
        {
            //Arrange
            var summary = new Summary { SummaryId = 1, Title = "Title", Content = "Content" };
            _mockRepo.GetSummaryByIdAsync(1, Arg.Any<Guid>()).Returns(Task.FromResult<Summary?>(summary));
            _mockRepo.When(x => x.DeleteSummaryAsync(1))
                     .Do(x => { throw new Exception("Database error"); });
            //Act
            var result = await _summaryService.DeleteSummaryAsync(1);
            //Assert
            Assert.Equal(StatusCodesEnum.ServerError, result.StatusCode);
            Assert.Equal("Database error", result.Message);
        }
        #endregion

    }
}
