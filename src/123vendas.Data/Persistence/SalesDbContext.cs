using Microsoft.EntityFrameworkCore;

namespace _123vendas.Data.Persistence
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
