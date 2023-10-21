using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using System.Collections.Generic;
using System;
namespace ChestVault.Schemas
{
    public class WorkSchedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public string User { get; set; }
        public List<int> RecitesSold { get; set; }
        public List<int> RecitesBougth { get; set; }

        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public bool Open { get; set; }
    }
}
