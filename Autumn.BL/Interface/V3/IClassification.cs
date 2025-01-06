using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autumn.BL.Models.Request.V3;
using Autumn.BL.Models.Response.V3;

namespace Autumn.BL.Interface.V3
{
    public interface IClassification
    {
         Task<BLSearchResponse> SearchAsync(BLSearchRequest request);
        Dictionary<string, float> GetHSCode(string product, double threshold);
    }
}
