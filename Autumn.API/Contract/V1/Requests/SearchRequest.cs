using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V1.Requests
{
    public class SearchRequest
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string level { get; set; }
        public string keyword { get; set; }
    }
}
