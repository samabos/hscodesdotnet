using Autumn.Domain.Models;
using Autumn.Repository.Interface;
using MongoDB.Driver;

namespace Autumn.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.ProductStoreCollectionName)
        {
        }

        public async Task<List<Product>> GetByTagsAsync(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                 return await _collection.Find(x => x.Tags != null).ToListAsync();
                //return await base.GetAsync();
            else
                return await _collection.Find(x => x.Tags.Contains(tag)).ToListAsync();
        }
       // public List<Product> GetByKeyword(string keyword) =>
       //  _collection.Find(x => x.Keyword == keyword).ToList();

        public async Task<List<Product>> GetByKeywordAsync(string keyword) =>
         await _collection.Find(x => x.Keyword.ToLower() == keyword.ToLower()).ToListAsync();

        public async Task<List<Product>> GetLikeKeywordAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                //return await _collection.Find(x => true).ToListAsync();
                return await base.GetAsync();
            }
            else
            {
                return await _collection.Find(x => x.Keyword.ToLower().StartsWith(keyword.ToLower())).ToListAsync();
            }
        }

        public override async Task<Product> CreateAsync(Product entity)
        {
            var now = DateTime.Now;
            entity.Created = now;
            entity.Modified = now;
            return await base.CreateAsync(entity);
        }

        public override async Task ReplaceOneAsync(string id, Product entity)
        {
            var now = DateTime.Now;
            entity.Modified = now;
            await base.ReplaceOneAsync(id, entity);
        }
    }
}
