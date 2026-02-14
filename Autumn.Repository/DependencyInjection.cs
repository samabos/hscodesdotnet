using Autumn.Domain.Data;
using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using Autumn.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            services.AddScoped<ICountryRepository, CountryRepository>();

        }

        public static IServiceCollection AddRelationalDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<classificationContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddDocumentDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StoreDatabaseSettings>(
                configuration.GetSection(nameof(StoreDatabaseSettings)));

            services.AddSingleton<IStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<StoreDatabaseSettings>>().Value);

            return services;
        }
    }
}
