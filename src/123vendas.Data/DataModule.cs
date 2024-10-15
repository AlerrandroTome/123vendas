using _123vendas.Data.Persistence;
using _123vendas.Data.Persistence.Repositories;
using _123vendas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _123vendas.Data
{
    public static class DataModule
    {
        public static IServiceCollection ConfigureDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SalesDB");
            services.AddDbContext<SalesDbContext>(options =>
                options.UseSqlServer(connectionString));


            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ISaleItemRepository, SaleItemRepository>();

            return services;
        }
    }
}