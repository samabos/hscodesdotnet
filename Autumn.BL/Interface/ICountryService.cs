using Autumn.Domain.Models;
using System.Threading.Tasks;

namespace Autumn.Service.Interface
{
    public interface ICountryService : IBaseService<Country>
    {
        Task<Country> GetByCodeAsync(string code);
    }
}
