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
            // Todo [Test if updates] - It might cache too good. And if so, remove the caching and let it 
            if(!_cache.TryGetValue($"group-{projectGroupId}", out TopSectionViewModel groupData))
            {
                var response = await _service.GetProjectGroupDataById(projectGroupId);
                groupData = new TopSectionViewModel
                {
                    GroupId = response.ProjectGroup.GroupId,
                    GroupName = response.ProjectGroup.GroupName,
                    GroupImg = response.ProjectGroup.GroupImg
                };
                _cache.Set($"group-{projectGroupId}", groupData, TimeSpan.FromMinutes(10));
            }

            // Pass along current context
            ViewData["CurrentGroupId"] = projectGroupId;

            return View(groupData);
        }
    }
}
