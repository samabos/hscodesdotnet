using Autumn.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages
{
    public class ProductsJSONModel : PageModel
    {
        private readonly IProductService _productService;
        public ProductsJSONModel(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<JsonResult> OnGet()
        {
            var products = await _productService.GetAsync();
            return new JsonResult(products.Select(x=>x.Keyword));
        }

    }
}