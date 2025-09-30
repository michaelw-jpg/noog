using Noog_api.DTOs;
using Noog_api.DTOs.BaseResponseDtos;

namespace Noog_api.Services.IServices
{
    public interface ISummaryService
    {
        Task<BaseResponseDto<List<SummaryResponseDto>>> GetAllSummariesAsync();

        Task<BaseResponseDto<SummaryResponseDto>> GetSummaryByIdAsync(int id);
        Task<BaseResponseDto<SummaryResponseDto>> CreateSummaryAsync(CreateSummaryDto Request);
        Task<BaseResponseDto<SummaryResponseDto>> UpdateSummaryAsync(int id, PatchSummaryDto updatedSummary);

        Task<BaseResponseDto<int>> DeleteSummaryAsync(int id);
    }
}
