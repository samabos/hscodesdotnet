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
using Autumn.Service.Interface;

namespace Autumn.UI.Pages.Admin.Products
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;

        public EditModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Product Product { get; set; }

        public string[] Structure => StaticString.Structure;

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

            var user = User.Identity.Name;
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
            _productService.UpdateAsync(Product.Id, Product);


            return RedirectToPage("./Index");
        }

        private bool ProductExists(string id)
        {
            if (_productService.GetAsync(id) != null)
                return true;
            else return false;

        }
    }
}