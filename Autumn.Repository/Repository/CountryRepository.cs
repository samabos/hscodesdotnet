using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using MongoDB.Driver;

namespace Autumn.Infrastructure.Repository
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.CountryStoreCollectionName)
        {
        }

        public async Task<Country> GetByCodeAsync(string code) =>
            await _collection.Find<Country>(x => x.Code == code).FirstOrDefaultAsync();
    }
}
