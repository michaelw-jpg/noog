using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Models.AssemblyAi;
using static Noog_api.Services.OpenAiPromptService;

namespace Noog_api.Services.IServices
{
    public interface IOpenAiService
    {
        Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(string prompt);
        //Add Transcript class input
        Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(PromptType type, string transcript, string language);
    }
}
