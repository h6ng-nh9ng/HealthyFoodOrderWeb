using HealthyFoodOrdering.Data;
using HealthyFoodOrdering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HealthyFoodOrdering.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ComboController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ComboController(ApplicationDbContext context) => _context = context;

        // Danh sách Combo cho Admin
        public IActionResult Index() => View(_context.NutritionCombos.ToList());

        // Trang tạo mới
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        // POST: Tạo mới
        [HttpPost]
        public IActionResult Create(NutritionCombo combo, List<int> selectedProducts)
        {
            var selected = _context.Products.Where(p => selectedProducts.Contains(p.Id)).ToList();

            // Tính toán tự động
            combo.TotalCalories = selected.Sum(p => p.Calories);
            combo.TotalProtein = selected.Sum(p => p.Protein);
            combo.TotalPrice = selected.Sum(p => p.Price);
            combo.CreatedDate = DateTime.Now;

            _context.NutritionCombos.Add(combo);
            _context.SaveChanges();

            // Lưu chi tiết Combo
            foreach (var productId in selectedProducts)
            {
                _context.ComboDetails.Add(new ComboDetail { ComboId = combo.Id, ProductId = productId });
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Sửa (Edit)
        public IActionResult Edit(int id)
        {
            var combo = _context.NutritionCombos.Include(c => c.ComboDetails).FirstOrDefault(c => c.Id == id);
            if (combo == null) return NotFound();

            ViewBag.Products = _context.Products.ToList();
            return View(combo);
        }

        // POST: Lưu thay đổi
        [HttpPost]
        public IActionResult Edit(NutritionCombo combo, List<int> selectedProducts)
        {
            var existing = _context.NutritionCombos.Include(c => c.ComboDetails).FirstOrDefault(c => c.Id == combo.Id);
            if (existing == null) return NotFound();

            // Cập nhật thông tin cơ bản
            existing.ComboName = combo.ComboName;
            existing.Description = combo.Description;
            existing.IsActive = combo.IsActive;

            // Xóa chi tiết cũ và thêm mới
            _context.ComboDetails.RemoveRange(existing.ComboDetails);
            foreach (var pid in selectedProducts)
            {
                existing.ComboDetails.Add(new ComboDetail { ComboId = combo.Id, ProductId = pid });
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Xóa (Delete)
        public IActionResult Delete(int id)
        {
            var combo = _context.NutritionCombos.Find(id);
            if (combo == null) return NotFound();
            return View(combo);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var combo = _context.NutritionCombos.Find(id);
            if (combo != null)
            {
                _context.NutritionCombos.Remove(combo);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}