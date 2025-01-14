using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using Autumn.Service.Interface;

namespace Autumn.Service
{
    public class DocumentService : BaseService<Document>, IDocumentService
    {
        protected readonly IDocumentRepository _repository;
        public DocumentService(IDocumentRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
