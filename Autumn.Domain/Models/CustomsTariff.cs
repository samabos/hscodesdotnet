using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Models
{
    [BsonIgnoreExtraElements]
    public class CustomsTariff
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Country { get; set; }
        public string Header { get; set; }
        public string HSCode { get; set; }
        public string Description { get; set; }
        // Common
        public string DUTY { get; set; }
        public string VAT { get; set; }
        public string LEVY { get; set; }
        // Nigeria
        public string NAC { get; set; }
        public string SUR { get; set; }
        public string ETLS { get; set; }
        public string CISS { get; set; }
        // Ghana
        public string NHIL { get; set; }
        public string GETFUND { get; set; }
        // Kenya
        public string IDF { get; set; }
        public string RDF { get; set; }
    }
}
