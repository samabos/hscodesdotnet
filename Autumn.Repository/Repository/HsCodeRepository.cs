using Autumn.Domain.Models;
using Autumn.Repository.Interface;
using MongoDB.Driver;

namespace Autumn.Repository
{
    public class HsCodeRepository : BaseRepository<HSCode>, IHsCodeRepository
    {
        public HsCodeRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.HSCodeStoreCollectionName)
        {
        }

        // Custom methods for specific queries

        public async Task<List<HSCode>> GetWithOptionsAsync(string id, string parentId, string level)
        {
            IEnumerable<HSCode> resp = new List<HSCode>();
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(parentId) && string.IsNullOrEmpty(level))
            {
                resp = await _collection.Find(x => x.Level == 1).ToListAsync();
            }
            else if (string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(parentId) && !string.IsNullOrEmpty(level))
            {
                resp = await _collection.Find(x => x.Level == long.Parse(level) && x.ParentId == parentId).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(parentId) && string.IsNullOrEmpty(level))
            {
                resp = await _collection.Find(x => x.Id == id).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(parentId) && !string.IsNullOrEmpty(level))
            {
                var f = await _collection.Find(x => x.Id == id && x.Level == long.Parse(level)).FirstOrDefaultAsync();
                resp = await _collection.Find(x => x.ParentId == f.ParentId && x.Level == long.Parse(level)).ToListAsync();
            }
            else if (string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(parentId) && string.IsNullOrEmpty(level))
            {
                resp = await _collection.Find(x => x.ParentId == parentId).ToListAsync();
            }
            return resp.OrderBy(x => x.Order).ToList();
        }

        public async Task<List<HSCode>> GetWithHSCodeOptionsAsync(string code, string parentCode, string level)
        {
            IEnumerable<HSCode> resp = new List<HSCode>();
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(parentCode) && string.IsNullOrEmpty(level))
            {
                resp = await _collection.Find(x => x.Level == 1).ToListAsync();
            }
            else if (string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(parentCode) && !string.IsNullOrEmpty(level))
            {
                resp = await _collection.Find(x => x.Level == long.Parse(level) && x.ParentCode == parentCode).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(code) && string.IsNullOrEmpty(parentCode) && string.IsNullOrEmpty(level))
            {
                resp = await _collection.Find(x => x.Code == code).ToListAsync();
            }
            else if (string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(parentCode) && string.IsNullOrEmpty(level))
            {
                resp = await _collection.Find(x => x.ParentCode == parentCode).ToListAsync();
                if (!resp.Any())
                    resp = await _collection.Find(x => x.Code.StartsWith(parentCode)).ToListAsync();
            }
            return resp.OrderBy(x => x.Order).ToList();
        }

        // Override methods for specific operations if necessary
        public override async Task InsertOneAsync(HSCode x)
        {
            await base.InsertOneAsync(x); // You can customize behavior here
        }

        public override async Task ReplaceOneAsync(string id, HSCode entity)
        {
            await _collection.ReplaceOneAsync(Builders<HSCode>.Filter.Eq("PId", id), entity);
        }

        public override async Task DeleteOneAsync(string id)
        {
            await _collection.DeleteOneAsync(Builders<HSCode>.Filter.Eq("PId", id));
        }

    }
}

