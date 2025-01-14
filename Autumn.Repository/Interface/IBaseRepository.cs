
namespace Autumn.Infrastructure.Interface
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetAsync();
        Task<T> GetAsync(string id);
        Task DeleteOneAsync(string id);
        Task InsertOneAsync(T x);
        Task ReplaceOneAsync(string id, T entity);
        Task<T> CreateAsync(T entity);
    }
}
