using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Models
{
    public class HSCode
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PId { get; set; }

        public long Order { get; set; }
        public long Level { get; set; }

        //[BsonElement("Code")]
        public string Id { get; set; }

        //[BsonElement("Parent Code")]
        public string ParentId { get; set; }

        //[BsonElement("HS Code")]
        public string Code { get; set; }

      //  [BsonElement("Parent HS Code")]
        public string ParentCode { get; set; }
        public string Description { get; set; }
        public string SelfExplanatory { get; set; }
    }
}
