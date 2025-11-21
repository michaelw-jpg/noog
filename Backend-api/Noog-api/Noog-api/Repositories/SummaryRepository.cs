using Microsoft.EntityFrameworkCore;
using Noog_api.Data;
using Noog_api.Models;
using Noog_api.Repositories.IRepositories;

namespace Noog_api.Repositories
{
    
    public class SummaryRepository(NoogDbContext context) : ISummaryRepository
    {
        private readonly NoogDbContext _context = context;
        

        public async Task<List<Summary>> GetAllSummariesAsync(Guid pgId)
        {
            return await _context.Summaries
                .Where(s => s.GroupStorage.ProjectGroupId == pgId)
                .ToListAsync();
        }

        public async Task<Summary?> GetSummaryByIdAsync(int id, Guid pgId)
        {
            return await _context.Summaries
                .Where(s => s.GroupStorage.ProjectGroupId == pgId
                    && s.SummaryId == id)   
                .FirstOrDefaultAsync();

        }

        public async Task<Summary> CreateSummaryAsync(Summary summary)
        {
            _context.Summaries.Add(summary);
            await _context.SaveChangesAsync();
            return summary;
        }

        public async Task<Summary?> UpdateSummaryAsync(int id, Summary updatedSummary)
        {
            var existingSummary = await _context.Summaries.FindAsync(id);
            if (existingSummary == null)
            {
                return null;
            }
            existingSummary.Title = updatedSummary.Title;
            existingSummary.Content = updatedSummary.Content;
            await _context.SaveChangesAsync();
            return existingSummary;

        }

        public async Task<bool> DeleteSummaryAsync(int id)
        {
            var summary = await _context.Summaries.FindAsync(id);
            if (summary == null)
            {
                return false;
            }
            _context.Summaries.Remove(summary);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
