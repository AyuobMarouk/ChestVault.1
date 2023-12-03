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

        public List<RecitesSchema> schemas;
        public void LoadData(List<RecitesSchema> Recites , string Customername)
        {
            label11.Text = Customername;
            label2.Text = Recites.Count.ToString();

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

            label4.Text = ((int)(TotalPrice * 100) / 100.0).ToString();

            label8.Text = ((int)(paid * 100) / 100.0).ToString();

            double remaining = TotalPrice - paid;
            label6.Text = ((int)(remaining * 100) / 100.0).ToString();

            if (Customername != "مختلط" && Customername != "زبون عام" && Customername != "مسترجعات" && remaining > 0) button1.Visible = true;
            else button1.Visible = false;

            schemas = Recites;
            LoadDataGrid(Recites);
        }
        public void LoadDataGrid(List<RecitesSchema> info)
        {
            dataGrid.SelectAble = false;
            DataGridColumn Itemname = new DataGridColumn();
            Itemname.LabelSize = new Size(300, 40);
            Itemname.HeaderTitle = "أسم الصنف";
            Itemname.dataType = DataGridColumn.DataType.Text;

            DataGridColumn SellPrice = new DataGridColumn();
            SellPrice.LabelSize = new Size(100, 40);
            SellPrice.HeaderTitle = "سعر البيع";
            SellPrice.dataType = DataGridColumn.DataType.Double;

            DataGridColumn amount = new DataGridColumn();
            amount.LabelSize = new Size(150, 40);
            amount.HeaderTitle = "الكمية";
            amount.dataType = DataGridColumn.DataType.Double;

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
                    TotalPrice.Text.Add(info[i].items[x].Total.ToString());

                }
            }

            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(Itemname);
                dataGrid.Column.Add(SellPrice);
                dataGrid.Column.Add(amount);
                dataGrid.Column.Add(TotalPrice);
            }
            else
            {
                dataGrid.Column[0].Text = Itemname.Text;
                dataGrid.Column[1].Text = SellPrice.Text;
                dataGrid.Column[2].Text = amount.Text;
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

        private async void button1_Click(object sender, EventArgs e)
        {
            DialogResult resoult = ChestVault.Me.InputField("القيمة مراد دفعها", InputField.InputFieldType.Double);
            if (resoult == DialogResult.Cancel) return;

            double PayedAmount = double.Parse(ChestVault.Me.InputFieldWindow);
            if (PayedAmount == 0) return;
            if(PayedAmount > double.Parse(label6.Text))
            {
                ChestVault.Me.MessageBox("المدفوع أكبر من المطلوب", "طلب غير مقبول", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            for (int i = 0; i < schemas.Count; i++)
            {
                if (PayedAmount == 0) break;
                double remaining = schemas[i].Total - schemas[i].Paid;

                if (remaining == 0) continue;
                if (PayedAmount > remaining)
                {
                    schemas[i].Paid += remaining;
                    PayedAmount -= remaining;

                     await db.UpdateSoldRecite(schemas[i]);

                    continue;
                }
                else
                {
                    schemas[i].Paid += PayedAmount;
                    PayedAmount = 0;

                    await db.UpdateSoldRecite(schemas[i]);

                    continue;
                }
            }

            ChestVault.Me.MessageBox("تم دفع الفواتير", "تم حفظ التغيرات", Controls_Dialogue.ButtonsType.Ok);
            ChestVault.Me.SoldRecitesWindow.Enabled = true;
            this.Hide();

        }
    }
}
