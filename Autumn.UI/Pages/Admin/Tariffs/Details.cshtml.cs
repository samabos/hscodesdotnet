﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Admin.Tariffs
{
    public class DetailsModel : PageModel
    {
        private readonly CustomsTariffService _ctService;

        public DetailsModel(CustomsTariffService ctService)
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

    }
}