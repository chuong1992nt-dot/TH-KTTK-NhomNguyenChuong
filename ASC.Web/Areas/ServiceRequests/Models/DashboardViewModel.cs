using ASC.Model.Models;

namespace ASC.Web.Areas.ServiceRequests.Models
{
    public class DashboardViewModel
    {
        public List<ServiceRequest> ServiceRequests { get; set; } = new();
        public bool IsAdmin { get; set; }
        public bool IsEngineer { get; set; }

        // Thống kê nhanh
        public int TotalNew { get; set; }
        public int TotalActive { get; set; }
        public int TotalInProgress { get; set; }
        public int TotalCompleted { get; set; }

        public bool IsCustomer => !IsAdmin && !IsEngineer;
    }
}
