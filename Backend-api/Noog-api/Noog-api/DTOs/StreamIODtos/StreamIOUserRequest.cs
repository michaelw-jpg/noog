using Newtonsoft.Json;
using StreamChat.Models;

namespace Noog_api.DTOs.StreamIODtos
{
    //Inheritance Stream IO's User obj props 
    public class StreamIOUserRequest : UserRequest
    {
        //Add custom props for Stream User obj
        [JsonProperty]
        public string Image { get; set; }
    }
}
