using Autumn.Domain.Models;

namespace Autumn.Infrastructure.Interface
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        Task<Country> GetByCodeAsync(string code);
    }
}
