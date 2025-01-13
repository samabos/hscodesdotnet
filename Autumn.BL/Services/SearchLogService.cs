using Autumn.Domain.Models;
using Autumn.Repository.Interface;
using Autumn.Service.Interface;

namespace Autumn.Service
{
    public class SearchLogService : BaseService<SearchLog>, ISearchLogService
    {
        protected readonly ISearchLogRepository _searchLogRepository;
        public SearchLogService(ISearchLogRepository searchLogRepository) : base(searchLogRepository)
        {
            _searchLogRepository = searchLogRepository;
        }
    }
}
