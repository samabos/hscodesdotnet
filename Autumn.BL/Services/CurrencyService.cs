using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using Autumn.Service.Interface;
using System.Threading.Tasks;

namespace Autumn.Service
{
    public class CurrencyService : BaseService<Currency>, ICurrencyService
    {
        protected readonly ICurrencyRepository _repository;

        public CurrencyService(ICurrencyRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Currency> GetByCurrencyAsync(string currency) =>
            await _repository.GetByCurrencyAsync(currency);
    }
}
