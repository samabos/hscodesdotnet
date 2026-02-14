using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Autumn.Infrastructure.Repository
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

        /// <summary>
        /// Atlas Search with fuzzy matching on Description field, filtered to levels 3 & 4.
        /// Requires an Atlas Search index named "default" on the hscodes collection.
        /// Falls back to tokenized regex if Atlas Search is unavailable.
        /// </summary>
        public async Task<List<HSCode>> SearchByDescriptionAsync(string keyword, int limit = 20)
        {
            if (string.IsNullOrEmpty(keyword?.Trim()))
                return new List<HSCode>();

            // Try Atlas Search first
            try
            {
                var searchStage = new BsonDocument("$search", new BsonDocument
                {
                    { "index", "hscodes-index" },
                    { "compound", new BsonDocument
                        {
                            { "must", new BsonArray
                                {
                                    new BsonDocument("text", new BsonDocument
                                    {
                                        { "query", keyword },
                                        { "path", "Description" },
                                        { "fuzzy", new BsonDocument { { "maxEdits", 1 }, { "prefixLength", 2 } } }
                                    })
                                }
                            },
                            { "filter", new BsonArray
                                {
                                    new BsonDocument("range", new BsonDocument
                                    {
                                        { "path", "Level" },
                                        { "gte", 3 },
                                        { "lte", 4 }
                                    })
                                }
                            }
                        }
                    }
                });
                var limitStage = new BsonDocument("$limit", limit);

                var pipeline = PipelineDefinition<HSCode, HSCode>.Create(searchStage, limitStage);
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
                .Select(w => System.Text.RegularExpressions.Regex.Escape(w));
            var pattern = string.Join("", words.Select(w => $"(?=.*{w})")) + ".*";

            var filter = Builders<HSCode>.Filter.And(
                Builders<HSCode>.Filter.Regex(x => x.Description, new BsonRegularExpression(pattern, "i")),
                Builders<HSCode>.Filter.In(x => x.Level, new long[] { 3, 4 })
            );
            return await _collection.Find(filter).Limit(limit).SortBy(x => x.Level).ToListAsync();
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

