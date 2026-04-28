using System.Collections.Generic;

namespace ASC.Web.Areas.Configuration.Models
{
    public class MasterValuesViewModel
    {
        // Khởi tạo danh sách mặc định
        public List<MasterDataValueViewModel> MasterDataValues { get; set; } = new List<MasterDataValueViewModel>();

        public MasterDataValueViewModel? MasterDataValueInContext { get; set; } // Thêm dấu ?
    }
}