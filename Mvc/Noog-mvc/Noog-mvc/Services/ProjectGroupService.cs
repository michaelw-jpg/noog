using Newtonsoft.Json;
using Noog_mvc.Models.ProjectGroup;
using Noog_mvc.Models.ProjectGroup.Dtos;
using System.Text;

namespace Noog_mvc.Services
{
    public class ProjectGroupService
    {
        private readonly HttpClient _client;

        public ProjectGroupService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("NoogApi");
        }

        public async Task<ProjectGroupViewModel> GetProjectGroupDataById(Guid id)
        {
            return await _client.GetFromJsonAsync<ProjectGroupViewModel>($"ProjectGroup/{id}");
        }

        public async Task<bool> AddUserToProjectGroup(AddUserToProjectGroup model)
        {
            var response = await _client.PostAsJsonAsync("ProjectGroup/AddUser", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateGroupProject(ProjectGroupCreate model)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("ProjectGroup", jsonContent);

            return response.IsSuccessStatusCode;
        }
    }
}
