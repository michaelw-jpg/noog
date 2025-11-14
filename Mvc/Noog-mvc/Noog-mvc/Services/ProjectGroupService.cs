using Noog_mvc.Models.ProjectGroup;

namespace Noog_mvc.Services
{
    public class ProjectGroupService
    {
        private readonly HttpClient _client;

        public ProjectGroupService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("NoogApi");
        }

        public async Task<TopSectionViewModel> GetProjectGroupDataById(Guid id)
        {
            return await _client.GetFromJsonAsync<TopSectionViewModel>($"ProjectGroup/{id}");
        }
    }
}
