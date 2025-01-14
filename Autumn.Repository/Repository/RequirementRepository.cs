using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using MongoDB.Driver;

namespace Autumn.Infrastructure.Repository
{
    public class RequirementRepository : BaseRepository<Requirement>, IRequirementRepository
    {
        public RequirementRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.RequirementStoreCollectionName)
        {
        }

        public async Task<List<Requirement>> GetByHSCodeAsync(string keyword) =>
            await _collection.Find<Requirement>(x => x.HSCode == keyword).ToListAsync();

        public async Task<List<Requirement>> GetLikeKeywordAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {

                return await _collection.Find<Requirement>(x => true).ToListAsync();
            }
            else
            {
                return await _collection.Find<Requirement>(x => x.HSCode.ToLower().StartsWith(keyword.ToLower())).ToListAsync();
            }
        }
    }
}
