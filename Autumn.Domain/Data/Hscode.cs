using System;
using System.Collections.Generic;

namespace Autumn.Domain.Data
{
    public partial class Hscode
    {
        public int? Order { get; set; }
        public int? Level { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string Parent { get; set; }
        public string Description { get; set; }
        public string SelfExplanatoryEnglish { get; set; }
        public string SelfExplanatoryFrench { get; set; }
        public string SelfExplanatoryGerman { get; set; }
        public int Pid { get; set; }
    }
}
