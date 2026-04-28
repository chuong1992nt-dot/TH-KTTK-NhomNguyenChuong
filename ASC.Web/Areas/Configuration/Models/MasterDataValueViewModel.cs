using System.ComponentModel.DataAnnotations;

namespace ASC.Web.Areas.Configuration.Models
{
    public class MasterDataValueViewModel
    {
        [Required(ErrorMessage = "Partition Key is required.")]
        public string? PartitionKey { get; set; } // Thêm dấu ?

        public string? RowKey { get; set; }       // Thêm dấu ?

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }         // Thêm dấu ?

        public bool IsActive { get; set; }
    }
}