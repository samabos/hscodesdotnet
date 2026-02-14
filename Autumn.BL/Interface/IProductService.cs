using Autumn.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service.Interface
{
    public interface IProductService : IBaseService<Product>
    {
        Task<List<Product>> GetByKeywordAsync(string keyword);
        Task<List<Product>> GetByTagsAsync(string tag);
        Task<List<Product>> GetLikeKeywordAsync(string keyword);
        Task<List<Product>> SearchByKeywordAsync(string keyword, int limit = 20);
    }
}
