using Microsoft.AspNetCore.Mvc;
using Noog_api.DTOs.Auth;
using Noog_api.Models;
using Noog_api.Services.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService<ApplicationUser> _userService;

        public AuthController(IAuthService authService, IUserService<ApplicationUser> userService)
        {
            _authService = authService;
            _userService = userService;
        }
        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IResult> AddRolesAsync(string email, string role)
        {
            var user = await _userService.FindByEmailAsync(email);
            if (user is null)
                return Results.NotFound("User not found.");

            var result = await _userService.AddToRoleAsync(user, role);

            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            return Results.Ok("Role added successfully.");
        }

        // POST api/<AuthController>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var res = await _authService.RegisterAsync(dto);
                return Ok(res);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result is null)
                return Unauthorized();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = result.ExpiresAt,
                IsEssential = true
            };

            Response.Cookies.Append("accessToken", result.Token, cookieOptions);
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new {message = "Signed out successfully"});
        }
    }
}
