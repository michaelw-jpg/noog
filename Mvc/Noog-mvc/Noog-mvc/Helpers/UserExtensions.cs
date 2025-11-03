using System.Security.Claims;

namespace Noog_mvc.Helpers
{
    public static class UserExtensions
    {
        /// <summary>
        /// If you need to get a userId for the current user. <br/>
        /// Call it like this: <c>var userId = User.GetCurrentUserId(); </c> <br/>
        /// In a ViewComponent: <c>var userId = ViewContext.HttpContext.User.GetCurrentUserId();</c>
        /// </summary>
        public static int? GetCurrentUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                return null;

            var idValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idValue, out var userId) ? userId : null;
        }
        /// <summary>
        /// If you need to add authorization headers. <br/>
        /// Call it like this: <c>var token = User.GetAccessToken();</c>
        /// </summary>
        public static string? GetAccessToken(this ClaimsPrincipal user)
        {
            return user?.FindFirstValue("AccessToken");
        }
    }
}
