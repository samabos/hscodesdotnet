using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Admin.Codes
{

        [Authorize]
        public class EditModel : PageModel
        {
        private readonly IHsCodeService _hscodeService;

        public EditModel(IHsCodeService hscodeService)
            {
            _hscodeService = hscodeService;
            }

            [BindProperty]
            public HSCode Row { get; set; }


            public async Task<IActionResult> OnGetAsync(string id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                Row = await _hscodeService.GetAsync(id);

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

            _hscodeService.UpdateAsync(Row.Id, Row);


                return RedirectToPage("./Index");
            }

            private bool ProductExists(string id)
            {
                if (_hscodeService.GetAsync(id) != null)
                    return true;
                else return false;

            }
        }
}