using ASC.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ASC.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Bổ sung RoleManager vào constructor
        public DashboardController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserAdministration()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // 1. Hiển thị Form tạo User
        [HttpGet]
        public IActionResult ProvisionUser()
        {
            // Lấy danh sách các quyền (Role) truyền sang View để làm dropdown
            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
            return View();
        }

        // 2. Xử lý Form khi nhấn nút Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProvisionUser(NewUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
                return View(model);
            }

            // Tạo user mới, tự động xác thực email (vì do Admin tạo)
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Gán quyền cho user vừa tạo
                await _userManager.AddToRoleAsync(user, model.Role);

                // Trở về trang danh sách
                return RedirectToAction(nameof(UserAdministration));
            }

            // Bắt lỗi nếu có (ví dụ: mật khẩu quá yếu, email trùng)
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
            return View(model);
        }

        // Xử lý thay đổi trạng thái Active/Deactive của User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(string email, bool isActive)
        {
            // Tìm user theo email
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Cập nhật lại trạng thái
                user.EmailConfirmed = isActive;
                await _userManager.UpdateAsync(user);
            }

            // Chạy xong thì load lại trang danh sách
            return RedirectToAction(nameof(UserAdministration));
        }
    }
}