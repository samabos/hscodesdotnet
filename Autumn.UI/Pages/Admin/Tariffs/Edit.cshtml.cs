using Autumn.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Autumn.Service.Interface;


namespace Autumn.UI.Pages.Admin.Tariffs
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ICustomsTariffService _ctService;

        public EditModel(ICustomsTariffService ctService)
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

            _ctService.UpdateAsync(Row.Id, Row);


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