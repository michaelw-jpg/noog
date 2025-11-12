using Microsoft.AspNetCore.Mvc;
using Noog_mvc.Models.Storage;

namespace Noog_mvc.Controllers
{
    public class StorageController : Controller
    {
        public IActionResult Index()
        {
            var model = new StorageViewModel();
            model.ProjectGroup = new Models.ProjectGroup.TopSectionViewModel { GroupImg = "", GroupName = "NoogTest" };
            model.SummaryList = new List<StorageSummary>
            {
                new StorageSummary { Id = 1, Title = "Summary 1", Date = DateTime.Now},
                new StorageSummary { Id = 2, Title = "Summary 2" , Date = DateTime.Today},
                new StorageSummary { Id = 3, Title = "Summary 2" , Date = DateTime.Today},
                new StorageSummary { Id = 4, Title = "Summary 2" , Date = DateTime.Today},
                new StorageSummary { Id = 5, Title = "Summary 2" , Date = DateTime.Today},
                new StorageSummary { Id = 6, Title = "Summary 2" , Date = DateTime.Today},
                new StorageSummary { Id = 7, Title = "Summary 2" , Date = DateTime.Today},
                new StorageSummary { Id = 8, Title = "Summary 2" , Date = DateTime.Today}
            };
            // Styling - Se Figma 
            return View(model);
        }

        public IActionResult Summary(int id)
        {
            // Styling - Se Figma 
            return View();
        }
    }
}
