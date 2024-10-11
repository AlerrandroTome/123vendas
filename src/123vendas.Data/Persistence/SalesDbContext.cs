using _123vendas.Domain.Entities.Aggregates.Sales;
using Microsoft.EntityFrameworkCore;

namespace _123vendas.Data.Persistence
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
