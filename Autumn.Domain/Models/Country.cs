using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Autumn.Domain.Models
{
    public class Country
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Currency { get; set; }
        public string Symbol { get; set; }
    }
}
