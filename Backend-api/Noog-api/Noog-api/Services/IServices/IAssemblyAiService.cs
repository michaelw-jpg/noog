using Noog_api.Models.AssemblyAi;

namespace Noog_api.Services.IServices
{
    public interface IAssemblyAiService
    {
        Task<String> UploadFileAsync(string fileUrl);
        Task<Transcript> CreateTranscriptAsync(string audioUrl, string? language = null);
        Task<Transcript> WaitForTranscriptToProcess(Transcript transcript);
        string? GetLanguageCode(string requestedLanguage);
    }
}
