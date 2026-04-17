using System.Collections.Generic;
using ASC.Web.Models;

namespace ASC.Web.Areas.Accounts.Models
{
    public class ServiceEngineerViewModel
    {
        // Danh sách kỹ sư để hiển thị lên bảng
        public List<ApplicationUser> ServiceEngineers { get; set; }

        // Dữ liệu cho Form thêm mới kỹ sư
        public ServiceEngineerRegistrationViewModel Registration { get; set; }
    }
}