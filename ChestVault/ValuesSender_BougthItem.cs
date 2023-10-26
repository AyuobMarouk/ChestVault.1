using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChestVault.Schemas;

namespace ChestVault
{
    public partial class ValuesSender_BougthItem : Form
    {
        public ValuesSender_BougthItem()
        {
            InitializeComponent();

            ChestVault.Me.ChangeDesign(this);
        }
        public BoughtItemsSchema NewItem = new BoughtItemsSchema();
        public Controls_Buy_Recite reciteForms;
        CRUD db = new CRUD();
        private void button1_Click(object sender, EventArgs e)
        {
            ChestVault.Me.BuyNewRecite.Enabled = true;
            this.Hide();
        }

        private void ValuesSender_BougthItem_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "شراء قطع";
        }
        public async void LoadData(BoughtItemsSchema Item,Controls_Buy_Recite form)
        {
            List<ItemsSchema> item = await db.GetItem(Item.Name);
            textBox1.Text = item[0].SellPrice.ToString();

            double total = 0;

            if (item[0].Info != null && item[0].Info.Count > 0)
            {
                foreach (ItemInfo a in item[0].Info)
                {
                    total += a.Amount;
                }
            }

            label6.Text = "المخزون المتوفر :" + total.ToString();

            label4.Text = "الصندوق يحتوي علي :" + item[0].BoxSize.ToString();
            reciteForms = form;
            textBox2.Text = Item.Name;
            textBox4.Text = Item.Amount.ToString();
            textBox7.Text = Item.BuyPrice.ToString();

            textBox1.Text = item[0].SellPrice.ToString();

            Item.SellPrice = item[0].SellPrice;

            dateTimePicker1.Value = Item.ExpDate;
            if (dateTimePicker1.Value == dateTimePicker1.MaxDate) checkBox1.Checked = false;

            NewItem = Item;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "شراء صندوق")
            {
                
                List<ItemsSchema> item = await db.GetItem(NewItem.Name);

                if(item[0].BoxSize == 0)
                {
                    DialogResult resoult = ChestVault.Me.MessageBox("لا يمكنك الشراء عن طريق الصندوق إذا كانت كمية الصندوق 0", "شراء", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }
                NewItem.Amount = (textBox4.Text != "")?(double.Parse(textBox4.Text) * item[0].BoxSize) : 0;
                NewItem.BuyPrice = (textBox7.Text != "")?(double.Parse(textBox7.Text) / item[0].BoxSize) : 0;
            }
            else
            {
                NewItem.Amount = double.Parse(textBox4.Text);
                NewItem.BuyPrice = double.Parse(textBox7.Text);
            }
            NewItem.ExpDate = (checkBox1.Checked)? dateTimePicker1.Value : DateTime.MaxValue;
            NewItem.SellPrice = (textBox1.Text != "") ? (double.Parse(textBox1.Text)) : 0;

            ChestVault.Me.BuyNewRecite.AddNewItemToRecite(NewItem);
            ChestVault.Me.BuyNewRecite.Enabled = true;
            ChestVault.Me.BuyNewRecite.LoadDataGrid();
            this.Hide();

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                       (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = checkBox1.Checked;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            ChestVault.Me.BuyNewRecite.RemoveItem(NewItem.Name);
            ChestVault.Me.BuyNewRecite.Enabled = true;
            this.Hide();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                      (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}
