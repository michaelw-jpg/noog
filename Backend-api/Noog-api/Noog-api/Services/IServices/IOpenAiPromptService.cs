using Noog_api.Enums;
using Noog_api.Models.AssemblyAi;
using static Noog_api.Services.OpenAiPromptService;

namespace Noog_api.Services.IServices
{
    public interface IOpenAiPromptService
    {
        Task<string> CreatePromptAsync(PromptType type, string transcript, string language);
    }
}
