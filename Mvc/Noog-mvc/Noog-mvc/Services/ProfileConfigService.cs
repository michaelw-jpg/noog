using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.User;

namespace Noog_mvc.Services
{
    public class ProfileConfigService
    {
        private readonly HttpClient _client;
        public ProfileConfigService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("NoogApi");
        }

        //TODO: Implement GetUserProfileByLogin method to fetch user profile based on login
        [HttpGet]
        [ValidateAntiForgeryToken]  
        public async Task<UserViewModel?> GetUserProfileByLogin()
            => await _client.GetFromJsonAsync<UserViewModel>($"api/user/"); 

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<bool> UpdateUserProfileAsync(UserPatchDto userPatchDto)
            => await _client.PutAsJsonAsync($"api/profile/config", userPatchDto)
                .ContinueWith(task => task.Result.IsSuccessStatusCode);

    }
}
