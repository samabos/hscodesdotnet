using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.UI.Options;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Autumn.UI.Pages.Admin.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ProductService _productService;

        public CreateModel(ProductService productService)
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
            _productService.Create(Product);

            return RedirectToPage("./Index");
        }
    }
}