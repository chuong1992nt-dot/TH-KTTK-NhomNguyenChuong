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

        // ==========================================
        // 1. QUẢN LÝ SERVICE ENGINEERS
        // ==========================================
        [HttpGet]
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
        public async Task<IActionResult> ServiceEngineers(ServiceEngineerViewModel model)
        {
            ModelState.Remove("ServiceEngineers");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Registration.Email, Email = model.Registration.Email, IsActive = true };
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

        // ==========================================
        // 2. QUẢN LÝ CUSTOMERS
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Customers()
        {
            var customers = await _userManager.GetUsersInRoleAsync("Customer");

            var model = new CustomerViewModel
            {
                Customers = customers?.ToList() ?? new List<ApplicationUser>(),
                Registration = new CustomerRegistrationViewModel()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Customers(CustomerViewModel model)
        {
            ModelState.Remove("Customers");

            var user = await _userManager.FindByEmailAsync(model.Registration.Email);
            if (user != null)
            {
                // Cập nhật trạng thái khóa tài khoản
                user.LockoutEnabled = model.Registration.IsActive;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Customers");
                }
            }

            model.Customers = (await _userManager.GetUsersInRoleAsync("Customer")).ToList();
            return View(model);
        }

        // ==========================================
        // 3. QUẢN LÝ PROFILE CÁ NHÂN
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null) return NotFound();

            var model = new ProfileViewModel
            {
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null) return NotFound();

            // Cập nhật thông tin từ ViewModel vào User
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}