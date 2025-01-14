using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using MongoDB.Driver;

namespace Autumn.Infrastructure.Repository
{
    public class HsCodeDocumentRepository : BaseRepository<HSCodeToDocument>, IHsCodeDocumentRepository
    {
        public HsCodeDocumentRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.HSCodeToDocumentStoreCollectionName)
        {
        }
        public async Task<List<HSCodeToDocument>> GetWithCodeAsync(string code) =>
            await _collection.Find<HSCodeToDocument>(x => x.Hscode == code).ToListAsync();
    }
}
