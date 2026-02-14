using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using MongoDB.Driver;

namespace Autumn.Infrastructure.Repository
{
    public class CustomsTariffRepository : BaseRepository<CustomsTariff>, ICustomsTariffRepository
    {
        public CustomsTariffRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.CustomsTariffStoreCollectionName)
        {
        }
        public async Task<CustomsTariff> GetByHSCodeAsync(string hscode) =>
            await _collection.Find<CustomsTariff>(x => x.HSCode == hscode).FirstOrDefaultAsync();
        public async Task<List<CustomsTariff>> GetByHeaderAsync(string header) =>
                 await _collection.Find<CustomsTariff>(x => x.Header == header).ToListAsync();

        public async Task<CustomsTariff> GetByHSCodeAndCountryAsync(string hscode, string country) =>
            await _collection.Find<CustomsTariff>(x =>
                x.HSCode == hscode && (x.Country == country || (x.Country == null && country == "NG"))
            ).FirstOrDefaultAsync();

        public async Task<List<CustomsTariff>> GetByHeaderAndCountryAsync(string header, string country) =>
            await _collection.Find<CustomsTariff>(x =>
                x.Header == header && (x.Country == country || (x.Country == null && country == "NG"))
            ).ToListAsync();
    }
}
