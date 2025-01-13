using Autumn.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service.Interface
{
    public interface IHsCodeService : IBaseService<HSCode>
    {
        Task<List<HSCode>> GetWithHSCodeOptionsAsync(string code, string pcode, string level);
        Task<List<HSCode>> GetWithOptionsAsync(string id, string pid, string level);
    }
}
