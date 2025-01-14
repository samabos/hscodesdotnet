using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;

namespace Autumn.Infrastructure.Repository
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(IStoreDatabaseSettings settings)
            : base(settings, settings.CurrencyStoreCollectionName)
        {
        }
    }
}
