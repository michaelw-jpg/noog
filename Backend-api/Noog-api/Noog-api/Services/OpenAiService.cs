using Azure;
using Azure.AI.OpenAI;
using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Models.AssemblyAi;
using Noog_api.Services.IServices;
using OpenAI.Chat;
using System.Net.Http.Headers;
using System.Text.Json;
using static Noog_api.Services.OpenAiPromptService;

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

        
        public async Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(PromptType type, string transcript, string language)
        {
            
            if (!Prompts.TryGetValue(type, out var additionalInstruction))
            {
                throw new ArgumentException($"No instruction found for {type}");
            }

            string summaryLanguage = language ?? "the same language as the transcript";
            string finalPrompt = transcript;

            var chatClient = _client.GetChatClient(_deployment);


            var context =
               $$"""
                Context: 
                You are an assistant summarizing a voice transcript. 

                Instructions:
                - Return a concise summary in {{summaryLanguage}}. 
                - Additional instruction: {{additionalInstruction}}
                - Return a title for the summary.
                - Return your answer strictly as valid JSON.
                - Use the following schema:

                { 
                    "title": "string", 
                    "summary": "string" 
                }

                Important:                
                - Do Not add explanation, commentary, or any extra text outside the JSON object. 
                """;

            var chatHistory = new List<ChatMessage>
            {
                new SystemChatMessage(context),
                new UserChatMessage(finalPrompt),
            };

            var response = await chatClient.CompleteChatAsync(chatHistory, new ChatCompletionOptions { Temperature = 0.2f, MaxOutputTokenCount = 600 });

            var assistantMessage = response.Value.Content[0].Text;

            if (assistantMessage == null)
            {
                return new BaseResponseDto<OpenAIResponseDto>(
                    Enums.StatusCodesEnum.NotFound,
                    "No response from OpenAi.",
                    null);
            }

            OpenAIResponseDto parsed;

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            try
            {
                parsed = JsonSerializer.Deserialize<OpenAIResponseDto>(assistantMessage, jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to deserialize JSON from model:");
                Console.WriteLine(assistantMessage);
                Console.WriteLine(ex);

                // Create response with empty fields so service doesn't crash
                parsed = new OpenAIResponseDto
                {
                    Title = "Invalid JSON From Model",
                    Summary = assistantMessage
                };
            }

            var result = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = parsed,
                StatusCode = Enums.StatusCodesEnum.Success
            };


            return result;
        }

        
        public async Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(string prompt)
        {
            var chatClient = _client.GetChatClient(_deployment);

            var context =
                """
                Context: 
                You are an assistant summarizing a voice transcript. 

                Instructions:
                - Return a concise summary in the same language as the transcript. 
                - Return a title for the summary.
                - Return your answer strictly as valid JSON.
                - Use the following schema:

                { 
                    "title": "string", 
                    "summary": "string" 
                }

                Important:                
                - Do Not add explanation, commentary, or any extra text outside the JSON object. 
                """;

            var chatHistory = new List<ChatMessage>
            {
                new SystemChatMessage(context),
                new UserChatMessage(prompt),
            };

            var response = await chatClient.CompleteChatAsync(chatHistory, new ChatCompletionOptions { Temperature = 0.2f, MaxOutputTokenCount = 600 });
            var assistantMessage = response.Value.Content[0].Text;


            if (assistantMessage == null)
            {
                return new BaseResponseDto<OpenAIResponseDto>(
                    Enums.StatusCodesEnum.NotFound,
                    "No response from OpenAi.",
                    null);
            }

            OpenAIResponseDto parsed;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                parsed = JsonSerializer.Deserialize<OpenAIResponseDto>(assistantMessage, jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to deserialize JSON from model:");
                Console.WriteLine(assistantMessage);
                Console.WriteLine(ex);

                // Create response with empty fields so service doesn't crash
                parsed = new OpenAIResponseDto
                {
                    Title = "Invalid JSON From Model",
                    Summary = assistantMessage
                };
            }

            var result = new BaseResponseDto<OpenAIResponseDto>
            {
                Data = parsed,
                StatusCode = Enums.StatusCodesEnum.Success
            };


            return result;
        }
    }
}
