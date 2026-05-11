using ASC.Web.Areas.Accounts.Models;
using ASC.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ══ SERVICE ENGINEERS ══

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ServiceEngineers()
        {
            var engineers = await _userManager.GetUsersInRoleAsync("Engineer");
            var model = new ServiceEngineerViewModel
            {
                ServiceEngineers = engineers.ToList(),
                Registration = new ServiceEngineerRegistrationViewModel { IsActive = true }
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ServiceEngineers(ServiceEngineerViewModel model)
        {
            ModelState.Remove("ServiceEngineers");
            if (!ModelState.IsValid)
            {
                model.ServiceEngineers = (await _userManager.GetUsersInRoleAsync("Engineer")).ToList();
                return View(model);
            }

            var existing = await _userManager.FindByEmailAsync(model.Registration.Email);
            if (existing != null)
            {
                ModelState.AddModelError("", "Email này đã được đăng ký trong hệ thống.");
                model.ServiceEngineers = (await _userManager.GetUsersInRoleAsync("Engineer")).ToList();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Registration.Email,
                Email = model.Registration.Email,
                EmailConfirmed = true,
                IsActive = model.Registration.IsActive,
                // BẮT BUỘC bật LockoutEnabled để chức năng khóa hoạt động
                LockoutEnabled = true
            };

            var result = await _userManager.CreateAsync(user, model.Registration.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Engineer");
                TempData["Success"] = $"Đã tạo tài khoản kỹ sư: {user.Email}";
                return RedirectToAction("ServiceEngineers");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            model.ServiceEngineers = (await _userManager.GetUsersInRoleAsync("Engineer")).ToList();
            return View(model);
        }

        // POST: Khóa / Mở khóa kỹ sư
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleEngineerStatus(string email, bool activate)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.IsActive = activate;

                // BẮT BUỘC: bật LockoutEnabled trước khi set LockoutEnd
                await _userManager.SetLockoutEnabledAsync(user, true);

                if (!activate)
                {
                    // Khóa: đặt LockoutEnd xa trong tương lai
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                }
                else
                {
                    // Mở khóa: đặt LockoutEnd về null + reset AccessFailedCount
                    await _userManager.SetLockoutEndDateAsync(user, null);
                    await _userManager.ResetAccessFailedCountAsync(user);
                }

                await _userManager.UpdateAsync(user);

                TempData["Success"] = activate
                    ? $"✅ Đã mở khóa tài khoản: {email}"
                    : $"🔒 Đã khóa tài khoản: {email}";
            }
            else
            {
                TempData["Error"] = $"Không tìm thấy tài khoản: {email}";
            }

            return RedirectToAction("ServiceEngineers");
        }

        // ══ CUSTOMERS ══

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Customers()
        {
            var customers = await _userManager.GetUsersInRoleAsync("Customer");
            var model = new CustomerViewModel
            {
                Customers = customers.ToList(),
                Registration = new CustomerRegistrationViewModel()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Customers(CustomerViewModel model)
        {
            ModelState.Remove("Customers");

            if (!string.IsNullOrEmpty(model.Registration?.Email))
            {
                var user = await _userManager.FindByEmailAsync(model.Registration.Email);
                if (user != null)
                {
                    user.IsActive = model.Registration.IsActive;

                    await _userManager.SetLockoutEnabledAsync(user, true);

                    if (!model.Registration.IsActive)
                    {
                        // Khóa
                        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                    }
                    else
                    {
                        // Mở khóa
                        await _userManager.SetLockoutEndDateAsync(user, null);
                        await _userManager.ResetAccessFailedCountAsync(user);
                    }

                    await _userManager.UpdateAsync(user);

                    TempData["Success"] = model.Registration.IsActive
                        ? $"✅ Đã mở khóa tài khoản: {user.Email}"
                        : $"🔒 Đã khóa tài khoản: {user.Email}";
                }
                else
                {
                    TempData["Error"] = $"Không tìm thấy tài khoản: {model.Registration.Email}";
                }
            }

            return RedirectToAction("Customers");
        }

        // ══ PROFILE ══

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity!.Name!);
            if (user == null) return NotFound();

            return View(new ProfileViewModel
            {
                Email = user.Email!,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(User.Identity!.Name!);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Cập nhật hồ sơ thành công!";
                return RedirectToAction("Profile");
            }

            foreach (var e in result.Errors)
                ModelState.AddModelError("", e.Description);

            return View(model);
        }
    }
}
