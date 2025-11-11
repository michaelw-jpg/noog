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
        public Guid UserId { get; set; }

        public void SetUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(claim != null)
            {
                UserId = Guid.Parse(claim);
            }
            else
            {
                throw new Exception("Invalid tenant ID in token.");
            }

        }

    }
}
