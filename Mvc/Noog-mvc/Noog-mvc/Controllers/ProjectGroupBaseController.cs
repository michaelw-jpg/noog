using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Noog_mvc.Controllers
{
    /// <summary>
    /// Serves as a base controller for handling actions related to project groups.
    /// </summary>
    /// <remarks>Takes the guid from route value and places it into a ViewBag for use in views. 
    /// @ViewBag.ProjectGroupId </remarks>
    public abstract class ProjectGroupBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values.TryGetValue("projectGroupId", out var pgId) 
                && Guid.TryParse(pgId?.ToString(), out var projectGroupId))
            {
                ViewBag.ProjectGroupId = projectGroupId;
            }

            /* === If testing and you wish for a projectgroupId
            if (!context.RouteData.Values.TryGetValue("projectGroupId", out var pgId))
            {
                // Inject a mock projectGroupId for local testing
                var testGuid = Guid.Parse("9b1deb4d-5b14-4886-9af0-1f7c3e0f3d00");
                context.RouteData.Values["projectGroupId"] = testGuid;
                ViewBag.ProjectGroupId = testGuid;
            }
            else if (Guid.TryParse(pgId?.ToString(), out var projectGroupId))
            {
                ViewBag.ProjectGroupId = projectGroupId;
            }
             
             
            */

            base.OnActionExecuting(context);
        }
    }
}
