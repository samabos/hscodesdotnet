using System;
using System.Collections.Generic;

namespace Autumn.UI.ViewModels
{
    public class SearchResponse
    {
        public IEnumerable<string> Error { get; set; }

        public bool Success { get; set; }

        //public List<BLResultModel> Records { get; set; }


        public Dictionary<string, List<ResultModel>> Records { get; set; }
    }
}
