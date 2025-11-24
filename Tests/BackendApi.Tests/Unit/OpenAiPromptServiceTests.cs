using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Enums;
using Noog_api.Models;
using Noog_api.Models.AssemblyAi;
using Noog_api.Services;
using Noog_api.Services.IServices;
using NSubstitute;
using OpenAI.Responses;
using StreamChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Noog_api.Services.OpenAiPromptService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendApi.Tests.Unit
{
    public class OpenAiPromptServiceTests
    {
        //TODO[Oliver] - Uppdatera tester

        private readonly IOpenAiService _openAiMock;
        private readonly OpenAiPromptService _promptServiceMock;

        public OpenAiPromptServiceTests()
        {
            _openAiMock = Substitute.For<IOpenAiService>();
            _promptServiceMock = new OpenAiPromptService(_openAiMock);
        }

        [Fact]
        public async Task CreatePromptAsync_WithExecutiveSummary_CallsWithCorrectPrompt()
        {
            //Arrange
            var transcript = "This is a meeting transcript with project discussions";
            var language = "English";
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Title = "Executive summary",
                    Summary = "This is a meeting summary"
                }
            };

            _openAiMock
                .GetChatResponseAsync(Arg.Any<PromptType>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(expectedResponse);

            //Act
            var result = await _promptServiceMock.CreatePromptAsync(PromptType.ExecutiveSummary, transcript, language);



           // Assert
            await _openAiMock.Received(1).GetChatResponseAsync(
                Arg.Is<PromptType>(pt => pt == PromptType.ExecutiveSummary),
                Arg.Is<string>(s => s == transcript),
                Arg.Is<string>(l => l == language));
        }
        [Fact]
        public async Task CreatePromptAsync_WithInsightExtraciontSummary_CallsWithCorrectPrompt()
        {
            //Arrange
            var transcript = "This is the meeting transcript focused on insight extraction.";
            var language = "English";
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Title = "Extraction insight summary",
                    Summary = "This is an insight meeting summary"
                }
            };

            _openAiMock
                 .GetChatResponseAsync(Arg.Any<PromptType>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(expectedResponse);

            // Act
            var result = await _promptServiceMock.CreatePromptAsync(PromptType.InsightExtractionSummary, transcript, language);

            //Assert
            await _openAiMock.Received(1).GetChatResponseAsync(
                 Arg.Is<PromptType>(pt => pt == PromptType.InsightExtractionSummary),
                 Arg.Is<string>(s => s == transcript),
                 Arg.Is<string>(l => l == language));
        }
        [Fact]
        public async Task CreatePromptAsync_WithClientCallSummary_CallsWithCorrectPrompt()
        {
            //Arrange
            var transcript = "This is the meeting transcript for client call summary.";
            var language = "English";
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Title = "Client call summary",
                    Summary = "This is a client call summary"
                }
            };

            _openAiMock
                .GetChatResponseAsync(Arg.Any<PromptType>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(expectedResponse);

            // Act
            var result = await _promptServiceMock.CreatePromptAsync(PromptType.ClientCallSummary, transcript, language);

            //Assert
            await _openAiMock.Received(1).GetChatResponseAsync(
                Arg.Is<PromptType>(pt => pt == PromptType.ClientCallSummary),
                Arg.Is<string>(s => s == transcript),
                Arg.Is<string>(l => l == language));
        }
        [Fact]
        public async Task CreatePromptAsync_WithDetailesSummary_CallsWithCorrectPrompt()
        {
            //Arrange
            var transcript = "This is the meeting transcript summamry focused on details.";
            var language = "English";
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Title = "Detailed summary",
                    Summary = "Detailed Summary with key points highlighted"
                }
            };

            _openAiMock
                .GetChatResponseAsync(Arg.Any<PromptType>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(expectedResponse);

            //Act
            var result = await _promptServiceMock.CreatePromptAsync(PromptType.DetailedSummary, transcript, language);

            //Assert
            await _openAiMock.Received(1).GetChatResponseAsync(
                Arg.Is<PromptType>(pt => pt == PromptType.DetailedSummary),
                Arg.Is<string>(s => s == transcript),
                Arg.Is<string>(l => l == language));
        }
        [Theory]
        [InlineData(PromptType.ExecutiveSummary, "English", "Executive summary")]
        [InlineData(PromptType.DetailedSummary, "English", "Detailed summary")]
        [InlineData(PromptType.ClientCallSummary, "English", "Client call summary")]
        [InlineData(PromptType.InsightExtractionSummary, "English", "Insight extraction summary")]
        public async Task CreatePromptAsync_TwoPromptTypes_CallsOpenAiSerive(PromptType type, string language, string expectedMessage)
        {
            //Arrange
            var transcript = "Test transcrip";
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Title = "Cooked",
                    Summary = expectedMessage
                },
            };
            _openAiMock
               .GetChatResponseAsync(Arg.Any<PromptType>(), Arg.Any<string>(), Arg.Any<string>())
               .ReturnsForAnyArgs(Task.FromResult(expectedResponse));

            //Act
            var result = await _promptServiceMock.CreatePromptAsync(type, transcript, language);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result);
            await _openAiMock.Received(1).GetChatResponseAsync(
                Arg.Is<PromptType>(pt => pt == type),
                Arg.Any<string>(),
                Arg.Is<string>(l => l == language));
        }

    }
}
