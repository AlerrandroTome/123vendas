using _123vendas.Domain.Interfaces;
using _123vendas.Domain.Services;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace _123vendas.Domain
{
    public static class DomainModule
    {
        public static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddFluentValidationAutoValidation();

            services.AddScoped<ISaleService, SaleService>();

            return services;
        }
    }
}
