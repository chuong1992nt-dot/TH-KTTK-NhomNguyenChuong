using ASC.Model.Models;
using ASC.Web.Areas.Configuration.Models;
using ASC.Web.Areas.ServiceRequests.Models;
using AutoMapper;

namespace ASC.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cấu hình map giữa Model và ViewModel
            CreateMap<MasterDataKey, MasterDataKeyViewModel>().ReverseMap();
            CreateMap<MasterDataValue, MasterDataValueViewModel>().ReverseMap();
            CreateMap<NewServiceRequestViewModel, ServiceRequest>().ReverseMap();
        }
    }
}