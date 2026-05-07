using ASC.Business.Interfaces;
using ASC.Model.Models;
using ASC.Web.Areas.ServiceRequests.Models;
using ASC.Web.Controllers;
using ASC.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var isAdmin = User.IsInRole("Admin");
            var isEngineer = User.IsInRole("Engineer");

            List<ServiceRequest> requests;

            if (isAdmin)
                requests = await _serviceRequestOps.GetAllServiceRequestsAsync();
            else if (isEngineer)
                requests = await _serviceRequestOps.GetServiceRequestsByEngineerAsync(currentUser?.Email ?? "");
            else
                requests = await _serviceRequestOps.GetServiceRequestsByCustomerAsync(currentUser?.Email ?? "");

            var model = new DashboardViewModel
            {
                ServiceRequests = requests,
                IsAdmin = isAdmin,
                IsEngineer = isEngineer
            };

            return View(model);
        }
    }
}