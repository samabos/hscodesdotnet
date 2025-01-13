using Autumn.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Autumn.Repository
{
    public static class DependencyInjection
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IHsCodeRepository, HsCodeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISearchLogRepository, SearchLogRepository>();
            services.AddScoped<ICustomsTariffRepository, CustomsTariffRepository>();
        }
    }
}
