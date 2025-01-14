using Autumn.Service.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Autumn.Service
{
    public static class DependencyInjection
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IHsCodeService, HsCodeService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISearchLogService, SearchLogService>();
            services.AddScoped<ICustomsTariffService, CustomsTariffService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IHsCodeDocumentService, HsCodeDocumentService>();
            services.AddScoped<IRequirementService, RequirementService>();

            services.AddScoped<IPredictService, PredictService>();
        }
    }
}
