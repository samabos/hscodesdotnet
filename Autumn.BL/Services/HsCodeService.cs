using Autumn.Domain.Models;
using Autumn.Repository.Interface;
using Autumn.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service
{
    public class HsCodeService : BaseService<HSCode>, IHsCodeService
    {
        protected readonly IHsCodeRepository _hsCodeRepository;
        public HsCodeService(IHsCodeRepository hsCodeRepository) : base(hsCodeRepository)
        {
            _hsCodeRepository = hsCodeRepository;
        }
        public async Task<List<HSCode>> GetWithOptionsAsync(string id, string parentId, string level) =>
           await _hsCodeRepository.GetWithOptionsAsync(id, parentId, level);

        public async Task<List<HSCode>> GetWithHSCodeOptionsAsync(string code, string parentCode, string level) =>
            await _hsCodeRepository.GetWithHSCodeOptionsAsync(code, parentCode, level);

    }
}
