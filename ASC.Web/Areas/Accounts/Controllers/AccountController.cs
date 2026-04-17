using ASC.Web.Areas.Accounts.Models;
using ASC.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ASC.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Action hiển thị danh sách Service Engineer
        [HttpGet]
        public async Task<IActionResult> ServiceEngineers()
        {
            // 1. Lấy danh sách tất cả người dùng có Role là "Engineer"
            var engineers = await _userManager.GetUsersInRoleAsync("Engineer");

            // 2. Đổ dữ liệu vào ViewModel để mang sang hiển thị ở View
            var model = new ServiceEngineerViewModel
            {
                ServiceEngineers = engineers.ToList(),
                Registration = new ServiceEngineerRegistrationViewModel { IsActive = true }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ServiceEngineers(ServiceEngineerViewModel model)
        {
            ModelState.Remove("ServiceEngineers");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Registration.Email, Email = model.Registration.Email };
                var result = await _userManager.CreateAsync(user, model.Registration.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Engineer");

                    return RedirectToAction("ServiceEngineers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.ServiceEngineers = (await _userManager.GetUsersInRoleAsync("Engineer")).ToList();
            return View(model);
        }

        // GET: Hiển thị danh sách khách hàng
        [HttpGet]
        public async Task<IActionResult> Customers()
        {
            // Lấy tất cả user có Role là "Customer"
            var customers = await _userManager.GetUsersInRoleAsync("Customer");

            var model = new CustomerViewModel
            {
                Customers = customers?.ToList() ?? new List<ApplicationUser>(),
                Registration = new CustomerRegistrationViewModel()
            };

            return View(model);
        }

        // POST: Thực hiện Khóa hoặc Kích hoạt khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Customers(CustomerViewModel model)
        {
            // Loại bỏ kiểm tra danh sách
            ModelState.Remove("Customers");

            var user = await _userManager.FindByEmailAsync(model.Registration.Email);
            if (user != null)
            {
                // Cập nhật trạng thái: True (Khóa) hoặc False (Kích hoạt)
                user.LockoutEnabled = model.Registration.IsActive;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Customers");
                }
            }

            // Nếu có lỗi, lấy lại danh sách và hiển thị
            model.Customers = (await _userManager.GetUsersInRoleAsync("Customer")).ToList();
            return View(model);
        }

        // GET: Hiển thị trang Profile cá nhân
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            var model = new ProfileViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                // Giả sử bạn đã có cột Name trong ApplicationUser
                // Name = user.Name 
            };
            return View(model);
        }

        // POST: Xử lý cập nhật thông tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                user.PhoneNumber = model.PhoneNumber;
                // user.Name = model.Name; // Cập nhật tên nếu có

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Thông báo thành công (có thể dùng TempData)
                    return RedirectToAction("Profile");
                }
            }
            return View(model);
        }
    }
}