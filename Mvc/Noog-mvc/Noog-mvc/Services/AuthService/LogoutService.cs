using Microsoft.AspNetCore.Http.HttpResults;

namespace Noog_mvc.Services.AuthService
{
    public class LogoutService
    {
        private readonly HttpClient _client;

        public LogoutService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("NoogApi");
        }
        public async Task<bool> LogoutAsync()
        {
            var response = await _client.PostAsync("auth/logout", null);

            
            if (response.IsSuccessStatusCode == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
