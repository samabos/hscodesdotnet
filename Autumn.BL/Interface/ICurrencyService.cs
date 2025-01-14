using Autumn.Domain.Models;
using System.Threading.Tasks;

namespace Autumn.Service.Interface
{
    public interface ICurrencyService : IBaseService<Currency>
    {
        Task<Currency> GetByCurrencyAsync(string currency);
    }
}
