using HealthyFoodOrdering.Data;
using HealthyFoodOrdering.Models;
using HealthyFoodOrdering.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace HealthyFoodOrdering.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(CheckoutVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var session = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(session))
            {
                return RedirectToAction(
                    "Index",
                    "Cart"
                );
            }

            var cart =
                JsonSerializer.Deserialize<List<CartItem>>(session)
                ?? new List<CartItem>();

            decimal total =
                cart.Sum(x => x.Price * x.Quantity);

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var order = new Order
            {
                UserId =
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier
                    ) ?? "",

                ReceiverName = model.ReceiverName,

                Phone = model.Phone,

                Address = model.Address,

                PaymentMethod = model.PaymentMethod,

                TotalAmount = total,

                Status =
                    model.PaymentMethod == "QR"
                    ? OrderStatus.WaitingPayment
                    : OrderStatus.Pending,

                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);

            _context.SaveChanges();

            foreach (var item in cart)
            {
                _context.OrderDetails.Add(
                    new OrderDetail
                    {
                        OrderId = order.Id,

                        ProductId =
                            item.Type == "Product"
                            ? item.ProductId
                            : null,

                        ComboId =
                            item.Type == "Combo"
                            ? item.ComboId
                            : null,

                        Quantity = item.Quantity,

                        Price = item.Price
                    });
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            if (model.PaymentMethod == "QR")
            {
                return RedirectToAction("QRPayment", new { id = order.Id });
            }

            return RedirectToAction("Success", new { id = order.Id });
        }

        public IActionResult QRPayment(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
                return RedirectToAction("Index", "Home");

            ViewBag.BankCode =
                _config["VietQR:BankCode"];

            ViewBag.AccountNumber =
                _config["VietQR:AccountNumber"];

            ViewBag.AccountName =
                _config["VietQR:AccountName"];

            ViewBag.BankName =
                _config["VietQR:BankName"];

            return View(order);
        }

        public IActionResult Success(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Combo)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                // Nếu không tìm thấy id đơn hàng cụ thể, điều hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }
        [HttpPost]
        public IActionResult ConfirmTransfer(int id)
        {
            var order =
                _context.Orders.Find(id);

            if (order == null)
                return NotFound();

            order.Status =
                OrderStatus.WaitingConfirm;

            _context.SaveChanges();

            return RedirectToAction(
                "Success",
                new { id }
            );
        }
    }
}
