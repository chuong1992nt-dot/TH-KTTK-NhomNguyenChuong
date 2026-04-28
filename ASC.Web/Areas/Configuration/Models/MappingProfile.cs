using ASC.Model.Models; // Gọi đến Project Model của bạn
using AutoMapper;

namespace ASC.Web.Areas.Configuration.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Tự động map dữ liệu 2 chiều giữa CSDL (Model) và Giao diện (ViewModel)
            CreateMap<MasterDataKey, MasterDataKeyViewModel>().ReverseMap();
            CreateMap<MasterDataValue, MasterDataValueViewModel>().ReverseMap();
        }
    }
}