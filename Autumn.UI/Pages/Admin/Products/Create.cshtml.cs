﻿using Autumn.Domain.Models;
using Autumn.UI.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Autumn.Service.Interface;

namespace Autumn.UI.Pages.Admin.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;

        public CreateModel(IProductService productService)
        {
            _productService = productService;
        }


        [BindProperty]
        public Product Product { get; set; }
        public string[] Structure => StaticString.Structure;

        public IActionResult OnGet()
        {

            var user = User.Identity.Name;
            Product = new Product() { Tags = new string[] { } };
            Product.ModifiedBy = user;
            Product.CreatedBy = user;
            return Page();
        }
        

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = (HttpContext.User.Identity as ClaimsIdentity);
            //var user = User.Identity.Name;
            Product.ModifiedBy = user.Name;
            Product.CreatedBy = user.Name;
            _productService.CreateAsync(Product);

            return RedirectToPage("./Index");
        }
    }
}