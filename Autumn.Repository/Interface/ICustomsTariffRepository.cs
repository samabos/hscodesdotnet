using Autumn.Domain.Models;

namespace Autumn.Infrastructure.Interface
{
    public interface ICustomsTariffRepository : IBaseRepository<CustomsTariff>
    {
        Task<List<CustomsTariff>> GetByHeaderAsync(string header);
        Task<CustomsTariff> GetByHSCodeAsync(string hscode);
    }
}
