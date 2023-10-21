using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChestVault.Schemas
{
    public class FastSellSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Menu { get; set; }
    }
}
