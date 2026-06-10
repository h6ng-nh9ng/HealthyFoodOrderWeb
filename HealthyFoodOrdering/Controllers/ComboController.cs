using HealthyFoodOrdering.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HealthyFoodOrdering.Controllers
{
    public class ComboController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ComboController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var combos = _context.NutritionCombos.Where(c => c.IsActive).ToList();
            return View(combos);
        }

        public IActionResult Details(int id)
        {
            var combo = _context.NutritionCombos
                .Include(c => c.ComboDetails).ThenInclude(cd => cd.Product)
                .FirstOrDefault(c => c.Id == id);
            return combo == null ? NotFound() : View(combo);
        }
    }
}