using Autumn.Domain.Models;

namespace Autumn.Repository.Interface
{
    public interface IHsCodeRepository : IBaseRepository<HSCode>
    {
        Task<List<HSCode>> GetWithHSCodeOptionsAsync(string code, string parentCode, string level);
        Task<List<HSCode>> GetWithOptionsAsync(string id, string parentId, string level);
    }
}
