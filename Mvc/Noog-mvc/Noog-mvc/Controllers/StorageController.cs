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
            // TODO [API call] - Change to this once the endpoint is checked in Backend
            // var model = await _service.GetSummaryList();

            var model = new List<StorageSummary>
            {
                new StorageSummary { Id = 1, Title = "Summary 1", Date = DateTime.Now},
                new StorageSummary { Id = 2, Title = "Summary 2" , Date = DateTime.Today},
                new StorageSummary { Id = 3, Title = "Summary 3" , Date = DateTime.Today},
                new StorageSummary { Id = 4, Title = "Summary 4" , Date = DateTime.Today},
                new StorageSummary { Id = 5, Title = "Summary 5" , Date = DateTime.Today},
                new StorageSummary { Id = 6, Title = "Summary 6" , Date = DateTime.Today},
                new StorageSummary { Id = 7, Title = "Summary 7" , Date = DateTime.Today},
                new StorageSummary { Id = 8, Title = "Summary 8" , Date = DateTime.Today}
            };
            // Styling - Se Figma 
            return View(model);
        }

        public async Task<IActionResult> Summary(int id)
        {
            // TODO [API call] - Change to this once the endpoint is checked in Backend
            // var modell = await _service.GetSummarybyId(id);
            id = 1;
            var model = new SummaryDisplayDto
            {
                Id = id,
                Title = "Summary 1",
                Content =   "The team discussed project timelines, identified delays in the design phase, " +
                            "and assigned tasks to accelerate development. Marketing will prepare an updated " +
                            "launch plan. Next meeting scheduled for Monday.",
                CreatedAt = DateTime.Now
            };
            // Styling - Se Figma 
            return View(model);
        }
    }
}
