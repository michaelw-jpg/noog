using Noog_api.Models;

namespace Noog_api.Repositories.IRepositories
{
    public interface ISummaryRepository
    {
        Task<List<Summary>> GetAllSummariesAsync(Guid parsedPgId);

        Task<Summary?> GetSummaryByIdAsync(int id, Guid parsedPgId);

        Task<Summary> CreateSummaryAsync(Summary summary);

        Task<Summary?> UpdateSummaryAsync(int id, Summary updatedSummary);

        Task<bool> DeleteSummaryAsync(int id);

    }
}
