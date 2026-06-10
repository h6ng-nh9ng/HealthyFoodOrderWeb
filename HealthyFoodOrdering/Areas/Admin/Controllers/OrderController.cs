using HealthyFoodOrdering.Data;
using HealthyFoodOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HealthyFoodOrdering.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(x => x.User)
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(x => x.User)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Combo)
                .FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult UpdateStatus(int id)
        {
            var order = _context.Orders
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.NextStatus = OrderStatus.GetNextStatus(order.Status);

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            if (!OrderStatus.CanMoveToNext(order.Status, status))
            {
                return BadRequest("Trang thai don hang khong hop le.");
            }

            order.Status = status;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult ConfirmPayment(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
                return NotFound();

            order.Status = OrderStatus.Pending;

            _context.SaveChanges();

            TempData["Success"] =
                $"Da xac nhan thanh toan don hang #{id}";

            return RedirectToAction(nameof(Index));
        }

    }
}
