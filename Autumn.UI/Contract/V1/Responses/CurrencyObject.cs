using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.UI.Contract.V1.Responses
{
    public class CurrencyObject
    {
        public string CurrencyCode { get; set; }
        public string Rate { get; set; }
        public string TimeStamp { get; set; }
    }
}
