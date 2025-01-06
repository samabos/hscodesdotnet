using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V2.Responses
{
    public class SearchResponse
    {

        public IEnumerable<string> Error { get; set; }

        public bool Success { get; set; }

        public List<ResultModel> Records { get; set; }
        public bool ai { get; set; }
    }

}
