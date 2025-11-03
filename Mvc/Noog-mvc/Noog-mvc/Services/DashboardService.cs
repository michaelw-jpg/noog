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
        [ValidateAntiForgeryToken]
        public async Task<DashboardViewModel> GetDashboardDataAsync()
            => await _client.GetFromJsonAsync<DashboardViewModel>($"api/dashboard");
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IEnumerable<ProjectGroupDto>> GetUserProjectGroupsAsync()
            => await _client.GetFromJsonAsync<IEnumerable<ProjectGroupDto>>($"api/dashboard/projectgroups");
    }
}
