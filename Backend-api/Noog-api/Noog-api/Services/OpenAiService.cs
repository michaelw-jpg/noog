using System.Net.Http.Headers;

namespace Noog_api.Services
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetChatResponseAsync(string prompt)
        {
            var endpoint = _configuration["OpenAI:Endpoint"];
            var apiKey = _configuration["OpenAI:ApiKey"];
            var deployment= _configuration["OpenAI:Deployment"];
            var apiVersion = "2024-12-01-preview";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }
    }
}
