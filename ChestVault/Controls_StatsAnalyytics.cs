using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChestVault;
using ChestVault.Schemas;
namespace ChestVault
{
    public partial class Controls_StatsAnalyytics : Form
    {
        public Controls_StatsAnalyytics()
        {
            InitializeComponent();

            ChestVault.Me.ChangeDesign(this);
        }

        CRUD db = new CRUD();
        DataGrid dataGrid = new DataGrid();

        public void LoadData(List<RecitesSchema> Recites)
        {
            label2.Text = "عدد الفواتير :" + Recites.Count.ToString();

            double totalamountofitem = 0;
            double TotalPrice = 0;
            double paid = 0;
            double headmoney = 0;
            int itemsamount = 0;
            for (int i = 0; i < Recites.Count; i++)
            {
                foreach (SoldItemsSchema a in Recites[i].items)
                {
                    totalamountofitem += a.Amount;
                    if (a.info == null) a.info = new List<SoldInfoSchema>();
                    foreach(SoldInfoSchema s in a.info)
                    {
                        headmoney += s.Amount * s.BuyPrice;
                    }
                }
                itemsamount += Recites[i].items.Count;
                TotalPrice += Recites[i].Total;
                paid += Recites[i].Paid;
            }

            label3.Text = "عدد القطع الاجمالية :" + totalamountofitem.ToString();
            label7.Text = "عدد الاصناف :" + Recites.Count.ToString();

            label4.Text = "السعر الاجمالي للفواتير :" + ((int)(TotalPrice * 100) / 100.0).ToString();
            label8.Text = "المدفوع من الفواتير :" + ((int)(paid * 100) / 100.0).ToString();

            label6.Text = "رأس المال :" + ((int)(headmoney * 100) / 100.0).ToString();

            double profit = TotalPrice - headmoney;
            label5.Text = "المكسب : " + ((int)(profit * 100) / 100.0).ToString();
            LoadDataGrid(Recites);
        }
        public void LoadDataGrid(List<RecitesSchema> info)
        {
            DataGridColumn Itemname = new DataGridColumn();
            Itemname.LabelSize = new Size(276, 40);
            Itemname.HeaderTitle = "أسم الصنف";
            Itemname.dataType = DataGridColumn.DataType.Text;

            DataGridColumn SellPrice = new DataGridColumn();
            SellPrice.LabelSize = new Size(100, 40);
            SellPrice.HeaderTitle = "سعر البيع";
            SellPrice.dataType = DataGridColumn.DataType.Double;

            DataGridColumn amount = new DataGridColumn();
            amount.LabelSize = new Size(100, 40);
            amount.HeaderTitle = "الكمية";
            amount.dataType = DataGridColumn.DataType.Double;

            DataGridColumn OriginalMoney = new DataGridColumn();
            OriginalMoney.LabelSize = new Size(150, 40);
            OriginalMoney.HeaderTitle = "رأس المال";
            OriginalMoney.dataType = DataGridColumn.DataType.Double;

            DataGridColumn TotalPrice = new DataGridColumn();
            TotalPrice.LabelSize = new Size(150, 40);
            TotalPrice.HeaderTitle = "الاجمالي";
            TotalPrice.dataType = DataGridColumn.DataType.Double;

            for (int i = 0; i < info.Count; i++)
            {
                for (int x = 0; x < info[i].items.Count; x++)
                {
                    bool adding = true;
                    for (int j = 0; j < Itemname.Text.Count; j++)
                    {
                        if (info[i].items[x].Name == Itemname.Text[j])
                        {
                            amount.Text[j] = (double.Parse(amount.Text[j]) + info[i].items[x].Amount).ToString();

                            double paying = double.Parse(OriginalMoney.Text[j]);
                            if (info[i].items[x].info == null) info[i].items[x].info = new List<SoldInfoSchema>();
                            foreach (SoldInfoSchema a in info[i].items[x].info)
                            {
                                paying += a.Amount * a.BuyPrice;
                            }

                            TotalPrice.Text[j] = (double.Parse(TotalPrice.Text[j]) + info[i].items[x].Total).ToString();
                            adding = false;
                            break;
                        }
                    }
                    if (adding == false) continue;

                    Itemname.Text.Add(info[i].items[x].Name);
                    SellPrice.Text.Add(info[i].items[x].SellPrice.ToString());
                    amount.Text.Add(info[i].items[x].Amount.ToString());

                    double paid = 0;
                    foreach (SoldInfoSchema a in info[i].items[x].info)
                    {
                        paid += a.Amount * a.BuyPrice;
                    }
                    OriginalMoney.Text.Add(paid.ToString());
                    TotalPrice.Text.Add(info[i].items[x].Total.ToString());

                }
            }

            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(Itemname);
                dataGrid.Column.Add(SellPrice);
                dataGrid.Column.Add(amount);
                dataGrid.Column.Add(OriginalMoney);
                dataGrid.Column.Add(TotalPrice);
            }
            else
            {
                dataGrid.Column[0].Text = Itemname.Text;
                dataGrid.Column[1].Text = SellPrice.Text;
                dataGrid.Column[2].Text = amount.Text;
                dataGrid.Column[3].Text = OriginalMoney.Text;
                dataGrid.Column[4].Text = TotalPrice.Text;
            }

            dataGrid.ReloadDataGrid();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ChestVault.Me.SoldRecitesWindow.Enabled = true;
            this.Hide();
        }
        private void Controls_StatsAnalyytics_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
        }

        private void Controls_StatsAnalyytics_Load(object sender, EventArgs e)
        {
            panel2.Controls.Add(dataGrid.DisplayForm(this));
        }
    }
}
