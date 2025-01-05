using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V1.Responses
{
    public class ClassifyCommodityResponse
    {

        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public string HSCode { get; set; }
        public string Accuracy { get; set; }
        public HSCodeTariff Record { get; set; }
    }
    public class HSCodeTariff
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Duty { get; set; }
        public string Levy { get; set; }
        public string VAT { get; set; }
        public string NAC { get; set; }
        public string SUR { get; set; }
        public string ETL { get; set; }
        public string CIS { get; set; }

    }
}
