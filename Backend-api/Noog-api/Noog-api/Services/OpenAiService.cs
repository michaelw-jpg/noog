using Azure;
using Azure.AI.OpenAI;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Services.IServices;
using OpenAI.Chat;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Noog_api.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly AzureOpenAIClient _client;
        private readonly string _deployment;

        public OpenAiService(HttpClient httpClient, IConfiguration configuration)
        {
            var endpoint = configuration["OpenAI:Endpoint"];
            var apiKey = configuration["OpenAI:ApiKey"];
            _deployment = configuration["OpenAI:Deployment"];

            if(string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(_deployment))
                throw new Exception("OpenAI configuration is missing");

            _client = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

        }

        public async Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(string prompt)
        {
            var chatClient = _client.GetChatClient(_deployment);

            var context = "Context: You are an assistant transcribing a voice call according to the transcript provided.Your task is to read the transcript below and write a concise summary.";
            var context2 = "Context: You are an assistant transcribing a voice call according to the transcript provided.Your task is to read the transcript below and write a concise summary. but translate to swedish";

            List<ChatMessage> chatHistory = [
            new SystemChatMessage(context2),
            new UserChatMessage(prompt),
            ];

            var response = await chatClient.CompleteChatAsync(chatHistory);
            var assistantMessage = response.Value.Content[0].Text;
            
            var result = new BaseResponseDto<OpenAIResponseDto>();
            result.Data = new OpenAIResponseDto
            {
                Message = assistantMessage
            };

            result.StatusCode = Enums.StatusCodesEnum.Success;
            return result;
        }
    }
}
