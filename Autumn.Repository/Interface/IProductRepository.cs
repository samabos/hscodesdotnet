using Autumn.Domain.Models;

namespace Autumn.Repository.Interface
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        //List<Product> GetByKeyword(string keyword);
        Task<List<Product>> GetByKeywordAsync(string keyword);
        Task<List<Product>> GetByTagsAsync(string tag);
        Task<List<Product>> GetLikeKeywordAsync(string keyword);
    }
}
