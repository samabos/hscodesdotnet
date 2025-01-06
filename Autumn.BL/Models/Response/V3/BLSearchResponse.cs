﻿using System;
using System.Collections.Generic;

namespace Autumn.BL.Models.Response.V3
{
    public class BLSearchResponse
    {
        public IEnumerable<string> Error { get; set; }

        public bool Success { get; set; }

        //public List<BLResultModel> Records { get; set; }


        public Dictionary<string, List<BLResultModel>> Records {get; set;}


    }
}
