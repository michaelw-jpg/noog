using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Noog_api.DTOs.BaseResponseDtos;
using Noog_api.DTOs.StreamIODtos;
using Noog_api.Helpers;
using Noog_api.Models;
using Noog_api.Services;
using StreamChat.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamIOController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly StreamIOService _streamIOService;

        public StreamIOController(IConfiguration configuration, StreamIOService streamIOService)
        {
            _configuration = configuration;
            _streamIOService = streamIOService;
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
                return StatusCode(500, "Stream Api configuration is missing. Please set StreamIo:ApiSecret and StreamIo:ApiKey in your app settings.");
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

        [HttpPost]
        public async Task<ActionResult<StreamIOUserResponseDto>> CreateStreamIOUser([FromBody] ApplicationUser user)
        {
            if (user == null)
                return BadRequest("User payload is missing.");

            // call service to create the user in Stream
            var response = await _streamIOService.CreateStreamIOUser(user);

            return ApiResponseHelper.ToActionResult<StreamIOUserResponseDto>(response);
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
                issuer: "https://getstream.io", // Optional
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("calls/{callId}/join")]
        public async Task<ActionResult<JoinCallDto>> JoinCall(Guid callId)
        {
            var userId = _currentUserService.UserId;

            var user = await _userService.FindByIdAsync(userId);

            if (Guid.Empty==callId)
                return BadRequest("callId is required.");

            if (user == null)
                return BadRequest("User payload is missing.");

            var streamApiSecret = _configuration["StreamIo:ApiSecret"];

            string guidAsString = callId.ToString();

            var streamToken = GenerateStreamToken(guidAsString, streamApiSecret);

            // 1. Get or create StreamIO user
            var streamUser = await _streamIOService.CreateStreamIOUser(user);

            // 3. Return a DTO to frontend can use to join the call
            var response = new JoinCallDto
            {
                callId = guidAsString,
                userName = streamUser.Name,
                image = streamUser.UserImage,
                token = streamUser.Token,
                apikey = _configuration["StreamIo:ApiKey"]
            };
            return Ok(response);
        }

        [HttpPost("create-callid/{callId}")]
        public async Task<IActionResult> CreateCallId(Guid callId)
        {
            if (callId == Guid.Empty)
                return BadRequest("callId required");

            // Detta kommer anropa ditt Express.js API via HttpClient
            var success = await _streamIOService.CreateStreamIOCallId(callId);

            return Ok(new
            {
                callId,
                success
            });
        }
    }
}
