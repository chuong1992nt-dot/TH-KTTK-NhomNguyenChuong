using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Controllers
{
    [Authorize] // Bất cứ ai kế thừa class này đều BẮT BUỘC phải đăng nhập
    public class BaseController : Controller
    {
    }
}