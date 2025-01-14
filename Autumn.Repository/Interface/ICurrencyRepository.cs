using Autumn.Domain.Models;

namespace Autumn.Infrastructure.Interface
{
    public interface ICurrencyRepository : IBaseRepository<Currency>
    {
        Task<Currency> GetByCurrencyAsync(string currency);
    }
}
