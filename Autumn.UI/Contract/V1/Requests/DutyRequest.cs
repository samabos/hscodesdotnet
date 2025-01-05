using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.UI.Contract.V1.Requests
{
    public class DutyRequest
    {
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
    }
}
