using Autumn.Domain.Models;

namespace Autumn.Infrastructure.Interface
{
    public interface IHsCodeDocumentRepository : IBaseRepository<HSCodeToDocument>
    {
        Task<List<HSCodeToDocument>> GetWithCodeAsync(string code);
    }
}
