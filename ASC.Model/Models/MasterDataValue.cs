using System.ComponentModel.DataAnnotations;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class MasterDataValue : BaseEntity
    {
        [Key]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}