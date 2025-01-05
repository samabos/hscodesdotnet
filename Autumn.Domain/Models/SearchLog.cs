using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Autumn.Domain.Models
{
    public class SearchLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Keyword { get; set; }
        public string Prediction { get; set; }
        public double Rating { get; set; }
        public double Threshold { get; set; }
        public DateTime Created { get; set; }

    }
}
