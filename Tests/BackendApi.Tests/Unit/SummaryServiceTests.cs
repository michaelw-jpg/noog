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
    }
}
