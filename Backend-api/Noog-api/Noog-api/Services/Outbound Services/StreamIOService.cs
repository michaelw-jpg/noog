using Azure.Core;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json.Linq;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.StreamIODtos;
using Noog_api.Models;
using StreamChat.Clients;
using StreamChat.Models;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace Noog_api.Services
{
    public class StreamIOService
    {
        private readonly string _StreamIOApiKey;
        private readonly string _StreamIOSecret;
        private readonly StreamClientFactory _StreamClientFactory;
        private readonly string _ExpressApi;
        private readonly HttpClient _http;

        public StreamIOService(IConfiguration configuration, StreamClientFactory streamClientFactory, HttpClient http)
        {
            _StreamIOApiKey = configuration["StreamIo:ApiKey"];
            _StreamIOSecret = configuration["StreamIo:ApiSecret"];
            _StreamClientFactory = streamClientFactory;
            _ExpressApi = configuration["ExpressApi:ApiUrl"];
            _http = http;

        }
        //TODO: Maybe change name later 
        public async Task<StreamIODTO> CreateStreamIOUser(ApplicationUser user)
        {
            var streamIODto = new StreamIODTO
            {
                Id = user.Id.ToString(), 
                Name = $"{user.FirstName} {user.LastName}".Trim(),
                UserImage = user.ImgProfile ?? "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcStltpfa69E9JTQOf5ZcyLGR8meBbxMFJxM0w&s"
            };

            //Fetch user client from StreamIO
            var userClient = _StreamClientFactory.GetUserClient();

            // request a new user obj 
            var request = new StreamIOUserRequest
            {
                Id = streamIODto.Id,
                Name = streamIODto.Name,
                Role = "user", 
                Image = streamIODto.UserImage,
            };

            //Create User based on request
            await userClient.UpsertAsync(request);

            //Create Token for user
            var token = userClient.CreateToken(request.Id, DateTimeOffset.UtcNow.AddDays(1));

            streamIODto.Token = token;

            return streamIODto;
        }

        //Test Controller method to create a callId in Express
        public async Task<bool> CreateStreamIOCallId(Guid id)
        {
            // Korrekt URL enligt Express-routes
            var url = $"{_ExpressApi}/StreamIOVideoCall/calls/{id}";

            var payload = new
            {
                callType = "default"
            };

            try
            {
                var response = await _http.PostAsJsonAsync(url, payload);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Express error: " + error);
                    return false;
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Express response: " + json);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling Express: " + ex.Message);
                return false;
            }
        }
    }
}
