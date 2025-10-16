using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CallInfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // Set up Dummy user
            // TODO - Replace later with Microsoft Identity User
            var userId = "Plant_Innovation";
            var userName = "Oliver";
            var userImage = $"https://getstream.io/random_svg/?id={userId}&name={userName}";


            // Get getstream.io secrets and keys from config
            var streamApiSecret = _configuration["StreamIo:ApiSecret"];
            var streamApiKey = _configuration["StreamIo:ApiKey"];

            if (string.IsNullOrEmpty(streamApiSecret) || string.IsNullOrEmpty(streamApiKey))
            {
                return StatusCode(500, "Stream Api configuration is missing");
            }

            // Generate stream token
            var streamToken = GenerateStreamToken(userId, streamApiSecret);

            // Generate or retrieve callId for voicechannel (Ex. group-based)
            // TODO - replace with real logic connected to group/project/channel-name
            // TODO - Use Guid for groupId creation
            // TODO - Store in DB
            var groupId = "group-423"; 
            var streamCallId = $"call-{groupId}".ToLowerInvariant();

            // Response object (that Frontend expects)
            // TODO - Change to real Dto
            // TODO - Change to APIBaseResponse
            var response = new
            {
                id = userId,
                name = userName,
                image = userImage,
                token = streamToken,
                callId = streamCallId,
                apiKey = streamApiKey
            };

            return Ok(response);
        }

        private string GenerateStreamToken(string userId, string apiSecret)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("user_id", userId)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: "https://getstream.io",
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
