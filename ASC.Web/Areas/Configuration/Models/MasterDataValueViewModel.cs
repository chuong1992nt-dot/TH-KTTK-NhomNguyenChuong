using System.ComponentModel.DataAnnotations;

namespace ASC.Web.Areas.Configuration.Models
{
    public class MasterDataValueViewModel
    {
        [Required(ErrorMessage = "Partition Key is required.")]
        public string? PartitionKey { get; set; }

        public string? RowKey { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }         
        public string? Value { get; set; }
        public bool IsActive { get; set; }
    }
}