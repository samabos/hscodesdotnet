using Autumn.Infrastructure.Interface;
using Autumn.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service
{
    public class BaseService<T> : IBaseService<T>
    {
        protected readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<List<T>> GetAsync() =>
            await _repository.GetAsync();

        public async Task<T> GetAsync(string id) =>
            await _repository.GetAsync(id);

        public async Task<T> CreateAsync(T entity) =>
            await _repository.CreateAsync(entity);

        public async Task UpdateAsync(string id, T entity) =>
            await _repository.ReplaceOneAsync(id, entity);

       // public async Task RemoveAsync(T entity) =>
       //     await _repository.DeleteOneAsync(entity.Id);

        public async Task RemoveAsync(string id) =>
            await _repository.DeleteOneAsync(id);
    }
}
