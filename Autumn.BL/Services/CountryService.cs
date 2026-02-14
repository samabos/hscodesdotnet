using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using Autumn.Service.Interface;
using System.Threading.Tasks;

namespace Autumn.Service
{
    public class CountryService : BaseService<Country>, ICountryService
    {
        protected readonly ICountryRepository _repository;

        public CountryService(ICountryRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Country> GetByCodeAsync(string code) =>
            await _repository.GetByCodeAsync(code);
    }
}
