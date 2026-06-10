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

        // GET: /Combo
        public IActionResult Index()
        {
            var combos = _context.NutritionCombos
                .Where(c => c.IsActive)
                .AsNoTracking()          // ✅ read-only → không track entity, nhanh hơn
                .ToList();
            return View(combos);
        }

        // GET: /Combo/Details/{id}
        public IActionResult Details(int id)
        {
            var combo = _context.NutritionCombos
                .Include(c => c.ComboDetails)
                .ThenInclude(cd => cd.Product)
                .AsNoTracking()          // ✅ read-only → không track entity, nhanh hơn
                .FirstOrDefault(c => c.Id == id);

            if (combo == null) return NotFound();

            return View(combo);
        }
    }
}