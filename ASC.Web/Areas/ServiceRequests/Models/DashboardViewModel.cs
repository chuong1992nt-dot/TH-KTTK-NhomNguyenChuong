using ASC.Model.Models;
using System.Collections.Generic;

namespace ASC.Web.Areas.ServiceRequests.Models
{
    public class DashboardViewModel
    {
        public List<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
        public bool IsAdmin { get; set; }
        public bool IsEngineer { get; set; }
    }
}