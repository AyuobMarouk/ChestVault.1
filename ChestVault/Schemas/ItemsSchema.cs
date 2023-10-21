using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace ChestVault.Schemas
{
    public class ItemsSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Type { get;set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public List<string> QRcode { get; set; }
        public List<ItemInfo> Info { get; set; }
        public double SellPrice { get; set; }
        public double BoxSellPrice { get; set; }
        public double BoxSize { get; set; }

    }
}
