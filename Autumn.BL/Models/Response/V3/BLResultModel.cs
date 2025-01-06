using System.Collections.Generic;
using Autumn.Domain.Models;

namespace Autumn.BL.Models.Response.V3
{
    public class BLResultModel
    {
        public List<HSCode> HSCodes { get; internal set; }
        public string Prediction { get; internal set; }
        public float Rating { get; internal set; }
        public List<string> Tags { get; internal set; }
        public string Code { get; internal set; }
        public List<HSCode> PHSCodes { get; internal set; }
    }
}