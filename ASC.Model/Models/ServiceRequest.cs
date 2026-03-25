using System;
using System.ComponentModel.DataAnnotations;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class ServiceRequest : BaseEntity
    {
        [Key]
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public string VehicleName { get; set; }
        public string VehicleType { get; set; }
        public string ServicePlan { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }
    }
}