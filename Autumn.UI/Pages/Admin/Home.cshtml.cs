using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Admin
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly HSCodeService _hscodeService;
        private readonly CustomsTariffService _ctService;

        public HomeModel(ProductService productService, HSCodeService hscodeService, CustomsTariffService ctService)
        {
            _productService = productService;
            _hscodeService = hscodeService;
            _ctService = ctService;
        }

        public int ProductCNT { get; set; }
        public int HSCodeCNT { get; set; }
        public int TariffCNT { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var p = await _productService.GetAsync();
                var h = await _hscodeService.GetAsync();
                var c = await _ctService.GetAsync();
                ProductCNT = p.Count; HSCodeCNT = h.Count; TariffCNT = c.Count;
            }
            catch (Exception ex) { }

        }



    }
}