using System;
using System.Collections.Generic;
using System.Linq;
using Autumn.Domain.Models;
using Autumn.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Admin.Tariffs
{
    public class CreateModel : PageModel
    {
        private readonly ICustomsTariffService _ctService;

        public CreateModel(ICustomsTariffService ctService)
        {
            _ctService = ctService;
        }

        [BindProperty]
        public CustomsTariff Row { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Row = new CustomsTariff();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _ctService.CreateAsync(Row);


            return RedirectToPage("./Index");
        }

        private bool ProductExists(string id)
        {
            if (_ctService.GetAsync(id) != null)
                return true;
            else return false;

        }
    }
}