﻿using Autumn.Domain.Models;
using Autumn.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Admin.Products
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }


        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _productService.GetAsync(id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}