using System.ComponentModel.DataAnnotations;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class MasterDataKey : BaseEntity
    {
        // PartitionKey (kế thừa từ BaseEntity) là Primary Key
        // ApplicationDbContext.OnModelCreating đã đăng ký: HasKey(c => c.PartitionKey)
        // KHÔNG dùng [Key] ở Name vì sẽ xung đột với DbContext config

        [Required]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
