using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.UI.Contract.V1.Responses
{

    public class HSCodeObject
    {
        public string PId { get; set; }

        public long Order { get; set; }
        public long Level { get; set; }

        public string Id { get; set; }

        public string ParentId { get; set; }

        public string Code { get; set; }

        public string ParentCode { get; set; }
        public string Description { get; set; }
        public string SelfExplanatory { get; set; }

    }
}
