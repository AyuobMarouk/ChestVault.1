using System.Collections.Generic;

namespace ChestVault.Schemas
{

    public class GraphAcount
    {
        /// <summary>
        ///  Adjustments
        ///  حسب الاجمالي المباع من فاتورة المبيعات و التأكد من المدفوع ليس الاجمالي
        ///  عند حساب المكسب تكون العملية ( اجمالي راس المال الفاتورة - المدفوع) تمام
        ///  الاسترجاع تمام لا تقم بتعديله
        ///  نفس الفكرة بنسبة للعميل حساب الاجمالي حسب المدفوع
        ///  نفس الفكرة بنسبة لمبيعات الاسبوع
        /// </summary>
        public List<long> Retuned = new List<long>();
        public List<long> Sold = new List<long>();
        public int CountSell = 0;
        public int CountReturn;
        public double TotalSell = 0.0;
        public double NetSell = 0.0;
        public double buySell = 0.0;
        public double TotalReturn = 0.0;
        public double NetReturn = 0.0;
        public double buyReturn = 0.0;
        public List<UsersLog> usersLogs = new List<UsersLog>();
        public List<itemslog> itemslog = new List<itemslog>();
        public WeekMid WeekMid = new WeekMid();
        public List<RemainedItemsInfo> RemainedItems = new List<RemainedItemsInfo>();


        /// Second Menu new things to calculate
        public double TotalNet; // راس الماس البضاعة الموجودة
        public double CountNet_Items; // مجموع الاصناف التي لها كمية
        public double CountNet_ItemsAmount; // مجموع كمية كافة الاصناف

        public double MidRecite;
        public double MidRecitePrice;
    }
    public class RemainedItemsInfo 
    {
        public string Name;
        public double count = 0;
    }
    public class WeekMid
    {
        public double[] Sell = { 0, 0, 0, 0, 0, 0, 0 };
        public double[] CountSell = { 0, 0, 0, 0, 0, 0, 0 };
        public double[] Return = { 0, 0, 0, 0, 0, 0, 0 };
        public double[] CountReturn = { 0, 0, 0, 0, 0, 0, 0 };
    }
    public class UsersLog
    {
        public string Name;
        public double TotalSell = 0.0;
        public double NetSell = 0.0;
        public double buySell = 0.0;
        public double TotalReturn = 0.0;
        public double NetReturn = 0.0;
        public double buyReturn = 0.0;
    }

    public class itemslog
    {
        public string Name;
        public double AmountSell = 0;
        public double AmountReturn = 0;
        public double TotalSell = 0.0;
        public double TotalReturn = 0.0;
    }
}
