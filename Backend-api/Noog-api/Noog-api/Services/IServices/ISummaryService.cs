using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.Summary;

namespace Noog_api.Services.IServices
{
    public interface ISummaryService
    {
        Task<BaseResponseDto<List<SummaryResponseDto>>> GetAllSummariesAsync();

        Task<BaseResponseDto<SummaryResponseDto>> GetSummaryByIdAsync(int id);
        Task<BaseResponseDto<SummaryResponseDto>> CreateSummaryAsync(CreateSummaryRequestDto Request);
        Task<BaseResponseDto<SummaryResponseDto>> UpdateSummaryAsync(int id, PatchSummaryDto updatedSummary);

        Task<BaseResponseDto<int>> DeleteSummaryAsync(int id);
    }
}
