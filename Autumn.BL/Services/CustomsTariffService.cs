using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using Autumn.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service
{
    public class CustomsTariffService : BaseService<CustomsTariff>, ICustomsTariffService
    {
        protected readonly ICustomsTariffRepository _repository;

        public CustomsTariffService(ICustomsTariffRepository repository) : base(repository)
        {
            _repository = repository;
        }
        public async Task<CustomsTariff> GetByHSCodeAsync(string hscode) =>
            await _repository.GetByHSCodeAsync(hscode);
        public async Task<List<CustomsTariff>> GetByHeaderAsync(string header) =>
            await _repository.GetByHeaderAsync(header);

        public async Task<CustomsTariff> GetByHSCodeAndCountryAsync(string hscode, string country) =>
            await _repository.GetByHSCodeAndCountryAsync(hscode, country);
        public async Task<List<CustomsTariff>> GetByHeaderAndCountryAsync(string header, string country) =>
            await _repository.GetByHeaderAndCountryAsync(header, country);
    }
}
