using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V1.Responses
{
    public class DutyResponse
    {
        public IEnumerable<string> Error { get; set; }
        public bool Success { get; set; }

        public string ProductDesc { get; set; }
        public string HSCode { get; set; }
        public decimal Cost { get; set; }
        public decimal Freight { get; set; }
        public decimal Insurance { get; set; }
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

    }
}
