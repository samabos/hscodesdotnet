using System;
using System.Threading.Tasks;
using Autumn.BL.Models.Request.V2;
using Autumn.BL.Models.Response.V2;

namespace Autumn.BL.Interface.V2
{
    public interface IClassification
    {
         Task<BLSearchResponse> SearchAsync(BLSearchRequest request);
    }
}
