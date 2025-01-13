using Autumn.Domain.Models;
using Autumn.Repository.Interface;
using MongoDB.Driver;

namespace Autumn.Repository
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
    }
}
