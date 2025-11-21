using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.Storage;
using System.Net.Http.Json;

namespace Noog_mvc.Services
{
    public class StorageService
    {
        private readonly HttpClient _client;

        public StorageService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("NoogApi");
        }

        // Todo [API Call]Not tested with API running
        public async Task<List<StorageSummary>> GetSummaryList(string pgId)
        {
            var response = await _client.GetAsync($"summary/projectgroup/{pgId}");

            response.EnsureSuccessStatusCode();

            var summaries = await response.Content.ReadFromJsonAsync<List<StorageSummary>>();
            return summaries ?? new List<StorageSummary>();
        }
        // Todo [API Call]Not tested with API running
        public async Task<SummaryDisplayDto> GetSummarybyId(int id, string pgId)
        {
            var response = await _client.GetAsync($"summary/{id}/projectgroup/{pgId}");

            response.EnsureSuccessStatusCode();

            var summary = await response.Content.ReadFromJsonAsync<SummaryDisplayDto>();
            return summary ?? new SummaryDisplayDto();
        }
    }
}
