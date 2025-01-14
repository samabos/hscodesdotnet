using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using MongoDB.Driver;

namespace Autumn.Infrastructure.Repository
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.CurrencyStoreCollectionName)
        {
        }
        public override async Task<List<Currency>> GetAsync()
        {
            return await _collection.Find<Currency>(x => true) // Fetch all documents
                .SortByDescending(x => x.TimeStamp) // Sort by the timestamp field in descending order
                .Limit(12) // Limit the result to the 12 most recent documents
                .ToListAsync(); // Convert the result to a list
        }
        public async Task<Currency> GetByCurrencyAsync(string currency) =>
            await _collection.Find<Currency>(x => x.CurrencyCode == currency).FirstOrDefaultAsync();

    }
}
