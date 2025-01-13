using Autumn.Domain.Models;
using Autumn.Repository.Interface;

namespace Autumn.Repository
{
    public class SearchLogRepository : BaseRepository<SearchLog>, ISearchLogRepository
    {
        public SearchLogRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.HSCodeStoreCollectionName)
        {
        }

    }
}
