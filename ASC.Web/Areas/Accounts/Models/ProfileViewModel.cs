using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ASC.Web.Areas.Accounts.Models
{
    public class ProfileViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên")]
        [Display(Name = "Họ và Tên")]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        // Dùng để xử lý Upload ảnh đại diện từ Form
        public IFormFile ProfilePhoto { get; set; }

        // Dùng để lưu đường dẫn ảnh hiển thị ra giao diện
        public string ProfilePictureUrl { get; set; }
    }
}