using ChestVault.Schemas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ChestVault
{
    public partial class SellingPoint_Display : Form
    {
        public SellingPoint_Display()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }
        public DataGrid dataGrid = new DataGrid();

        public double TotalPrice;

        public double PaidPrice;
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SellingPoint_Display_Load(object sender, EventArgs e)
        {
            panel2.Controls.Add(dataGrid.DisplayForm(this));
            LoadDataGrid(new List<SoldItemsSchema>());
            timer1.Start();
        }
        public void LoadDataGrid(List<SoldItemsSchema> info)
        {

            DataGridColumn itemname = new DataGridColumn();
            itemname.LabelSize = new Size(353, 40);
            itemname.HeaderTitle = "أسم الصنف";
            itemname.dataType = DataGridColumn.DataType.Text;

            DataGridColumn price = new DataGridColumn();
            price.LabelSize = new Size(100, 40);
            price.HeaderTitle = "السعر";
            price.dataType = DataGridColumn.DataType.Double;

            DataGridColumn amount = new DataGridColumn();
            amount.LabelSize = new Size(100, 40);
            amount.HeaderTitle = "الكمية";
            amount.dataType = DataGridColumn.DataType.Double;

            DataGridColumn total = new DataGridColumn();
            total.LabelSize = new Size(200, 40);
            total.HeaderTitle = "الاجمالي";
            total.dataType = DataGridColumn.DataType.Double;

            for (int i = 0; i < info.Count; i++)
            {
                itemname.Text.Add(info[i].Name.ToString());
                price.Text.Add(info[i].SellPrice.ToString());
                amount.Text.Add(info[i].Amount.ToString());
                total.Text.Add((info[i].Amount * info[i].SellPrice).ToString());
            }

            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(itemname);
                dataGrid.Column.Add(price);
                dataGrid.Column.Add(amount);
                dataGrid.Column.Add(total);
            }
            else
            {
                dataGrid.Column[0].Text = itemname.Text;
                dataGrid.Column[1].Text = price.Text;
                dataGrid.Column[2].Text = amount.Text;
                dataGrid.Column[3].Text = total.Text;
            }

            dataGrid.ReloadDataGrid();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGrid.Selected >= 0)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("هل أنتا متأكد من مسح الصنف من الفاتورة", "مسح الصنف من الفاتورة", Controls_Dialogue.ButtonsType.SureCancel);
                if (resoult == DialogResult.OK)
                {
                    ChestVault.Me.MainForm.sellingpoint.inSellReceit[ChestVault.Me.MainForm.sellingpoint.CurrentReceit].inSellReceit.RemoveAt(dataGrid.Selected);
                    LoadDataGrid(ChestVault.Me.MainForm.sellingpoint.inSellReceit[ChestVault.Me.MainForm.sellingpoint.CurrentReceit].inSellReceit);
                    ChestVault.Me.MainForm.sellingpoint.Calculate();
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ChestVault.Me.MainForm.sellingpoint.CustomerName != "زبون عام")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                           (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }
        }
        public void ChangeCustomer()
        {
            if (ChestVault.Me.MainForm.sellingpoint.CustomerName == "زبون عام" || ChestVault.Me.MainForm.sellingpoint.CustomerName == "مسترجعات")
            {
                textBox2.Enabled = false;
                textBox2.Text = "كامل";
            }
            else
            {
                textBox2.Enabled = true;
                if (textBox2.Text == "كامل" || textBox2.Text == "") textBox2.Text = TotalPrice.ToString() ;
            }
        }

        public void ChangeTotalPrice(double price)
        {
            TotalPrice = price;

            label11.Text = price.ToString();
            label7.Text = "0";
            ChangeCustomer();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "") return;
            if (textBox2.Text == "كامل") PaidPrice = TotalPrice;
            else PaidPrice = double.Parse(textBox2.Text);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(dataGrid.Selected >= 0)
            {
                label6.Visible = true;
                label4.Visible = true;
                label2.Visible = true;
                label1.Visible = true;
                label3.Visible = true;
                label5.Visible = true;
                textBox1.Visible = true;
                button23.Visible = true;
                button22.Visible = true;
                button8.Visible = true;

                textBox1.Text = ChestVault.Me.MainForm.sellingpoint.inSellReceit[ChestVault.Me.MainForm.sellingpoint.CurrentReceit].inSellReceit[dataGrid.Selected + (dataGrid.CurrentPage * dataGrid.DisplayLimit)].Amount.ToString();
                label6.Text = ChestVault.Me.MainForm.sellingpoint.inSellReceit[ChestVault.Me.MainForm.sellingpoint.CurrentReceit].inSellReceit[dataGrid.Selected + (dataGrid.CurrentPage * dataGrid.DisplayLimit)].Name;
                label4.Text = (ChestVault.Me.MainForm.sellingpoint.inSellReceit[ChestVault.Me.MainForm.sellingpoint.CurrentReceit].inSellReceit[dataGrid.Selected + (dataGrid.CurrentPage * dataGrid.DisplayLimit)].SellPrice * ChestVault.Me.MainForm.sellingpoint.inSellReceit[ChestVault.Me.MainForm.sellingpoint.CurrentReceit].inSellReceit[dataGrid.Selected].Amount).ToString();
                label2.Text = ChestVault.Me.MainForm.sellingpoint.inSellReceit[ChestVault.Me.MainForm.sellingpoint.CurrentReceit].inSellReceit[dataGrid.Selected + (dataGrid.CurrentPage * dataGrid.DisplayLimit)].SellPrice.ToString();
            }
            else
            {
                label6.Visible = false;
                label4.Visible = false;
                label2.Visible = false;
                label1.Visible = false;
                label3.Visible = false;
                label5.Visible = false;
                textBox1.Visible = false;
                button23.Visible = false;
                button22.Visible = false;
                button8.Visible = false;
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            ChestVault.Me.MainForm.sellingpoint.PlusItem(1);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            ChestVault.Me.MainForm.sellingpoint.MinusItems(-1);
        }

        public void TurnOn()
        {
            timer1.Start();
        }
        public void TurnOff()
        {
            timer1.Stop();
        }
    }
}
