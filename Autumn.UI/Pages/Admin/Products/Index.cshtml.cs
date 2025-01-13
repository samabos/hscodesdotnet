using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Autumn.Service.Interface;
using Autumn.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Autumn.UI.Pages.Admin.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {

        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public List<Product> ProductList { get; set; }
        public async Task OnGetAsync()
        {
           // ProductList = await _productService.GetAsync();
        }


        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }
        public async Task<JsonResult> OnPostAsync()
        {
            ProductList = await _productService.GetAsync();
            var recordsTotal = ProductList.Count();

            var customersQuery = ProductList.AsQueryable();

            var searchText = DataTablesRequest.Search.Value?.ToUpper();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                customersQuery = customersQuery.Where(s =>
                    s.Keyword.ToUpper().Contains(searchText) ||
                    s.Class.ToUpper().Contains(searchText) ||
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
    }
}
