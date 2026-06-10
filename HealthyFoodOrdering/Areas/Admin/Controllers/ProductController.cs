using HealthyFoodOrdering.Data;
using HealthyFoodOrdering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyFoodOrdering.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(x => x.Category).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            var categories = _context.Categories
                                     .Where(c => c.IsActive)
                                     .Select(c => new { c.Id, c.Name }) // Chỉ lấy ID và Name
                                     .ToList();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Tăng cường bảo mật
        public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
        {
            // 1. Kiểm tra validate cơ bản
            if (product.CategoryId == 0)
                ModelState.AddModelError("CategoryId", "Vui lòng chọn danh mục.");

            if (ModelState.IsValid)
            {
                // 2. Xử lý lưu ảnh
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/products");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = "/uploads/products/" + uniqueFileName;
                }

                // 3. Gán các giá trị mặc định
                product.CreatedDate = DateTime.Now;
                product.IsActive = true;

                // 4. Lưu vào Database
                _context.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi, nạp lại danh sách Category để người dùng chọn lại
            ViewBag.CategoryId = new SelectList(_context.Categories.Where(c => c.IsActive), "Id", "Name", product.CategoryId);
            return View(product);
        }
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile imageFile)
        {
            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null) return NotFound();

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Calories = product.Calories;
            existing.Protein = product.Protein;
            existing.Carbs = product.Carbs;
            existing.Fat = product.Fat;
            existing.CategoryId = product.CategoryId;
            existing.IsActive = product.IsActive;
            if (imageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "products");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(existing.ImageUrl))
                {
                    string oldPath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                existing.ImageUrl = "/uploads/products/" + fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    string filePath = Path.Combine(_env.WebRootPath, product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}