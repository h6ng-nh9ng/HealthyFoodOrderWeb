using HealthyFoodOrdering.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HealthyFoodOrdering.Models;

namespace HealthyFoodOrdering.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult MyOrders()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );

            var orders = _context.Orders
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return View(orders);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var order = _context.Orders
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Combo)
                .FirstOrDefault(x =>
                    x.Id == id &&
                    x.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult ConfirmPayment(int id)
        {
            var order =
                _context.Orders.Find(id);

            if (order == null)
                return NotFound();

            order.Status =
                OrderStatus.Pending;

            _context.SaveChanges();

            TempData["Success"] =
                "Đã xác nhận thanh toán";

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Cancel(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var order = _context.Orders
                .FirstOrDefault(x =>
                    x.Id == id &&
                    x.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Status != OrderStatus.Pending &&
                order.Status != OrderStatus.Confirmed)
            {
                TempData["Error"] =
                    "Đơn hàng không thể hủy.";

                return RedirectToAction(nameof(MyOrders));
            }

            order.Status = OrderStatus.Cancelled;

            _context.SaveChanges();

            TempData["Success"] =
                "Đã hủy đơn hàng.";

            return RedirectToAction(nameof(MyOrders));
        }
    }
}