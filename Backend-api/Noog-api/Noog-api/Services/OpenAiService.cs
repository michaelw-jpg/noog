using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Services.IServices;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Noog_api.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(string prompt)
        {
            var endpoint = _configuration["OpenAI:Endpoint"];
            var apiKey = _configuration["OpenAI:ApiKey"];
            var deployment= _configuration["OpenAI:Deployment"];
            var apiVersion = "2024-12-01-preview";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                max_tokens = 500,
                temperature = 0.7,

            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var url = $"{endpoint}/openai/deployments/{deployment}/chat/completions?api-version={apiVersion}";

            var result = new BaseResponseDto<OpenAIResponseDto>();

            //sends request
            var response = await _httpClient.PostAsync(url, content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                result.StatusCode = Enums.StatusCodesEnum.ServerError;
                result.Message = $"Error: {ex.Message}";
                return result;
            }
            

            var responseContent = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseContent);
            var message = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            result.Data = new OpenAIResponseDto
            {
                Message = message ?? string.Empty
            };
            result.StatusCode = Enums.StatusCodesEnum.Success;


            return result;

        }
    }
}
