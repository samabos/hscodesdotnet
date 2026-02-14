using Autumn.Domain.Models;

namespace Autumn.Infrastructure.Interface
{
    public interface ICustomsTariffRepository : IBaseRepository<CustomsTariff>
    {
        Task<List<CustomsTariff>> GetByHeaderAsync(string header);
        Task<CustomsTariff> GetByHSCodeAsync(string hscode);
        Task<CustomsTariff> GetByHSCodeAndCountryAsync(string hscode, string country);
        Task<List<CustomsTariff>> GetByHeaderAndCountryAsync(string header, string country);
    }
}
