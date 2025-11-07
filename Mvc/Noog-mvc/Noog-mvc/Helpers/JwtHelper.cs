using Microsoft.AspNetCore.Authentication.Cookies;
using Noog_mvc.Models.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Noog_mvc.Helpers
{
    public class JwtHelper
    {
        public static ClaimsIdentity IdentityCreator(LoginResponseDto loginResponse)
        {
            

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginResponse!.UserName),
                new Claim("AccessToken", loginResponse.Token)
            };

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(loginResponse.Token);
            var roleClaims = jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => new Claim(ClaimTypes.Role, c.Value));

            claims.AddRange(roleClaims);

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return identity;
        }

    }
}
