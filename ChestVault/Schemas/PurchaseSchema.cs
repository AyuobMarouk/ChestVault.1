using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using System.Collections.Generic;
namespace ChestVault.Schemas
{
    public class PurchaseSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Number { get; set; }

        public int Date { get; set; }

        public List<BoughtItemsSchema> Items { get; set; }

        public double Total { get; set; }
        public double Paid { get; set; }
        public double Dept { get; set; }

        public string User { get; set; }
        public string supplier { get; set; }
    }
}