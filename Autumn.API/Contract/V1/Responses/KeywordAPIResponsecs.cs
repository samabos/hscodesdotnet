using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V1.Responses
{
    public class KeywordAPIResponsecs
    {
            public string status { get; set; }
            public string message { get; set; }
            public string queryPrefix { get; set; }
            public string fullQuery { get; set; }
            public string query { get; set; }
            public string type { get; set; }
            public List<Terms> results { get; set; }
        
    }
    public class Terms
    {
        public string term { get; set; }
    }
}
