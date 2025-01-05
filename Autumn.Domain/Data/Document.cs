using System;
using System.Collections.Generic;

namespace Autumn.Domain.Data
{
    public partial class Document
    {
        public int? Level { get; set; }
        public string Parent { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Issuer { get; set; }
        public string Validity { get; set; }
        public string DurationForIssue { get; set; }
        public string ApplicationForm { get; set; }
        public string InspectionFee { get; set; }
        public string PermitNew { get; set; }
        public string PermitRenewal { get; set; }
        public string LateRenewal { get; set; }
        public string PnsupportingDocument { get; set; }
        public string PrsupportingDocument { get; set; }
        public string Remark { get; set; }
        public int Id { get; set; }
    }
}
