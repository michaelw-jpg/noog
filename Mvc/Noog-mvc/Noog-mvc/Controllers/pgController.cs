using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Noog_mvc.Controllers
{
    public class pgController : Controller
    {
        // GET: pgController
        public ActionResult Index()
        {
            return View();
        }

        // GET: pgController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: pgController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: pgController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: pgController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: pgController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: pgController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: pgController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
