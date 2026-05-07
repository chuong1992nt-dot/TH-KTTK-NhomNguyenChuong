using System;
using System.ComponentModel.DataAnnotations;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class ServiceRequest : BaseEntity
    {
        public string? RequestedServices { get; set; }
        public string? SubRequestedServices { get; set; }
        public string? RequestedDate { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleRegNo { get; set; }
        public string? Status { get; set; }
        public string? CustomerContact { get; set; }
        public bool IsRead { get; set; }
        public string? CustomerCode { get; set; }
        public string? ServiceEngineer { get; set; }
    }
}