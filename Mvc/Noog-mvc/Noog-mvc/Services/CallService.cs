using Noog_mvc.Models.ProjectGroup.Dtos;

namespace Noog_mvc.Services
{
    public class CallService
    {
        private readonly HttpClient _client;
        private readonly ProjectGroupService _groupService;

        public CallService(IHttpClientFactory factory, ProjectGroupService groupService)
        {
            _client = factory.CreateClient("NoogApi");
            _groupService = groupService;

        }

        public async Task<string> StartCallAsync(Guid projectGroupId)
        {
           
            

            var response = await _client.PostAsJsonAsync($"streamio/calls/{projectGroupId}/join", projectGroupId);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to start call.");
            }

            var content = await response.Content.ReadFromJsonAsync<JoinCallDto>();
            //open newn window with joinlink
            return content.joinUrl;
        }
    }
}
