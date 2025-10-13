using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;

namespace Noog_api.Services.IServices
{
    public interface IOpenAiService
    {
        Task<BaseResponseDto<OpenAIResponseDto>> GetChatResponseAsync(string prompt);
    }
}
