using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChestVault.Schemas
{
    public class ActivitiesSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string User { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
    }
}
