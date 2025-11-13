using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Noog_mvc.Models.ProjectGroup;
using Noog_mvc.Services;

namespace Noog_mvc.ViewComponents
{
    public class PGHeaderViewComponent(ProjectGroupService service, IMemoryCache cache) : ViewComponent
    {
        private readonly ProjectGroupService _service = service;
        private readonly IMemoryCache _cache = cache;

        public async Task<IViewComponentResult> InvokeAsync(Guid projectGroupId)
        {
            // Hopeful real code
            if(!_cache.TryGetValue($"group-{projectGroupId}", out TopSectionViewModel groupData))
            {
                groupData = await _service.GetProjectGroupDataById(projectGroupId);
                _cache.Set($"group-{projectGroupId}", groupData, TimeSpan.FromMinutes(10));
            }

            return View(groupData);
        }
    }
}
