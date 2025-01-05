using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Admin.Tariffs
{
    [Authorize]
    public class IndexModel : PageModel
    {

        private readonly CustomsTariffService _ctService;

        public IndexModel(CustomsTariffService ctService)
        {
            _ctService = ctService;
        }

        public List<CustomsTariff> Rows { get; set; }
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
                Rows = await _ctService.GetAsync();
                var recordsTotal = Rows.Count();

                var customersQuery = Rows.AsQueryable();

                var searchText = DataTablesRequest.Search.Value?.ToUpper();
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    customersQuery = customersQuery.Where(s =>
                        //s.Description.ToUpper().Contains(searchText) ||
                        //s.Header.ToUpper().Contains(searchText) ||
                        s.HSCode.ToUpper().Contains(searchText)
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
            catch (Exception ex) {
                return null;
            }
        }
    }
}