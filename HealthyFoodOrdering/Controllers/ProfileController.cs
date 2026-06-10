using HealthyFoodOrdering.Data;
using HealthyFoodOrdering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyFoodOrdering.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser>
            _userManager;

        private readonly ApplicationDbContext
            _context;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user =
                await _userManager.GetUserAsync(User);

            return View(user);
        }

        public async Task<IActionResult> Edit()
        {
            var user =
                await _userManager.GetUserAsync(User);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.Height = model.Height;
            user.Weight = model.Weight;
            user.FitnessGoal = model.FitnessGoal;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật.";
            return View(model);
        }

        public async Task<IActionResult> MyAddresses()
        {
            var user =
                await _userManager.GetUserAsync(User);

            var addresses =
                await _context.Addresses
                .Where(x => x.UserId == user.Id)
                .ToListAsync();

            return View(addresses);
        }

        public IActionResult AddAddress()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(
            Address address)
        {
            var user =
                await _userManager.GetUserAsync(User);

            address.UserId = user.Id;

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyAddresses));
        }

        public async Task<IActionResult>DeleteAddress(int id)
        {
            var address =
                await _context.Addresses.FindAsync(id);

            if (address != null)
            {
                _context.Addresses.Remove(address);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyAddresses));
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>
        ChangePassword(
        string oldPassword,
        string newPassword)
        {
            var user =
                await _userManager.GetUserAsync(User);

            var result =
                await _userManager.ChangePasswordAsync(
                    user,
                    oldPassword,
                    newPassword
                );

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error =
                "Đổi mật khẩu thất bại";

            return View();
        }
    }
}