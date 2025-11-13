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

            base.OnActionExecuting(context);
        }
    }
}
