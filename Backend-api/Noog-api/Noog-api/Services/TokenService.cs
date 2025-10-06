using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Noog_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Noog_api.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration config, UserManager<User> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> CreateToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
                new(JwtRegisteredClaimNames.Email, user.Email ?? "")
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
