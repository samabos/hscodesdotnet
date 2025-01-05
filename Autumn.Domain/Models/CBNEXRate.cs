using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Models
{
    public class CBNEXRate
    {
        [Name("Currency")]
        public string Currency  { get; set; }
        [Name("Rate Date")]
        public string RateDate { get; set; }
        [Name("Rate Year")]
        public string RateYear { get; set; }
        [Name("Rate Month")]
        public string RateMonth { get; set; }
        [Name("Buying Rate")]
        public string BuyingRate { get; set; }
        [Name("Central Rate")]
        public string CentralRate { get; set; }
        [Name("Selling Rate")]
        public string SellingRate { get; set; }
    }
}
