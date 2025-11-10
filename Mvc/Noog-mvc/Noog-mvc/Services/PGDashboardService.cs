using Noog_mvc.Models.ProjectGroup;

namespace Noog_mvc.Services
{
    public class PGDashboardService
    {
        private readonly HttpClient _client;

        public PGDashboardService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("NoogApi");
        }

        public async Task<TopSectionViewModel> GetProjectGroupDataById(Guid id)
            => await _client.GetFromJsonAsync<TopSectionViewModel>($"api/ProjectGroup/{id}");
    }
}
