using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Noog_api.Models.AssemblyAi;
using Noog_api.Services.IServices;
namespace Noog_api.Services
{
    public class AssemblyAiService : IAssemblyAiService
    {
        private static readonly string _BaseUrl = "https://api.assemblyai.com/v2";
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public AssemblyAiService(IConfiguration config, HttpClient httpClient)
        {
            _apiKey = config["ApiKeys:AssemblyAi"]
                ?? throw new ArgumentNullException("AssemblyAI:ApiKey is not set in configuration");

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri($"{_BaseUrl}/");
            // AssemblyAI expects the raw API key in the Authorization header 
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
            _httpClient.DefaultRequestHeaders.Add("Authorization", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<String> UploadFileAsync(string fileUrl)
        {
            using var fileStream =  await new HttpClient().GetStreamAsync(fileUrl);

            using var streamContent = new StreamContent(fileStream);
            {
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                using var response = await _httpClient.PostAsync("upload", streamContent);
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<UploadResponse>();
                    if (string.IsNullOrWhiteSpace(result?.UploadUrl))
                    {
                        throw new Exception("AssemblyAI upload returned empty upload_url.");
                    }
                    return result.UploadUrl;
                }
            }
        }

        public async Task<Transcript> CreateTranscriptAsync(string audioUrl)
        {
            var data = new
            {
                audio_url = audioUrl,
                language_code = "sv"
            };
            
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync("transcript", content);
            {
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<Transcript>();

                return result;
            }
        }

        public async Task<Transcript> WaitForTranscriptToProcess(Transcript transcript)
        {
            var pollingEndpoint = $"{_BaseUrl}/transcript/{transcript.Id}";

            while (true)
            {
                var pollingResponse = await _httpClient.GetAsync(pollingEndpoint);

                transcript = await pollingResponse.Content.ReadFromJsonAsync<Transcript>();

                switch (transcript.Status)
                {
                    case "queued" or "processing":
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        break;
                    case "completed":
                        return transcript;
                    case "error":
                        throw new Exception($"Transcription failed: {transcript.Error}");
                    default:
                        throw new Exception("This code should not be reachable.");
                }
            }
        }
    }
}
