using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.Summary;
using static Noog_api.Services.OpenAiPromptService;

namespace Noog_api.Services.IServices
{
    public interface IOrchestrateService
    {
        Task<BaseResponseDto<SummaryResponseDto>> OrchestrateAsync(string audioUrl, Guid projectGroupId, 
            string? language = "swedish", PromptType? type = PromptType.MeetingSummary);
    }
}
