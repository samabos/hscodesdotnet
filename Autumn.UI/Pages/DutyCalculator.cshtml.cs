using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Infra;
using Autumn.Domain.Models;
using Autumn.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Autumn.UI.Pages
{
    public class DutyCalculatorModel : PageModel
    {
        private IExRate _exRate;
        private readonly CurrencyService _curencyService;
        private readonly CustomsTariffService _tariffService;

        public DutyCalculatorModel(IExRate exRate, CurrencyService curencyService, CustomsTariffService tariffService) {
            _exRate = exRate;
            _curencyService = curencyService;
            _tariffService = tariffService;
        }

        [BindProperty]
        [Required]
        [Display(Name = "Commodity Description")]
        public string ProductDesc { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "HS Code")]
        public string HSCode { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Cost Price")]
        public decimal Cost { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Freight Amount")]
        public decimal Freight { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Insurance Amount")]
        public decimal Insurance { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Currency")]
        public string Currency { get; set; }
        public decimal ExRate { get; set; }
        public decimal CF { get; set; }
        public decimal CIF { get; set; }
        public decimal CIFLocal { get; set; }

        public string IDRate { get; set; }
        public string VATRate { get; set; }
        public string ETLRate { get; set; }
        public string SURRate { get; set; }
        public string CISSRate { get; set; }
        public string NACRate { get; set; }
        public string LEVYRate { get; set; }
        public decimal IDPayableLocal { get; set; }
        public decimal VATPayableLocal { get; set; }
        public decimal ETLPayableLocal { get; set; }
        public decimal SURPayableLocal { get; set; }
        public decimal CISSPayable { get; set; }
        public decimal CISSPayableLocal { get; set; }
        public decimal NACPayable { get; set; }
        public decimal NACPayableLocal { get; set; }
        public decimal LEVYPayableLocal { get; set; }
        public decimal TotalPayableLocal { get; set; }

        public string HSCodeDescription { get; set; }

        public IList<SelectListItem> GetCurrencies { get; set; }

        public async Task OnGetAsync()
        {
            GetCurrencies = _curencyService.Get().Select(x=> new SelectListItem {Text=x.CurrencyCode,Value=x.CurrencyCode}).ToList();
        }
        public IActionResult OnPost()
        {

            GetCurrencies = _curencyService.Get().Select(x => new SelectListItem { Text = x.CurrencyCode, Value = x.CurrencyCode }).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    //Get HS Code Tariff 
                    var tariff = _tariffService.GetByHSCode(HSCode);
                    var currency = _curencyService.GetByCurrency(Currency);
                    var cif = Cost + Insurance + Freight;
                    var cf = Cost + Freight;
                    ExRate = Convert.ToDecimal(currency.Rate);
                    var duty = cif * (decimal.Parse(tariff.DUTY) / 100);
                    var vat = (cif + duty) * (decimal.Parse(tariff.VAT) / 100);
                    var sur = cif * (decimal.Parse(tariff.SUR) / 100);
                    var etl = cif * (decimal.Parse(tariff.ETLS) / 100);
                    var ciss = cif * (decimal.Parse(tariff.CISS) / 100);
                    var nac = cif * (decimal.Parse(tariff.NAC) / 100);
                    var levy = cif * (decimal.Parse(tariff.LEVY) / 100);

                    CF = cf;
                    CIF = cif;
                    CIFLocal = cif * ExRate;
                    IDRate = tariff.DUTY;
                    IDPayableLocal = duty * ExRate;
                    VATRate = tariff.VAT;
                    VATPayableLocal = vat * ExRate;
                    ETLRate = tariff.ETLS;
                    ETLPayableLocal = etl * ExRate;
                    SURRate = tariff.SUR;
                    SURPayableLocal = sur * ExRate;
                    CISSRate = tariff.CISS;
                    CISSPayableLocal = ciss * ExRate;
                    NACRate = tariff.NAC;
                    NACPayableLocal = nac * ExRate;
                    LEVYRate = tariff.LEVY;
                    LEVYPayableLocal = levy * ExRate;
                    TotalPayableLocal = (duty + vat + sur + etl + ciss + nac + levy) * ExRate;

                    HSCodeDescription = tariff.Description;

                }
                catch (Exception ex) { }
            }
            return Page();
        }
    }
}