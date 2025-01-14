using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;

namespace Autumn.Infrastructure.Repository
{
    public class KeywordRepository : BaseRepository<Keyword>, IKeywordRepository
    {
        public KeywordRepository(IStoreDatabaseSettings settings) 
            : base(settings, settings.KeywordStoreCollectionName)
        {
        }

    }
}
