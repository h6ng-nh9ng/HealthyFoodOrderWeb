using System.ComponentModel.DataAnnotations;

namespace HealthyFoodOrdering.Models
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Vui long nhap mat khau hien tai")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = "";

        [Required(ErrorMessage = "Vui long nhap mat khau moi")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mat khau moi toi thieu 6 ky tu")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = "Vui long xac nhan mat khau moi")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Xac nhan mat khau khong khop")]
        public string ConfirmPassword { get; set; } = "";
    }

    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Vui long nhap email")]
        [EmailAddress(ErrorMessage = "Email khong hop le")]
        public string Email { get; set; } = "";
    }

    public class ResetPasswordVM
    {
        [Required(ErrorMessage = "Vui long nhap email")]
        [EmailAddress(ErrorMessage = "Email khong hop le")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Vui long nhap mat khau moi")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mat khau moi toi thieu 6 ky tu")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = "Vui long xac nhan mat khau moi")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Xac nhan mat khau khong khop")]
        public string ConfirmPassword { get; set; } = "";
    }
}
