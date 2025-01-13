using Autumn.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service.Interface
{
    public interface IBaseService<T>
    {
        Task<List<T>> GetAsync();
        Task<T> GetAsync(string id);
        //Task RemoveAsync(T entity);
        Task RemoveAsync(string id);
        Task UpdateAsync(string id, T entity);
        Task<T> CreateAsync(T entity);
    }
}
