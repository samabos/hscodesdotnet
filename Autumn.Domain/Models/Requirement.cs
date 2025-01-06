using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Autumn.Domain.Models
{
    public class Requirement
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Agency { get; set; }
        public string Department { get; set; }
        public string HSCode { get; set; }
        public string HSCodeLocal { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImportGuidelines { get; set; }
        public string Forms { get; set; }
        public string DocumentsDelivered { get; set; }
        public string EstimatedTime { get; set; }
        public string FormCost { get; set; }
        public string ProductCost { get; set; }
        public string InspectionAnalysis { get; set; }
        public string RenewalCost { get; set; }
        public string RenewalDuration { get; set; }
    }
}
