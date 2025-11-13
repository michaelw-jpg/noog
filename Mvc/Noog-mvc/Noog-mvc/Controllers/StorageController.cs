using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.Storage;
using Noog_mvc.Services;

namespace Noog_mvc.Controllers
{
    [Route("Dashboard/ProjectGroup/{projectGroupId:guid}/[controller]/[action]/{id?}")]
    public class StorageController(StorageService service) : ProjectGroupBaseController
    {
        private readonly StorageService _service = service;
        public async Task<IActionResult> Index()
        {
           
            var model = await _service.GetSummaryList();

         
            // Styling - Se Figma 
            return View(model);
        }

        public async Task<IActionResult> Summary(int id)
        {
            
            var model = await _service.GetSummarybyId(id);
            
            // Styling - Se Figma 
            return View(model);
        }
    }
}
