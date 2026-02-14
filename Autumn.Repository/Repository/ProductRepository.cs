using System.Text.RegularExpressions;
using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Autumn.Infrastructure.Repository
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
            else
                return await _collection.Find(x => x.Tags.Contains(tag)).ToListAsync();
        }

        public async Task<List<Product>> GetByKeywordAsync(string keyword) =>
         await _collection.Find(x => x.Keyword.ToLower() == keyword.ToLower()).ToListAsync();

        public async Task<List<Product>> GetLikeKeywordAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await base.GetAsync();
            }
            else
            {
                return await _collection.Find(x => x.Keyword.ToLower().StartsWith(keyword.ToLower())).ToListAsync();
            }
        }

        /// <summary>
        /// Atlas Search with fuzzy matching on the Keyword field.
        /// Requires an Atlas Search index named "default" on the products collection.
        /// Falls back to tokenized regex if Atlas Search is unavailable.
        /// </summary>
        public async Task<List<Product>> SearchByKeywordAsync(string keyword, int limit = 20)
        {
            if (string.IsNullOrEmpty(keyword?.Trim()))
                return new List<Product>();

            // Try Atlas Search first (word splitting + fuzzy matching)
            try
            {
                var searchStage = new BsonDocument("$search", new BsonDocument
                {
                    { "index", "product-index" },
                    { "text", new BsonDocument
                        {
                            { "query", keyword },
                            { "path", "Keyword" },
                            { "fuzzy", new BsonDocument { { "maxEdits", 1 }, { "prefixLength", 2 } } }
                        }
                    }
                });
                var limitStage = new BsonDocument("$limit", limit);

                var pipeline = PipelineDefinition<Product, Product>.Create(searchStage, limitStage);
                var results = await _collection.Aggregate(pipeline).ToListAsync();
                if (results.Count > 0)
                    return results;
            }
            catch
            {
                // Atlas Search not available — fall through to regex
            }

            // Fallback: tokenized regex (split query into words, match all in any order)
            var words = keyword.Trim().Split(new[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => Regex.Escape(w));
            var pattern = string.Join("", words.Select(w => $"(?=.*{w})")) + ".*";

            var filter = Builders<Product>.Filter.Regex(
                x => x.Keyword,
                new BsonRegularExpression(pattern, "i"));
            return await _collection.Find(filter).Limit(limit).ToListAsync();
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
