using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChestVault.Schemas
{
    class NotificationsSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Message { get; set; }
        public string ItemName { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public int AlertLevel { get; set; }
    }
}
