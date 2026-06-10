using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using HealthyFoodOrdering.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;

namespace HealthyFoodOrdering.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // 1. GET: Hiển thị trang Login/Register chung
        [HttpGet]
        public IActionResult Login()
        {
            // Nếu người dùng đã đăng nhập rồi thì không cần vào trang Login nữa, đá về Trang chủ luôn
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // 2. POST: Xử lý khi ấn nút ĐĂNG KÝ
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["RegisterError"] = "Vui lòng kiểm tra lại thông tin.";
                return View("Login", new LoginVM());
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Account");
            }
        
            foreach (var error in result.Errors)
            {
                ViewData["RegisterError"] = result.Errors.FirstOrDefault()?.Description;
            }

            return View("Login", new LoginVM());
        }

        // 3. POST: Xử lý khi ấn nút ĐĂNG NHẬP
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewData["LoginError"] = "Tài khoản hoặc mật khẩu không chính xác.";
            }
            return View(model);
        }

        // 4. GET: Xử lý Đăng xuất
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string fullName, string email, DateTime? dateOfBirth, decimal height, decimal weight, string? fitnessGoal, string currentPassword, string newPassword, IFormFile? avatarFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            user.FullName = fullName;
            user.DateOfBirth = dateOfBirth;
            user.FitnessGoal = fitnessGoal ?? "";
            user.Height = height;
            user.Weight = weight;

            // 1. XỬ LÝ UPLOAD AVATAR
            if (avatarFile != null && avatarFile.Length > 0)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(avatarFile.FileName);
                string filePath = Path.Combine(uploadDir, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(fileStream);
                }

                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    string oldFilePath = Path.Combine(uploadDir, user.Avatar);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                user.Avatar = uniqueFileName;
            }

            // 2. CẬP NHẬT THÔNG TIN KHÁC
            if (!string.IsNullOrEmpty(fullName) && user.FullName != fullName) user.FullName = fullName;
            if (!string.IsNullOrEmpty(email) && user.Email != email)
            {
                user.Email = email;
                user.UserName = email;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                TempData["ErrorMessage"] = "Cập nhật thông tin thất bại!";
                return RedirectToAction("Profile");
            }

            // 3. XỬ LÝ ĐỔI MẬT KHẨU
            if (!string.IsNullOrEmpty(currentPassword) && !string.IsNullOrEmpty(newPassword))
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!changePasswordResult.Succeeded)
                {
                    TempData["ErrorMessage"] = "Mật khẩu cũ không đúng hoặc mật khẩu mới chưa đủ mạnh!";
                    return RedirectToAction("Profile");
                }
                await _signInManager.RefreshSignInAsync(user);
            }

            TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
            return RedirectToAction("Profile");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordVM());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản với email này.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Mô phỏng gửi email thành công. Bạn có thể đặt lại mật khẩu ngay.";

            return RedirectToAction(
                "ResetPassword",
                new { email = model.Email }
            );
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordVM
            {
                Email = email ?? ""
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản với email này.");
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                model.NewPassword
            );

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
