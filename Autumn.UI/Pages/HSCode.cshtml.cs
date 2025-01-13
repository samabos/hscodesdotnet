using Autumn.Domain.Models;
using Autumn.Service.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages
{
    public class HSCodeModel : PageModel
    {
        private readonly IHsCodeService _hsCodeService;
        public HSCodeModel(IHsCodeService hsCodeService)
        {
            _hsCodeService = hsCodeService;
        }

        public List<HSCode> HSCodes { get; set; }
        //public string Id { get; set; }
        //public string ParentId { get; set; }
        //public string Level { get; set; }
        public async Task OnGetAsync(string id=null,string pid=null,string level=null)
        {

            HSCodes = await _hsCodeService.GetWithOptionsAsync(id,pid,level);
            
        }
    }
}