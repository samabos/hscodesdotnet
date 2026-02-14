using Autumn.Domain.Models;

namespace Autumn.Infrastructure.Interface
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        //List<Product> GetByKeyword(string keyword);
        Task<List<Product>> GetByKeywordAsync(string keyword);
        Task<List<Product>> GetByTagsAsync(string tag);
        Task<List<Product>> GetLikeKeywordAsync(string keyword);
        Task<List<Product>> SearchByKeywordAsync(string keyword, int limit = 20);
    }
}
