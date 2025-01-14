using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using Autumn.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service
{
    public class HsCodeDocumentService : BaseService<HSCodeToDocument>, IHsCodeDocumentService
    {
        protected readonly IHsCodeDocumentRepository _repository;

        public HsCodeDocumentService(IHsCodeDocumentRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<List<HSCodeToDocument>> GetWithCodeAsync(string code) =>
            await _repository.GetWithCodeAsync(code);
    }
}
