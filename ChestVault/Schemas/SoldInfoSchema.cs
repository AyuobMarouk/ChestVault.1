using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChestVault.Schemas
{
    public class SoldInfoSchema
    {
        public double BuyPrice { get; set; }
        public double Amount { get; set; }
    }
}
