using HealthyFoodOrdering.Data;
using HealthyFoodOrdering.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthyFoodOrdering.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }

        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Details(int id)
        {
            var category =
                _context.Categories.Find(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var category =
                _context.Categories.Find(id);

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _context.Categories.Update(category);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var category =
                _context.Categories.Find(id);

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category =
                _context.Categories.Find(id);

            _context.Categories.Remove(category);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
