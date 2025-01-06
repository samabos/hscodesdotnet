using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autumn.Domain.Models;

namespace Autumn.UI.ViewModels
{
    public class ResultModel
    {
        public List<HSCode> HSCodes { get; internal set; }
        public string Prediction { get; internal set; }
        public float Rating { get; internal set; }
        public List<string> Tags { get; internal set; }
        public string Code { get; internal set; }
        public List<HSCode> PHSCodes { get; internal set; }
    }
}
