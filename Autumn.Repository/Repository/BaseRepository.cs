using Autumn.Domain.Data;
using Autumn.Domain.Models;
using MongoDB.Driver;

namespace Autumn.Repository
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public BaseRepository(IStoreDatabaseSettings settings, string collectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<T>(collectionName);
        }

        // Common CRUD operations

        public virtual async Task<List<T>> GetAsync() =>
            await _collection.Find<T>(_ => true).ToListAsync();

        public virtual async Task<T> GetAsync(string id) =>
            await _collection.Find<T>(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();

        public virtual async Task InsertOneAsync(T entity) =>
            await _collection.InsertOneAsync(entity);

        public virtual async Task ReplaceOneAsync(string id, T entity) =>
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Id", id), entity);

        public virtual async Task DeleteOneAsync(string id) =>
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));

        public virtual async Task<List<T>> FindAsync(FilterDefinition<T> filter) =>
            await _collection.Find<T>(filter).ToListAsync();
        public virtual async Task<T> CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }
    }
}