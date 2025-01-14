using Autumn.Domain.Models;

namespace Autumn.Infrastructure.Interface
{
    public interface IRequirementRepository : IBaseRepository<Requirement>
    {
        Task<List<Requirement>> GetByHSCodeAsync(string keyword);
        Task<List<Requirement>> GetLikeKeywordAsync(string keyword);
    }
}
