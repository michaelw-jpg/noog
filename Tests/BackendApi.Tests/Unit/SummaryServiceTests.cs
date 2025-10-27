using Xunit;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noog_api.Repositories.IRepositories;
using Noog_api.Services;
using Noog_api.Models;
using Noog_api.Enums;
using Noog_api.DTOs;

namespace BackendApi.Tests.Unit
{
    public class SummaryServiceTests
    {
        private readonly ISummaryRepository _mockRepo;
        private readonly SummaryService _summaryService;

        public SummaryServiceTests()
        {
            _mockRepo = Substitute.For<ISummaryRepository>();
            _summaryService = new SummaryService(_mockRepo);
        }

        #region GetAllSummariesAsync
        [Fact]
        public async Task GetAllSummariesAsync_whenNoSummaries_ReturnsEmptyList()
        {
            // Arrange
            _mockRepo.GetAllSummariesAsync().Returns(Task.FromResult(new List<Summary>()));
            // Act
            var result = await _summaryService.GetAllSummariesAsync();
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
            var summaries = new List<Summary>
            {
                new Summary { SummaryId = 1, Title = "A", Content = "X" },
                new Summary { SummaryId = 2, Title = "B", Content = "Y" }
            };
            _mockRepo.GetAllSummariesAsync().Returns(Task.FromResult(summaries));
            // Act
            var result = await _summaryService.GetAllSummariesAsync();
            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(StatusCodesEnum.Success, result.StatusCode);
            Assert.Equal("A", result.Data[0].Title);
            Assert.Equal("Y", result.Data[1].Content);
        }
        #endregion

        #region GetSummaryByIdAsync
        [Fact]
        public async Task GetSummaryByIdAsync_WhenSummaryExists_ReturnsSummary()
        {
            //Arrange
            var summary = new Summary { SummaryId = 1, Title = "Test Title", Content = "Test Summary" };
            _mockRepo.GetSummaryByIdAsync(1).Returns(Task.FromResult<Summary?>(summary));

            //act
            var result = await _summaryService.GetSummaryByIdAsync(1);

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
            _mockRepo.GetSummaryByIdAsync(99).Returns(Task.FromResult<Summary?>(null));
            //Act
            var result = await _summaryService.GetSummaryByIdAsync(99);
            //Assert
            Assert.Null(result.Data);
            Assert.Equal(StatusCodesEnum.NotFound, result.StatusCode);
            Assert.Equal("Summary not found", result.Message);
        }
        #endregion

        #region CreateSummaryAsync
        [Fact]
        public async Task CreateSummaryAsync_WhenRepoSucceds_returnsCreated()
        {
            //arrange
            var dto = new CreateSummaryDto { Title = "New Title", Content = "New Content" };

            //Act
            var result = await _summaryService.CreateSummaryAsync(dto);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodesEnum.Created, result.StatusCode);
            Assert.Equal("Summary created successfully", result.Message);
            await _mockRepo.Received(1).CreateSummaryAsync(Arg.Any<Summary>());
        }

        [Fact]
        public async Task CreateSummaryAsync_WhenRepoThrows_ReturnsServerError()
        {
            //arrange
            var dto = new CreateSummaryDto { Title = "New Title", Content = "New Content" };
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
            var dto = new CreateSummaryDto {Title = "" ,Content = "New Content" };
            //Act
            var result = await _summaryService.CreateSummaryAsync(dto);
            //Assert
            Assert.Equal(StatusCodesEnum.BadRequest, result.StatusCode);
            Assert.Equal("Title and Content are required", result.Message);
        }

        #endregion

        #region UpdateSummaryAsync
        [Fact]
        public async Task UpdateSummaryAsync_WhenSummaryNotFound_ReturnsNotFound()
        {
            //Arrange
            var dto = new PatchSummaryDto { Title = "Updated Title", Content = "Updated Content" };
            _mockRepo.GetSummaryByIdAsync(99).Returns(Task.FromResult<Summary?>(null));
            //Act
            var result = await _summaryService.UpdateSummaryAsync(99, dto);
            //Assert
            Assert.Null(result.Data);
            Assert.Equal(StatusCodesEnum.NotFound, result.StatusCode);
            Assert.Equal("Summary not found", result.Message);
        }

        [Fact]
        public async Task UpdateSummaryAsync_WhenRepoSucceds_Returns()
        {
            //Arrange
            var summary = new Summary { SummaryId = 1, Title = "Old Title", Content = "Old Content" };
            _mockRepo.GetSummaryByIdAsync(1).Returns(Task.FromResult<Summary?>(summary));
            var patchDto = new PatchSummaryDto { Title = "Updated Title"};

            //Act
            var result = await _summaryService.UpdateSummaryAsync(1, patchDto);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal("Updated Title", result.Data.Title);
            Assert.Equal("Old Content", result.Data.Content);
            Assert.Equal(StatusCodesEnum.Success, result.StatusCode);
            Assert.Equal("Summary updated successfully", result.Message);

            await _mockRepo.Received(1).UpdateSummaryAsync(1, Arg.Any<Summary>());
        }

        [Fact]
        public async Task UpdateSummaryAsync_WhenRepoThrows_ReturnsServerError()
        {
            //Arrange
            var summary = new Summary { SummaryId = 1, Title = "Old Title", Content = "Old Content" };
            _mockRepo.GetSummaryByIdAsync(1).Returns(Task.FromResult<Summary?>(summary));
            var patchDto = new PatchSummaryDto { Title = "Updated Title", Content = "Updated Content" };
            _mockRepo.When(x => x.UpdateSummaryAsync(1, Arg.Any<Summary>()))
                     .Do(x => { throw new Exception("Database error"); });

            //Act
            var result = await _summaryService.UpdateSummaryAsync(1, patchDto);

            //Assert
            Assert.Equal(StatusCodesEnum.ServerError, result.StatusCode);
            Assert.Equal("Database error", result.Message);
        }
        #endregion

        #region DeleteSummaryAsync
        [Fact]
        public async Task DeleteSummaryAsync_WhenSummaryNotFound_ReturnsNotFound()
        {
            //Arrange
            _mockRepo.GetSummaryByIdAsync(99).Returns(Task.FromResult<Summary?>(null));
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
            _mockRepo.GetSummaryByIdAsync(1).Returns(Task.FromResult<Summary?>(summary));
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
            _mockRepo.GetSummaryByIdAsync(1).Returns(Task.FromResult<Summary?>(summary));
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
