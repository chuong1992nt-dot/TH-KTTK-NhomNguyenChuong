using AutoMapper;
using ServiceRequestModel = ASC.Model.Models.ServiceRequest;

namespace ASC.Web.Areas.ServiceRequests.Models
{
    public class ServiceRequestMappingProfile : Profile
    {
        public ServiceRequestMappingProfile()
        {
            CreateMap<NewServiceRequestViewModel, ServiceRequestModel>().ReverseMap();
        }
    }
}