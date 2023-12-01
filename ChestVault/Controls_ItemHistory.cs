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
    public partial class Controls_ItemHistory : Form
    {
        public Controls_ItemHistory()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }

        CRUD db = new CRUD();
        DataGrid dataGrid = new DataGrid();
        SearchMenu SearchingMenu = new SearchMenu();
        List<ItemHistory> history = new List<ItemHistory>();
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


        public async void SearchForName(string ItemName)
        {
            List<ItemsSchema> list = await db.GetItem(ItemName);
            if(list.Count == 0)
            {
                ChestVault.Me.MessageBox("هذا الصنف غير موجود", "مفقود", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            history = new List<ItemHistory>();

            history = await db.GetPurchesByName(ItemName);

            MessageBox.Show(history.Count.ToString()  + "   " + ItemName);
            LoadDataGrid();

        }

        public void LoadDataGrid()
        {

            DataGridColumn supplier = new DataGridColumn();
            supplier.LabelSize = new Size(200, 40);
            supplier.HeaderTitle = "المسوق";
            supplier.dataType = DataGridColumn.DataType.Text;

            DataGridColumn number = new DataGridColumn();
            number.LabelSize = new Size(100, 40);
            number.HeaderTitle = "رقم الفاتورة";
            number.dataType = DataGridColumn.DataType.Double;

            DataGridColumn Buyprice = new DataGridColumn();
            Buyprice.LabelSize = new Size(150, 40);
            Buyprice.HeaderTitle = "سعر الشراء";
            Buyprice.dataType = DataGridColumn.DataType.Double;

            DataGridColumn sellprice = new DataGridColumn();
            sellprice.LabelSize = new Size(150, 40);
            sellprice.HeaderTitle = "سعر البيع";
            sellprice.dataType = DataGridColumn.DataType.Double;

            DataGridColumn date = new DataGridColumn();
            date.LabelSize = new Size(100, 40);
            date.HeaderTitle = "تاريخ الشراء";
            date.dataType = DataGridColumn.DataType.Text;

            for (int i = 0; i < history.Count; i++)
            {
                supplier.Text.Add(history[i].Supplier.ToString());
                number.Text.Add(history[i].Number.ToString());
                Buyprice.Text.Add(history[i].Buy.ToString());
                sellprice.Text.Add(history[i].Sell.ToString());
                date.Text.Add(history[i].Date.ToString());
            }

            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(supplier);
                dataGrid.Column.Add(number);
                dataGrid.Column.Add(Buyprice);
                dataGrid.Column.Add(sellprice);
                dataGrid.Column.Add(date);
            }
            else
            {
                dataGrid.Column[0].Text = supplier.Text;
                dataGrid.Column[1].Text = number.Text;
                dataGrid.Column[2].Text = Buyprice.Text;
                dataGrid.Column[3].Text = sellprice.Text;
                dataGrid.Column[4].Text = date.Text;
            }

            dataGrid.ReloadDataGrid();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {

        }

        private void Controls_ItemHistory_Load(object sender, EventArgs e)
        {
            Point location = this.PointToScreen(textBox1.Location);
            SearchingMenu.Setup(location, textBox1.Size, this);


            panel1.Controls.Add(dataGrid.DisplayForm(this));
            LoadDataGrid();
        }
    }
}
