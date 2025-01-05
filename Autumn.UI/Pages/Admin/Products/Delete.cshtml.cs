﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Admin.Products
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ProductService _productService;

        public DeleteModel(ProductService productService)
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _productService.GetAsync(id);

            if (Product != null)
            {
                _productService.Remove(Product);
            }

            return RedirectToPage("./Index");
        }
    }
}