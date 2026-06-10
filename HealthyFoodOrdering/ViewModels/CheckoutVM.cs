using System.ComponentModel.DataAnnotations;

namespace HealthyFoodOrdering.ViewModels
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên người nhận")]
        [StringLength(50,
            MinimumLength = 2,
            ErrorMessage = "Họ và tên phải từ 2 đến 50 ký tự")]
        [RegularExpression(
            @"^[\p{L}\s]+$",
            ErrorMessage = "Họ và tên không được chứa số hoặc ký tự đặc biệt")]
        public string ReceiverName { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(
            @"^(0[3|5|7|8|9])+([0-9]{8})$",
            ErrorMessage = "Số điện thoại không hợp lệ"
        )]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [MinLength(10, ErrorMessage = "Địa chỉ phải có ít nhất 10 ký tự")]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng chọn hình thức thanh toán")]
        public string PaymentMethod { get; set; } = "COD";
    }
}