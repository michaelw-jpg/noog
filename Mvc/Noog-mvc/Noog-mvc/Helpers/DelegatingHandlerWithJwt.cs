namespace Noog_mvc.Helpers
{
    public class DelegatingHandlerWithJwt : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DelegatingHandlerWithJwt(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var excludedPaths = new[]
            {
                "/api/auth/login",
                 "/api/auth/register"
            };

            if (!excludedPaths.Any(p => request.RequestUri?.AbsolutePath.StartsWith(p, StringComparison.OrdinalIgnoreCase) == true))
            {
                Console.WriteLine(_httpContextAccessor.HttpContext?.User.Identity.Name);

                var token = _httpContextAccessor.HttpContext?.User.FindFirst("AccessToken")?.Value;
                Console.WriteLine($"THE JWT TOKEN BEING SENT BACK : {token} ");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
