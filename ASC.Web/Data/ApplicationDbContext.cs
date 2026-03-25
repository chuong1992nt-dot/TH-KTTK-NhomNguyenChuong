using ASC.Model.Models;
using ASC.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASC.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<MasterDataKey> MasterDataKeys { get; set; }
        public DbSet<MasterDataValue> MasterDataValues { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public virtual DbSet<Product> Products { get; set; }    

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MasterDataKey>().HasKey(c => c.PartitionKey);
            builder.Entity<MasterDataValue>().HasKey(c => c.RowKey);
            builder.Entity<ServiceRequest>().HasKey(c => c.RowKey);

            base.OnModelCreating(builder); // Bắt buộc phải có dòng này cho Identity
        }
    }
}