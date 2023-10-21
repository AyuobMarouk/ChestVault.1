using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
namespace ChestVault.Schemas
{
    public class RecitesSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Number { get; set; }

        public int Date { get; set; }

        public string User { get; set; }
        public List<SoldItemsSchema> items { get; set; }
        public string Consumer { get; set; }
        public double Paid { get; set; }
        public double Total { get; set; }
    }
}