using System;

namespace ChestVault.Schemas
{
    public class ItemHistory
    {
        public string Name { get; set; }
        public string Supplier { get; set; }
        public int Date { get; set; }
        public int Number { get; set; }
        public double Buy { get; set; }
        public double Sell { get; set; }

    }
}
