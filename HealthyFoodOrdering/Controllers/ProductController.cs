using HealthyFoodOrdering.Data;
using HealthyFoodOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HealthyFoodOrdering.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? maxCalories)
        {
            // Lấy danh sách các món ăn đang hoạt động
            var query = _context.Products.Where(p => p.IsActive);

            // Nếu người dùng có nhập số Calo tối đa, thực hiện lọc dữ liệu
            if (maxCalories.HasValue)
            {
                query = query.Where(p => p.Calories <= maxCalories.Value);
                ViewBag.MaxCalories = maxCalories; // Giữ lại giá trị hiển thị lên ô nhập
            }

            var products = query.ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}