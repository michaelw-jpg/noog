using Noog_api.Services.IServices;
using StreamChat.Models;
using System.Security.Claims;

namespace Noog_api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (claim != null && Guid.TryParse(claim, out var userId))
                {
                    return userId;
                }

                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

      

    }
}
