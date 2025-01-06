using System;
namespace Autumn.BL.Models.Request.V2
{
    public class BLSearchRequest
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string level { get; set; }
        public string keyword { get; set; }
        public string settings { get; set; }
    }
}
