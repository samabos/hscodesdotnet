using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Models
{
    public class CustomsTariff
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Header { get; set; }
        public string HSCode { get; set; }
        public string Description { get; set; }
        public string DUTY { get; set; }
        public string LEVY { get; set; }
        public string VAT { get; set; }
        public string NAC { get; set; }
        public string SUR { get; set; }
        public string ETLS { get; set; }
        public string CISS { get; set; }
    }
}
