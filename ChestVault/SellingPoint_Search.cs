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
using static ChestVault.Controls_SellingPoint;

namespace ChestVault
{
    public partial class SellingPoint_Search : Form
    {
        public SellingPoint_Search()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }

        CRUD db = new CRUD();

        public List<ItemsSchema> SearchedItems = new List<ItemsSchema>();

        public DataGrid dataGrid = new DataGrid();

        private string Type;
        private bool ItemName;
        public void LoadDataGrid()
        {

            DataGridColumn itemname = new DataGridColumn();
            itemname.LabelSize = new Size(374, 40);
            itemname.HeaderTitle = "أسم الصنف";
            itemname.dataType = DataGridColumn.DataType.Text;

            DataGridColumn price = new DataGridColumn();
            price.LabelSize = new Size(200, 40);
            price.HeaderTitle = "سعر القطعة";
            price.dataType = DataGridColumn.DataType.Double;

            DataGridColumn BoxPrice = new DataGridColumn();
            BoxPrice.LabelSize = new Size(200, 40);
            BoxPrice.HeaderTitle = "سعر الصندوق";
            BoxPrice.dataType = DataGridColumn.DataType.Double;

            for (int i = 0; i < SearchedItems.Count; i++)
            {
                    itemname.Text.Add(SearchedItems[i].Name.ToString());
                    price.Text.Add(SearchedItems[i].SellPrice.ToString());
                    BoxPrice.Text.Add(SearchedItems[i].BoxSellPrice.ToString());
            }
            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(itemname);
                dataGrid.Column.Add(price);
                dataGrid.Column.Add(BoxPrice);
            }
            else
            {
                dataGrid.Column[0].Text = itemname.Text;
                dataGrid.Column[1].Text = price.Text;
                dataGrid.Column[2].Text = BoxPrice.Text;
            }

            dataGrid.ReloadDataGrid();
        }

        private async void SellingPoint_Search_Load(object sender, EventArgs e)
        {
            panel1.Controls.Add(dataGrid.DisplayForm(this));

            SearchedItems = await db.GetItemby(null, null, null);

            LoadDataGrid();


        }

        public async void ChangeCatagory(object sender, EventArgs e)
        {
            Type = (sender as Button).AccessibleDescription;

            SearchedItems = await db.GetItemby((Type == "") ? null : Type, ((ItemName)? ((textBox2.Text == "")? null : textBox2.Text) : null), (ItemName) ? null : ((textBox2.Text == "") ? null : textBox2.Text));
            LoadDataGrid();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dataGrid.Selected >= 0)
            {
                if (dataGrid.Selected > SearchedItems.Count) return;
                label1.Text = SearchedItems[dataGrid.Selected].Name.ToString();
                double sum = 0;
                foreach (var item in SearchedItems[dataGrid.Selected].Info)
                {
                    sum += item.Amount;
                }
                label2.Text = sum.ToString();

                label1.Visible = true;
                label2.Visible = true;
            }
        }

        public void TurnOn()
        {
            timer1.Start();
        }
        public void TurnOff()
        {
            timer1.Stop();
        }

        private async void comboBox1_TextChanged(object sender, EventArgs e)
        {
            List<FastSellSchema> itemsBuFastSell = await db.GetFastSellByMenu(comboBox1.Text);

            SearchedItems = new List<ItemsSchema>();

            foreach (var item in itemsBuFastSell)
            {
                List<ItemsSchema> items = new List<ItemsSchema>();
                items = await db.GetItemby((Type == "")? null : Type, item.Name,null);
                SearchedItems.Add(items[0]);
            }
            LoadDataGrid();
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            ItemName = true;
            if (textBox2.Text != "")
            {
                SearchedItems = await db.GetItemby((Type == "") ? null : Type, textBox2.Text, null);
                LoadDataGrid();
            }
        }
        private async void button9_Click(object sender, EventArgs e)
        {
            ItemName = false;
            if (textBox2.Text != "")
            {
                SearchedItems = await db.GetItemby((Type == "") ? null : Type, null, textBox2.Text);
                LoadDataGrid();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FastSell_Settings form = new FastSell_Settings();
            form.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (dataGrid.Selected < 0)
            {
                ChestVault.Me.MessageBox("يرجي اختيار صنف للاضافة", "بحث", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            if(int.Parse(label2.Text) <=0)
            {
                ChestVault.Me.MessageBox("لا يوجد مخزون", "بحث", Controls_Dialogue.ButtonsType.Ok);
                return;
            }


            ChestVault.Me.MainForm.sellingpoint.SearchItem(SearchedItems[dataGrid.Selected].Name, 1);

            ChestVault.Me.MainForm.sellingpoint.CurrentState = FormState.SellingPoint;
            ChestVault.Me.MainForm.sellingpoint.FillMainPanel();
            ChestVault.Me.MainForm.sellingpoint.sellingPoint.TurnOn();
            ChestVault.Me.MainForm.sellingpoint.SearchPoint.TurnOff();

            ChestVault.Me.MainForm.sellingpoint.sellingPoint.LoadDataGrid(ChestVault.Me.MainForm.sellingpoint.inSellReceit[ChestVault.Me.MainForm.sellingpoint.CurrentReceit].inSellReceit);
        }
    }
}
