using ASC.Business.Interfaces;
using ASC.Model.Models;
using ASC.Web.Areas.ServiceRequests.Models;
using ASC.Web.Controllers;
using ASC.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")]
    public class DashboardController : BaseController
    {
        private readonly IServiceRequestOperations _serviceRequestOps;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            IServiceRequestOperations serviceRequestOps,
            UserManager<ApplicationUser> userManager)
        {
            _serviceRequestOps = serviceRequestOps;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToPage("/Account/Login", new { area = "Identity" });

            var isAdmin = User.IsInRole("Admin");
            var isEngineer = User.IsInRole("Engineer");

            List<ServiceRequest> requests;

            if (isAdmin)
                requests = await _serviceRequestOps.GetAllServiceRequestsAsync();
            else if (isEngineer)
                requests = await _serviceRequestOps.GetServiceRequestsByEngineerAsync(currentUser.Email ?? "");
            else
                requests = await _serviceRequestOps.GetServiceRequestsByCustomerAsync(currentUser.Email ?? "");

            var model = new DashboardViewModel
            {
                ServiceRequests = requests,
                IsAdmin = isAdmin,
                IsEngineer = isEngineer,
                TotalNew = requests.Count(r => r.Status == "New"),
                TotalActive = requests.Count(r => r.Status == "Active"),
                TotalInProgress = requests.Count(r => r.Status == "InProgress"),
                TotalCompleted = requests.Count(r => r.Status == "Completed"),
            };

            return View(model);
        }

        // POST: Admin/Engineer cập nhật trạng thái yêu cầu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(string rowKey, string status, string? engineerEmail)
        {
            var request = await _serviceRequestOps.GetServiceRequestByRowKeyAsync(rowKey);
            if (request == null) return NotFound();

            request.Status = status;
            if (!string.IsNullOrEmpty(engineerEmail))
                request.ServiceEngineer = engineerEmail;
            request.UpdatedDate = DateTime.UtcNow;
            request.UpdatedBy = User.Identity?.Name;

            await _serviceRequestOps.UpdateServiceRequestAsync(request);
            TempData["Success"] = "Cập nhật trạng thái thành công!";
            return RedirectToAction("Dashboard");
        }
    }
}
