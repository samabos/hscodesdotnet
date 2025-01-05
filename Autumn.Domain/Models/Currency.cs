using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Autumn.Domain.Models
{
    public class Currency
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CurrencyCode { get; set; }
        public double Rate { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
