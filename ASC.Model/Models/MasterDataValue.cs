using System.ComponentModel.DataAnnotations;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class MasterDataValue : BaseEntity
    {
        // RowKey (kế thừa từ BaseEntity) là Primary Key
        // ApplicationDbContext.OnModelCreating đã đăng ký: HasKey(c => c.RowKey)
        // PartitionKey = tên của MasterDataKey cha (liên kết logic)

        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Value { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
