using ASC.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASC.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Thử lấy thông tin User từ Session
            var currentUser = HttpContext.Session.GetSession<CurrentUser>("CurrentUser");

            // Nếu Session trống (người dùng vừa đăng nhập xong)
            if (currentUser == null)
            {
                // Kiểm tra xem Identity đã xác thực chưa
                if (User.Identity.IsAuthenticated)
                {
                    // Trích xuất thông tin (Email, Name, Roles...) bằng hàm ta đã viết ở ASC.Utilities
                    currentUser = User.GetCurrentUserDetails();

                    // Lưu lại vào Session để lần sau không cần lấy lại nữa
                    HttpContext.Session.SetSession("CurrentUser", currentUser);
                }
            }
        }
    }
}