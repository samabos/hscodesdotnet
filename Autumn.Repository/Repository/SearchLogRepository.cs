using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;

namespace Autumn.Infrastructure.Repository
{
    public class SearchLogRepository : BaseRepository<SearchLog>, ISearchLogRepository
    {
        public SearchLogRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.HSCodeStoreCollectionName)
        {
        }

    }
}
