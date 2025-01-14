
using Autumn.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages
{
    public class CurrenciesJSONModel : PageModel
    {
        private readonly ICurrencyService _currencyService;
        public CurrenciesJSONModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<JsonResult> OnGet()
        {
            return new JsonResult(await _currencyService.GetAsync());
        }
    }
}