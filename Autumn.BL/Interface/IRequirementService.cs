using Autumn.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service.Interface
{
    public interface IRequirementService : IBaseService<Requirement>
    {
        Task<List<Requirement>> GetByHSCodeAsync(string keyword);
        Task<List<Requirement>> GetLikeKeywordAsync(string keyword);
    }
}
