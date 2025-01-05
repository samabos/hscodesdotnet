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


namespace Autumn.UI.Pages.Admin.Tariffs
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly CustomsTariffService _ctService;

        public EditModel(CustomsTariffService ctService)
        {
            _ctService = ctService;
        }

        [BindProperty]
        public CustomsTariff Row { get; set; }
        

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Row = await _ctService.GetAsync(id);

            if (Row == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _ctService.Update(Row.Id, Row);


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