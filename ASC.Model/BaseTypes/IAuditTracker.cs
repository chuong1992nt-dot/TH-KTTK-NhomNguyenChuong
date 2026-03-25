using System;

namespace ASC.Model.BaseTypes
{
    public interface IAuditTracker
    {
        bool IsDeleted { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
    }
}