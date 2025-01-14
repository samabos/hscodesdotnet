using Autumn.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service.Interface
{
    public interface IHsCodeDocumentService : IBaseService<HSCodeToDocument>
    {
        Task<List<HSCodeToDocument>> GetWithCodeAsync(string code);
    }
}
