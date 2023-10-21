using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChestVault.Schemas
{
    public class BoughtItemsSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpDate { get; set; }
        public double BuyPrice { get; set; }
        public double Amount { get; set; }
        public double SellPrice { get; set; }
    }
}
