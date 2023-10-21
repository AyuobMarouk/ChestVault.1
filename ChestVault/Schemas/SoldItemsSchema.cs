using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ChestVault.Schemas
{
    public class SoldItemsSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public double SellPrice { get; set; }
        public List<SoldInfoSchema> info { get; set; }
        public int Amount { get; set; }
        public double Total { get; set; }

        public double Storage;
    }
}
