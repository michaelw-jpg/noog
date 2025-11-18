namespace Noog_mvc.Services
{
    public class CallService
    {
        private readonly HttpClient _client;
        private readonly ProjectGroupService _groupService;

        public CallService(IHttpClientFactory factory, ProjectGroupService groupService)
        {
            _client = factory.CreateClient("BaseUrl");
            _groupService = groupService;

        }

        public async Task<string> StartCallAsync(Guid ProjectGroupId)
        {
            var result = await _groupService.GetProjectGroupDataById(ProjectGroupId);
            var callid = result.MeetingRoom.CallId;

            var reactResponse = await _client.PostAsJsonAsync($"streamio/calls/{callid}/join", callid);
            if (!reactResponse.IsSuccessStatusCode)
            {
                throw new Exception("Failed to start call.");
            }
            var joinLink = await reactResponse.Content.ReadFromJsonAsync<string>();
            //open newn window with joinlink
            return joinLink;
        }
    }
}
