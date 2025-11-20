using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Noog_mvc.Helpers;
using Noog_mvc.Models.DashboardSidebar;
using Noog_mvc.Services;

namespace Noog_mvc.ViewComponents
{
    public class DashboardSidebarViewComponent : ViewComponent
    {
        private readonly DashboardService _service;
        private readonly IMemoryCache _cache;

        public DashboardSidebarViewComponent(DashboardService service, IMemoryCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Check route data for current context
            var currentGroupId = ViewContext.RouteData.Values["projectGroupId"]?.ToString();

            if (!_cache.TryGetValue("sidebar-projects", out IEnumerable<ProjectGroupDto> groups))
            {
                groups = await _service.GetUserProjectGroupsAsync();
                //_cache.Set("sidebar-projects", groups, TimeSpan.FromMinutes(10));
            }

            // Force the ViewComponent to re-render even when cache hit
            ViewData["CurrentGroupId"] = currentGroupId;

            return View(groups);
        }
    }
}
