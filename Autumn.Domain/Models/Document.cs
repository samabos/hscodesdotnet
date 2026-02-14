using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Autumn.Domain.Models
{
    [BsonIgnoreExtraElements]
    public partial class Document
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
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
    }
}
