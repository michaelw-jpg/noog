using Azure.Core;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json.Linq;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.StreamIODtos;
using Noog_api.Models;
using StreamChat.Clients;
using StreamChat.Models;

namespace Noog_api.Services
{
    public class StreamIOService
    {
        private readonly string _StreamIOApiKey;
        private readonly string _StreamIOSecret;
        private readonly StreamClientFactory _StreamClientFactory;
        
        public StreamIOService(IConfiguration configuration, StreamClientFactory streamClientFactory)
        {
            _StreamIOApiKey = configuration["StreamIo:ApiKey"];
            _StreamIOSecret = configuration["StreamIo:ApiSecret"];
            _StreamClientFactory = streamClientFactory;
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
    }
}
