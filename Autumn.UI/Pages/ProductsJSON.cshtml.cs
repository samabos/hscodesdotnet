using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages
{
    public class ProductsJSONModel : PageModel
    {
        private readonly ProductService _productService;
        public ProductsJSONModel(ProductService productService)
        {
            _productService = productService;
        }

        public JsonResult OnGet()
        {
            return new JsonResult(_productService.Get().Select(x=>x.Keyword));
        }

      
    }
}