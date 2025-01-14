﻿using System.Linq.Dynamic.Core;
using Autumn.Domain.Models;
using Autumn.Service.Interface;
using Autumn.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Admin.Codes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHsCodeService _hscodeService;

        public IndexModel(IHsCodeService hscodeService) {
            _hscodeService = hscodeService;
        }

        public List<HSCode> Rows { get; set; }
        public async Task OnGetAsync()
        {
            // ProductList = await _productService.GetAsync();
        }


        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }
        public async Task<JsonResult> OnPostAsync()
        {
            try
            {
                Rows = await _hscodeService.GetAsync();
                var recordsTotal = Rows.Count();

                var customersQuery = Rows.AsQueryable();

                var searchText = DataTablesRequest.Search.Value?.ToUpper();
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    customersQuery = customersQuery.Where(s =>
                        //s.Description.ToUpper().Contains(searchText) ||
                        //s.Header.ToUpper().Contains(searchText) ||
                        s.Code.ToUpper().Contains(searchText)
                    );
                }

                var recordsFiltered = customersQuery.Count();

                var sortColumnName = DataTablesRequest.Columns.ElementAt(DataTablesRequest.Order.ElementAt(0).Column).Name;
                var sortDirection = DataTablesRequest.Order.ElementAt(0).Dir.ToLower();

                // using System.Linq.Dynamic.Core
                customersQuery = customersQuery.OrderBy($"{sortColumnName} {sortDirection}");

                var skip = DataTablesRequest.Start;
                var take = DataTablesRequest.Length;
                var data = customersQuery
                    .Skip(skip)
                    .Take(take).ToList();

                return new JsonResult(new
                {
                    Draw = DataTablesRequest.Draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}