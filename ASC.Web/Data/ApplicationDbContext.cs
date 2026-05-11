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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MasterDataKey>().HasKey(c => c.PartitionKey);
            builder.Entity<MasterDataKey>().Ignore(c => c.RowKey); 

            builder.Entity<MasterDataValue>().HasKey(c => c.RowKey);

            builder.Entity<ServiceRequest>().HasKey(c => c.RowKey);
            builder.Entity<ServiceRequest>().Ignore(c => c.PartitionKey);

            base.OnModelCreating(builder);
        }
    }
}
