using ASC.Web.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASC.Web.ViewComponents
{
    [ViewComponent(Name = "LeftNavigation")]
    public class LeftNavigationViewComponent : ViewComponent
    {
        private readonly NavigationMenu _navigationMenu;

        // DI (Dependency Injection) - Hút dữ liệu NavigationMenu từ hệ thống
        public LeftNavigationViewComponent(IOptions<NavigationMenu> navigationMenu)
        {
            _navigationMenu = navigationMenu.Value;
        }

        public IViewComponentResult Invoke()
        {
            // Trả danh sách menu (đã lấy được từ file JSON) sang cho View
            return View(_navigationMenu);
        }
    }
}