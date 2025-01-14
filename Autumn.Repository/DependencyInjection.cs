using Autumn.Infrastructure.Interface;
using Autumn.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Autumn.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IHsCodeRepository, HsCodeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISearchLogRepository, SearchLogRepository>();
            services.AddScoped<ICustomsTariffRepository, CustomsTariffRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IHsCodeDocumentRepository, HsCodeDocumentRepository>();
            services.AddScoped<IRequirementRepository, RequirementRepository>();
            services.AddScoped<IDocumentRepository,DocumentRepository>();
            services.AddScoped<IKeywordRepository, KeywordRepository>();

        }
    }
}
