using System.Collections.Generic;

namespace ASC.Web.Areas.Configuration.Models
{
    public class MasterKeysViewModel
    {
        // Phải có 2 dòng public này thì Controller mới gọi được
        public List<MasterDataKeyViewModel> MasterDataKeys { get; set; } = new List<MasterDataKeyViewModel>();

        public MasterDataKeyViewModel? MasterDataKeyInContext { get; set; }
    }
}