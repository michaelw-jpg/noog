using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Models;
using Noog_api.Services;
using Noog_api.Services.IServices;
using NSubstitute;
using OpenAI.Responses;
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
        // TODO [Oliver] - Uppdatera tester

        /*
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
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Message = "This is a meeting summary"
                }
            };

            _openAiMock
                .GetChatResponseAsync(Arg.Any<string>())
                .Returns(expectedResponse);

            //Act
            var result = await _promptServiceMock.CreatePromptAsync(OpenAiPromptService.PromptType.ExecutiveSummary);

            //Assert
            Assert.Equal("This is a meeting summary", result);
            await _openAiMock.Received(1).GetChatResponseAsync(
                Arg.Is<string>(s =>
                    s.Contains("Highligt") &&
                    s.Contains("Keep it under 300 words")));
        }
        [Fact]
        public async Task CreatePromptAsync_WithInsightExtraciontSummary_CallsWithCorrectPrompt()
        {
            //Arrange
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Message = "This is an insight meeting summary"
                }
            };

            _openAiMock
                .GetChatResponseAsync(Arg.Any<string>())
                .Returns(expectedResponse);

            //Act
            var result = await _promptServiceMock.CreatePromptAsync(OpenAiPromptService.PromptType.InsightExtractionSummary);

            //Assert
            Assert.Equal("This is an insight meeting summary", result);
            await _openAiMock.Received(1).GetChatResponseAsync(
                Arg.Is<string>(s =>
                    s.Contains("Extract insight") &&
                    s.Contains("strategy budget and timelines")));
        }
        [Fact]
        public async Task CreatePromptAsync_WithClientCallSummary_CallsWithCorrectPrompt()
        {
            //Arrange
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Message = "This is a client call summary"
                }
            };

            _openAiMock
                .GetChatResponseAsync(Arg.Any<string>())
                .Returns(expectedResponse);

            //Act
            var result = await _promptServiceMock.CreatePromptAsync(OpenAiPromptService.PromptType.ClientCallSummary);

            //Assert
            Assert.Equal("This is a client call summary", result);
            await _openAiMock.Received(1).GetChatResponseAsync(
                Arg.Is<string>(s =>
                    s.Contains("client summary") &&
                    s.Contains("Use a professional tone")));
        }
        [Fact]
        public async Task CreatePromptAsync_WithDetailesSummary_CallsWithCorrectPrompt()
        {
            //Arrange
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Message = "Detailed Summary with key points highlighted"
                }
            };

            _openAiMock
                .GetChatResponseAsync(Arg.Any<string>())
                .Returns(expectedResponse);

            //Act
            var result = await _promptServiceMock.CreatePromptAsync(OpenAiPromptService.PromptType.DetailedSummary);

            //Assert
            Assert.Equal("Detailed Summary with key points highlighted", result);
            await _openAiMock.Received(1).GetChatResponseAsync(
                Arg.Is<string>(s =>
                    s.Contains("Provide a structured summary") &&
                    s.Contains("Open Questions or Follow-ups")));
        }
        [Theory]
        [InlineData(PromptType.ExecutiveSummary)]
        [InlineData(PromptType.DetailedSummary)]
        [InlineData(PromptType.ClientCallSummary)]
        [InlineData(PromptType.InsightExtractionSummary)]
        public async Task CreatePromptAsync_TwoPromptTypes_CallsOpenAiSerive(PromptType type)
        {
            //Arrange
            var expectedResponse = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = new OpenAIResponseDto
                {
                    Message = "Meeting summary"
                }
            };
            _openAiMock
                .GetChatResponseAsync(Arg.Any<string>())
                .Returns(expectedResponse);

            //Act
            var result = await _promptServiceMock.CreatePromptAsync(type);

            //Assert
            await _openAiMock.Received(1).GetChatResponseAsync(Arg.Any<string>());
        }
        */
    }
}
