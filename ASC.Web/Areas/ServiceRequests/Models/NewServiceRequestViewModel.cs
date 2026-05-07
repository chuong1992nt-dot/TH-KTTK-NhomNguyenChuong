using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASC.Web.Areas.ServiceRequests.Models
{
    public class NewServiceRequestViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn dịch vụ")]
        public string? RequestedServices { get; set; }

        public string? SubRequestedServices { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày")]
        public string? RequestedDate { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại xe")]
        public string? VehicleType { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập biển số xe")]
        public string? VehicleRegNo { get; set; }

        public string? CustomerContact { get; set; }
        public string? Status { get; set; }

        // Dropdown lists
        public List<SelectListItem>? ServicesList { get; set; }
        public List<SelectListItem>? VehicleTypeList { get; set; }
    }
}