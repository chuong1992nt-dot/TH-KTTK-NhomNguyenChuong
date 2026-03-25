using ASC.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace ASC.DataAccess
{
    // Kế thừa từ DbContext của Entity Framework
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Khai báo 3 bảng tương ứng với 3 Model bạn đã tạo
        public DbSet<MasterDataKey> MasterDataKeys { get; set; }
        public DbSet<MasterDataValue> MasterDataValues { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ép EF Core hiểu PartitionKey và RowKey là khóa chính
            modelBuilder.Entity<MasterDataKey>().HasKey(c => c.PartitionKey);
            modelBuilder.Entity<MasterDataValue>().HasKey(c => c.RowKey);
            modelBuilder.Entity<ServiceRequest>().HasKey(c => c.RowKey);
        }
    }
}