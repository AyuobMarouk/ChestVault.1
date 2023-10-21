using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChestVault.Schemas
{
    public class DeptSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DeptInfo> Info { get; set; }
        public string Title { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public double Dept { get; set; }
    }
}