using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace ChestVault.Schemas
{
    public class UsersSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Accessibility { get; set; }
        public List<SavedColors> UserColor { get; set; }
    }
}
