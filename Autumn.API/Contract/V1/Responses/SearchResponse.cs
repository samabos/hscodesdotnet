using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V1.Responses
{
    public class SearchResponse
    {
        
        public IEnumerable<string> Error { get; set; }

        public bool Success { get; set; }

        public List<HSCodeObject> Records { get; set; }
    }

}
