using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages
{
    public class HSCodeModel : PageModel
    {
        private readonly HSCodeService _hscodeService;
        public HSCodeModel(HSCodeService hscodeService)
        {
            _hscodeService = hscodeService;
        }

        public List<HSCode> HSCodes { get; set; }
        //public string Id { get; set; }
        //public string ParentId { get; set; }
        //public string Level { get; set; }
        public async Task OnGetAsync(string id=null,string pid=null,string level=null)
        {

            HSCodes = await _hscodeService.GetWithOptionsAsync(id,pid,level);
            
        }
    }
}