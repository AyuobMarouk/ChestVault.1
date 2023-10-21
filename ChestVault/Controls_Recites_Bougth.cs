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
    public partial class Controls_Recites_Bougth : Form
    {
        public Controls_Recites_Bougth()
        {
            InitializeComponent();

            ChestVault.Me.ChangeDesign(this);
        }
        CRUD db = new CRUD();
        DataGrid dataGrid = new DataGrid();

        public List<PurchaseSchema> Recite_info = new List<PurchaseSchema>();
        private void Controls_Recites_Bougth_Load(object sender, EventArgs e)
        {
            panel2.Controls.Add(dataGrid.DisplayForm(this));

            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            LoadComboBox();
            LoadDataGrid(new List<PurchaseSchema>());
        }
        public void LoadDataGrid(List<PurchaseSchema> info)
        {

            Recite_info = info;

            DataGridColumn supplier = new DataGridColumn();
            supplier.LabelSize = new Size(200, 40);
            supplier.HeaderTitle = "الموزع";
            supplier.dataType = DataGridColumn.DataType.Text;

            DataGridColumn number = new DataGridColumn();
            number.LabelSize = new Size(100, 40);
            number.HeaderTitle = "رقم";
            number.dataType = DataGridColumn.DataType.Double;

            DataGridColumn amount = new DataGridColumn();
            amount.LabelSize = new Size(150, 40);
            amount.HeaderTitle = "عدد الاصناف";
            amount.dataType = DataGridColumn.DataType.Double;

            DataGridColumn peritem = new DataGridColumn();
            peritem.LabelSize = new Size(150, 40);
            peritem.HeaderTitle = "عدد القطع";
            peritem.dataType = DataGridColumn.DataType.Double;

            DataGridColumn state = new DataGridColumn();
            state.LabelSize = new Size(100, 40);
            state.HeaderTitle = "الحالة";
            state.dataType = DataGridColumn.DataType.Text;

            DataGridColumn recitedate = new DataGridColumn();
            recitedate.LabelSize = new Size(150, 40);
            recitedate.HeaderTitle = "تاريخ الفاتورة";
            recitedate.dataType = DataGridColumn.DataType.Date;

            DataGridColumn user = new DataGridColumn();
            user.LabelSize = new Size(150, 40);
            user.HeaderTitle = "المستخدم";
            user.dataType = DataGridColumn.DataType.Text;

            for(int i = 0; i < info.Count; i++) { 
                supplier.Text.Add(info[i].supplier.ToString());
                number.Text.Add(info[i].Number.ToString());
                amount.Text.Add(info[i].Items.Count.ToString());
                double sum = 0;
                foreach (BoughtItemsSchema a in info[i].Items)
                {
                    sum += a.Amount;
                }
                peritem.Text.Add(sum.ToString());
                state.Text.Add((info[i].Total - info[i].Paid == 0) ? "مغلقة" : "مفتوحة");
                recitedate.Text.Add(info[i].Date.ToString());
                user.Text.Add(info[i].User);
            }

            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(supplier);
                dataGrid.Column.Add(number);
                dataGrid.Column.Add(amount);
                dataGrid.Column.Add(peritem);
                dataGrid.Column.Add(state);
                dataGrid.Column.Add(recitedate);
                dataGrid.Column.Add(user);
            }
            else
            {
                dataGrid.Column[0].Text = supplier.Text;
                dataGrid.Column[1].Text = number.Text;
                dataGrid.Column[2].Text = amount.Text;
                dataGrid.Column[3].Text = peritem.Text;
                dataGrid.Column[4].Text = state.Text;
                dataGrid.Column[5].Text = recitedate.Text;
                dataGrid.Column[6].Text = user.Text;
            }

            if (dataGrid.Column[0].Label.Count > 0) dataGrid.ReloadDataGrid();
            else dataGrid.FormDesign();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChestVault.Me.MainForm.FillNewWindow(ChestVault.Me.MainForm.SidePanel);
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            switch(comboBox1.Text)
            {
                case "الكل":
                    panel4.Visible = false;
                    panel5.Visible = false;
                    panel6.Visible = false;
                    panel7.Visible = false;
                    break;
                case "حسب الموزع":
                    panel4.Visible = true;
                    panel5.Visible = false;
                    panel6.Visible = false;
                    panel7.Visible = false;
                    break;
                case "حسب التاريخ":
                    panel4.Visible = false;
                    panel5.Visible = true;
                    panel6.Visible = false;
                    panel7.Visible = false;
                    break;
                case "حسب الحالة":
                    panel4.Visible = false;
                    panel5.Visible = false;
                    panel6.Visible = true;
                    panel7.Visible = false;
                    break;
                case "حسب المستخدم":
                    panel4.Visible = false;
                    panel5.Visible = false;
                    panel6.Visible = false;
                    panel7.Visible = true;
                    break;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DisplayData();
        }
        public async void DisplayData()
        {
            switch (comboBox1.Text)
            {
                case "الكل":
                    List<PurchaseSchema> recites = await db.GetAllPurches();
                    LoadDataGrid(recites);
                    break;
                case "حسب الموزع":
                    if (comboBox2.Text == "")
                    {
                        DialogResult resoult = ChestVault.Me.MessageBox("يرجي إختيار أسم الموزع", "بحث", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }
                    List<PurchaseSchema> bysupplierrecite = await db.GetPurchesBySupplier(comboBox2.Text);
                    LoadDataGrid(bysupplierrecite);
                    break;
                case "حسب التاريخ":
                    int Date = (dateTimePicker1.Value.Year * 10000) + (dateTimePicker1.Value.Month * 100) + (dateTimePicker1.Value.Day * 1);
                    List<PurchaseSchema> bydaterecite = await db.GetPurchesByDate(Date);
                    LoadDataGrid(bydaterecite);
                    break;
                case "حسب الحالة":
                    if (comboBox3.Text == "")
                    {
                        DialogResult resoult = ChestVault.Me.MessageBox("يرجي إختيار حالة للبحث", "بحث", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }
                    bool state = (comboBox3.Text == "مفتوحة") ? false : true;
                    List<PurchaseSchema> bystaterecite = await db.GetPurchesByDept(state);
                    LoadDataGrid(bystaterecite);
                    break;
                case "حسب المستخدم":
                    if (comboBox4.Text == "")
                    {
                        DialogResult resoult = ChestVault.Me.MessageBox("يرجي إختيار أسم المستخدم", "بحث", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }
                    List<PurchaseSchema> byuserrecites = await db.GetPurchaseByUser(comboBox4.Text);
                    LoadDataGrid(byuserrecites);
                    break;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ChestVault.Me.EditReciteSchema = false;
            Controls_Buy_Recite form = new Controls_Buy_Recite();
            form.Show();
            ChestVault.Me.MainForm.Enabled = false;
        }

        public async void LoadComboBox()
        {
            // Suppliers Load
            comboBox2.Items.Clear();
            string[] suppliers = await db.GetSuppliers();
            comboBox2.Items.Add("شركة عامة");
            comboBox2.Items.Add("مسترجعات");
            if(suppliers.Length > 0)
            foreach (string a in suppliers)
            {
                    comboBox2.Items.Add(a);
            }
            //users load
            comboBox4.Items.Clear();
            List<UsersSchema> users = await db.GetAllUsers();
            foreach(UsersSchema a in users)
            {
                comboBox4.Items.Add(a.Name);
            }

            comboBox1.Text = "الكل";
            comboBox3.Text = "مفتوحة";
        }

        private void Controls_Recites_Bougth_TextChanged(object sender, EventArgs e)
        {
                ChestVault.Me.EditReciteSchema = true;
                ChestVault.Me.EditingSchema = Recite_info[dataGrid.DoubleClick];
                Controls_Buy_Recite form = new Controls_Buy_Recite();
                ChestVault.Me.BuyNewRecite = form;
                form.Enabled = true;
                form.Show();
                ChestVault.Me.MainForm.Enabled = false;
        }

        private void Controls_Recites_Bougth_Enter(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChestVault.Me.EditReciteSchema = false;
            Controls_Buy_Recite form = new Controls_Buy_Recite();
            form.Show();
            ChestVault.Me.MainForm.Enabled = false;
        }
    }
}
