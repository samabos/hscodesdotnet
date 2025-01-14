using Autumn.Domain.Models;
using Autumn.Infrastructure.Interface;
using Autumn.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service
{
    public class RequirementService : BaseService<Requirement>, IRequirementService
    {
        protected readonly IRequirementRepository _repository;
        public RequirementService(IRequirementRepository repository) : base(repository)
        {
            _repository = repository;
        }
        public async Task<List<Requirement>> GetByHSCodeAsync(string keyword) =>
            await _repository.GetByHSCodeAsync(keyword);

        public async Task<List<Requirement>> GetLikeKeywordAsync(string keyword) =>
            await _repository.GetLikeKeywordAsync(keyword);
    }
}
