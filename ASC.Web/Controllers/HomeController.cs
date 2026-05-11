using System.Diagnostics;
using ASC.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ASC.Web.Configuration;


namespace ASC.Web.Controllers
{
    public class HomeController : AnonymousController
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IOptions<ApplicationSettings> _settings;

        public HomeController(ILogger<HomeController> logger, IOptions<ApplicationSettings> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public IActionResult Index()
        {
            // Admin và Engineer tự động vào Dashboard, không ở lại trang chủ
            if (User.IsInRole("Admin") || User.IsInRole("Engineer"))
                return RedirectToAction("Dashboard", "Dashboard", new { area = "ServiceRequests" });

            ViewBag.Title = _settings.Value.ApplicationTitle;
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}