using System.ComponentModel.DataAnnotations;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class MasterDataKey : BaseEntity
    {
        [Key]
        public string PartitionKey { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}