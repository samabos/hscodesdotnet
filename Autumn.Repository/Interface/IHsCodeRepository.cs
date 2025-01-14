using Autumn.Domain.Models;

namespace Autumn.Infrastructure.Interface
{
    public interface IHsCodeRepository : IBaseRepository<HSCode>
    {
        Task<List<HSCode>> GetWithHSCodeOptionsAsync(string code, string parentCode, string level);
        Task<List<HSCode>> GetWithOptionsAsync(string id, string parentId, string level);
    }
}
