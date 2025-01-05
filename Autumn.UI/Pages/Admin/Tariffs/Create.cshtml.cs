using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Admin.Tariffs
{
    public class CreateModel : PageModel
    {
        private readonly CustomsTariffService _ctService;

        public CreateModel(CustomsTariffService ctService)
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

            _ctService.Create(Row);


            return RedirectToPage("./Index");
        }

        private bool ProductExists(string id)
        {
            if (_ctService.Get(id) != null)
                return true;
            else return false;

        }
    }
}