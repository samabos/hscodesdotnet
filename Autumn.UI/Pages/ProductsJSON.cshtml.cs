using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn.Service.Interface;
using Autumn.UI.ViewModels;
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