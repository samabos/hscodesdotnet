using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages
{
    public class CurrenciesJSONModel : PageModel
    {
        private readonly CurrencyService _currencyService;
        public CurrenciesJSONModel(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public JsonResult OnGet()
        {
            return new JsonResult(_currencyService.Get());
        }
    }
}