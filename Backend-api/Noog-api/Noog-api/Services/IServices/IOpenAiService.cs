using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.Models.AssemblyAi;
using static Noog_api.Services.OpenAiPromptService;

namespace Noog_api.Services.IServices
{
    public interface IOpenAiService
    {
        /// <summary>
        /// Use for testing only the AI service via swagger.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(string prompt);
        /// <summary>
        /// Use internally via OrchestrateService
        /// </summary>
        /// <param name="type"></param>
        /// <param name="transcript"></param>
        /// <param name="language"></param>
        /// <returns>OpenAIResponseDto with Title and Summary</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(PromptType type, string transcript, string language);
    }
}
