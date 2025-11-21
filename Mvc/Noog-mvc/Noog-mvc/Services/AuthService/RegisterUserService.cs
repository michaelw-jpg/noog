using Noog_mvc.Helpers;
using Noog_mvc.Models.Register;

namespace Noog_mvc.Services
{
    public class RegisterUserService
    {
        private readonly HttpClient _client;
        public RegisterUserService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("NoogApi");
        }

        public async Task<ServiceResultHelper<string>> RegisterAsync(RegisterUserViewModel model)
        {
            var response = await _client.PostAsJsonAsync("Auth/register", model);
            
            if (response.IsSuccessStatusCode)
            {
                return ServiceResultHelper<string>.SuccessResult(
                    null, "Registration successfull");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return ServiceResultHelper<string>.FailiureResult(
                    "Registration Failed, Please try again",
                    new List<string> { errorContent });
            }

        }
    }
}
