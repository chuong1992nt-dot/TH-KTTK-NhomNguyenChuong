using System.ComponentModel.DataAnnotations;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class ServiceRequest : BaseEntity
    {
        // RowKey (từ BaseEntity) = Primary Key (đã đăng ký trong DbContext)
        // PartitionKey (từ BaseEntity) = Email của Customer (dùng để filter)

        public string? RequestedServices { get; set; }   // Dịch vụ chính (từ MasterKey)
        public string? SubRequestedServices { get; set; } // Dịch vụ phụ (từ MasterValue)
        public string? RequestedDate { get; set; }        // Ngày yêu cầu (string để tương thích)
        public string? VehicleType { get; set; }          // Loại xe (từ MasterValue)
        public string? VehicleRegNo { get; set; }         // Biển số xe
        public string? Status { get; set; } = "New";     // New | Active | InProgress | Completed | Rejected
        public string? CustomerContact { get; set; }      // SĐT liên hệ
        public bool IsRead { get; set; } = false;
        public string? CustomerCode { get; set; }         // Email customer (giữ nguyên tương thích với migration cũ)
        public string? ServiceEngineer { get; set; }      // Email kỹ sư được giao
    }
}
