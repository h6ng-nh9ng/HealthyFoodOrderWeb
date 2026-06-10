using HealthyFoodOrdering.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HealthyFoodOrdering.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách khách hàng/thành viên đăng ký trên hệ thống
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    }
}