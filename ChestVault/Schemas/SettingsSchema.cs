using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace ChestVault.Schemas
{
    public class SettingsSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int NotificationAmount { get; set; }
        public DateTime SignUpDate { get; set; }

        public string ShopName { get;set; }
        public string LogoURL;
    }
}