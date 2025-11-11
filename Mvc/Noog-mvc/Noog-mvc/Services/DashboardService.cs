using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.Dashboard;
using Noog_mvc.Models.DashboardSidebar;

namespace Noog_mvc.Services
{
    public class DashboardService
    {
        private readonly HttpClient _client;

        public DashboardService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("NoogApi");
        }
        [HttpGet]
        public async Task<DashboardViewModel?> GetDashboardDataAsync()
        {
            var response = await _client.GetAsync("dashboard");

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var message = $"Dashboard API request failed:\n" +
                              $"StatusCode: {(int)response.StatusCode} ({response.StatusCode})\n" +
                              $"Reason: {response.ReasonPhrase}\n" +
                              $"URL: {response.RequestMessage?.RequestUri}\n" +
                              $"Response:\n{content}";

                // Write to console/log output
                Console.WriteLine(message);

                // Throw detailed exception so it appears in Developer Exception Page
                throw new Exception(message);
            }

            return await response.Content.ReadFromJsonAsync<DashboardViewModel>();
        }
        [HttpGet]
        public async Task<IEnumerable<ProjectGroupDto>> GetUserProjectGroupsAsync()
            => await _client.GetFromJsonAsync<IEnumerable<ProjectGroupDto>>($"dashboard/projectgroups");
    }
}
