using _123vendas.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _123vendas.Data
{
    public static class DataModule
    {
        public static IServiceCollection ConfigureDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("salesdb");
            services.AddDbContext<SalesDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}