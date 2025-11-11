using Noog_mvc.Helpers;
using Noog_mvc.Models.Login;
using System.Security.Claims;

namespace Noog_mvc.Services
{
    public class LoginService
    {
        private readonly HttpClient _client;

        public LoginService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("NoogApi");
        }
        public async Task<LoginResult> LoginAsync(LoginViewModel request)
        {
          
            var response = await _client.PostAsJsonAsync("Auth/login", request);
            if (!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new LoginResult
                    {
                        Status = LoginStatus.InvalidCredentials,
                        ErrorMessage = "Invalid username or password."
                    };
                }

                return new LoginResult
                {
                    Status = LoginStatus.ServerError,
                    ErrorMessage = "An error occurred while processing your request. Please try again later."
                };

            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (loginResponse == null)
            {
                return new LoginResult
                {
                    Status = LoginStatus.ServerError,
                    ErrorMessage = "An error occurred while processing your request. Please try again later."
                };
            }


            var identity = JwtHelper.IdentityCreator(loginResponse!);

            return new LoginResult
            {
                Identity = identity,
                ExpiresAt = loginResponse.ExpiresAt,
                Status = LoginStatus.Success
            };
        }
    }
}
