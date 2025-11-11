using Noog_api.Services.IServices;

namespace Noog_api.Middlewares
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;
        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
        {
            var requestedPathFromContext = context.Request.Path.Value?.ToLower();

            var excludedPaths = new List<string>
            {
                "/api/auth/login",
                "/api/auth/register"
            };
            if (excludedPaths.Any(path => requestedPathFromContext.StartsWith(path)))
            {
                await _next(context);
                return;
            }
            
            currentUserService.SetUserId();

            await _next(context);
        }
    }
}
