using ASC.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")] // Đánh dấu Controller này thuộc Area nào
    public class DashboardController : BaseController
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}