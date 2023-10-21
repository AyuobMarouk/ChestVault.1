using System;
namespace ChestVault.Schemas
{
    public class DeptInfo
    {
        public DateTime Date { get; set; }
        public string User { get; set; }
        public double Amount { get; set; }
        public double Paid { get; set; }
        public double Dept { get; set; }
        public string Type { get; set; }
    }
}