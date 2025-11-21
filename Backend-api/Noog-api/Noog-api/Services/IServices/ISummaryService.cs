using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.Summary;

namespace Noog_api.Services.IServices
{
    public interface ISummaryService
    {
        Task<BaseResponseDto<List<SummaryResponseDto>>> GetAllSummariesAsync(string pgId);

        Task<BaseResponseDto<SummaryResponseDto>> GetSummaryByIdAsync(int id, string pgId);
        Task<BaseResponseDto<SummaryResponseDto>> CreateSummaryAsync(CreateSummaryRequestDto Request);

        Task<BaseResponseDto<int>> DeleteSummaryAsync(int id);
    }
}
