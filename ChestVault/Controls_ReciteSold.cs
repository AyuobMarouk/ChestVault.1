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
    public partial class Controls_ReciteSold : Form
    {
        public Controls_ReciteSold()
        {
            InitializeComponent();

           
            ChestVault.Me.ChangeDesign(this);
        }

        CRUD db = new CRUD();

        DataGrid dataGrid = new DataGrid();

        public List<RecitesSchema> SavedRecites = new List<RecitesSchema>();

        private void Controls_ReciteSold_Load(object sender, EventArgs e)
        {
            panel2.Controls.Add(dataGrid.DisplayForm(this));

            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            LoadComboBox();
            panel8.Visible = false;
            LoadDataGrid(new List<RecitesSchema>());
        }
        public void LoadDataGrid(List<RecitesSchema> info)
        {

            SavedRecites = info;

            DataGridColumn supplier = new DataGridColumn();
            supplier.LabelSize = new Size(200, 40);
            supplier.HeaderTitle = "الزبون";
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

            for (int i = 0; i < info.Count; i++)
            {
                supplier.Text.Add(info[i].Consumer.ToString());
                number.Text.Add(info[i].Number.ToString());
                amount.Text.Add(info[i].items.Count.ToString());
                double sum = 0;
                foreach (SoldItemsSchema a in info[i].items)
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

            dataGrid.ReloadDataGrid();
        }
        public async void DisplayData()
        {
            //ChestVault.Me.DisplayRecitesInControl = this;
            switch (comboBox1.Text)
            {
                case "الكل":
                    List<RecitesSchema> recites = await db.GetAllRecites();
                    LoadDataGrid(recites);
                    break;
                case "حسب الزبون":
                    if (comboBox2.Text == "")
                    {
                        DialogResult resoult = ChestVault.Me.MessageBox("يرجي إختيار أسم الزبون", "بحث", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }
                    List<RecitesSchema> bysupplierrecite = new List<RecitesSchema>();

                    int FROM = (dateTimePicker2.Value.Year * 10000) + (dateTimePicker2.Value.Month * 100) + (dateTimePicker2.Value.Day * 1);
                    int To = (dateTimePicker3.Value.Year * 10000) + (dateTimePicker3.Value.Month * 100) + (dateTimePicker3.Value.Day * 1);

                    if (checkBox1.Checked) bysupplierrecite = await db.GetReciteRangeCustomer(FROM,To, comboBox2.Text);
                    else bysupplierrecite = await db.GetReciteByConsumer(comboBox2.Text);
                    LoadDataGrid(bysupplierrecite);
                    break;
                case "حسب التاريخ":
                    int Date = (dateTimePicker1.Value.Year * 10000) + (dateTimePicker1.Value.Month * 100) + (dateTimePicker1.Value.Day * 1);
                    List<RecitesSchema> bydaterecite = await db.GetReciteByDate(Date);
                    LoadDataGrid(bydaterecite);
                    break;
                case "حسب الحالة":
                    if (comboBox3.Text == "")
                    {
                        DialogResult resoul = ChestVault.Me.MessageBox("يرجي إختيار حالة للبحث", "بحث", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }

                    List<RecitesSchema> bystaterecite = new List<RecitesSchema>();
                    int from = (dateTimePicker2.Value.Year * 10000) + (dateTimePicker2.Value.Month * 100) + (dateTimePicker2.Value.Day * 1);
                    int to = (dateTimePicker3.Value.Year * 10000) + (dateTimePicker3.Value.Month * 100) + (dateTimePicker3.Value.Day * 1);

                    bool state = (comboBox3.Text == "مفتوحة") ? false : true;

                    if (checkBox1.Checked) bystaterecite = await db.GetReciteRangeState(from, to, state);
                    else bystaterecite = await db.GetReciteByState(state);
                    LoadDataGrid(bystaterecite);
                    break;
                case "حسب المستخدم":
                    if (comboBox4.Text == "")
                    {
                        DialogResult resoult = ChestVault.Me.MessageBox("يرجي إختيار أسم المستخدم", "بحث", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }

                    List<RecitesSchema> byuserrecites = new List<RecitesSchema>();
                    int FRom = (dateTimePicker2.Value.Year * 10000) + (dateTimePicker2.Value.Month * 100) + (dateTimePicker2.Value.Day * 1);
                    int TO = (dateTimePicker3.Value.Year * 10000) + (dateTimePicker3.Value.Month * 100) + (dateTimePicker3.Value.Day * 1);

                    if (checkBox1.Checked) byuserrecites = await db.GetReciteRangeUser(FRom,TO,comboBox4.Text);
                    else byuserrecites = await db.GetReciteByUser(comboBox4.Text);
                    LoadDataGrid(byuserrecites);
                    break;
                case "حسب التاريخ ( من ألي )":
                    int FROm = (dateTimePicker2.Value.Year * 10000) + (dateTimePicker2.Value.Month * 100) + (dateTimePicker2.Value.Day * 1);
                    int ToR = (dateTimePicker3.Value.Year * 10000) + (dateTimePicker3.Value.Month * 100) + (dateTimePicker3.Value.Day * 1);
                    List<RecitesSchema> bydate = await db.GetReciteInDateRange(FROm, ToR);
                    LoadDataGrid(bydate);
                    break;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            DisplayData();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChestVault.Me.MainForm.FillNewWindow(ChestVault.Me.MainForm.SidePanel);

        }
        public async void LoadComboBox()
        {
            // Consumer Load
            comboBox2.Items.Clear();
            comboBox2.Items.Add("زبون عام");
            comboBox2.Items.Add("مسترجعات");
            List<DeptSchema> suppliers = await db.GetAllCustomers();
            if(suppliers.Count > 0)
            foreach (DeptSchema a in suppliers)
            {
                comboBox2.Items.Add(a.Name);
            }
            //users load
            comboBox4.Items.Clear();
            List<UsersSchema> users = await db.GetAllUsers();
            foreach (UsersSchema a in users)
            {
                comboBox4.Items.Add(a.Name);
            }

            comboBox1.Text = "الكل";
            comboBox3.Text = "مفتوحة";
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "الكل":
                    panel4.Visible = false;
                    panel5.Visible = false;
                    panel6.Visible = false;
                    panel7.Visible = false;
                    panel1.Visible = false;
                    panel8.Visible = false;
                    button3.Visible = false;
                    break;
                case "حسب الزبون":
                    panel4.Visible = true;
                    panel5.Visible = false;
                    panel6.Visible = false;
                    panel7.Visible = false;
                    panel1.Visible = false;
                    panel8.Visible = true;
                    button3.Visible = false;
                    break;
                case "حسب التاريخ":
                    panel4.Visible = false;
                    panel5.Visible = true;
                    panel6.Visible = false;
                    panel1.Visible = false;
                    panel8.Visible = false;
                    button3.Visible = false;
                    break;
                case "حسب الحالة":
                    panel4.Visible = false;
                    panel5.Visible = false;
                    panel6.Visible = true;
                    panel7.Visible = false;
                    panel1.Visible = false;
                    panel8.Visible = true;
                    button3.Visible = false;
                    break;
                case "حسب المستخدم":
                    panel4.Visible = false;
                    panel5.Visible = false;
                    panel6.Visible = false;
                    panel7.Visible = true;
                    panel1.Visible = false;
                    panel8.Visible = true;
                    button3.Visible = true;
                    break;
                case "حسب التاريخ ( من ألي )":
                    panel4.Visible = false;
                    panel5.Visible = false;
                    panel6.Visible = false;
                    panel7.Visible = false;
                    panel1.Visible = true;
                    panel8.Visible = false;
                    button3.Visible = false;
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker4.Visible = checkBox1.Checked;
            dateTimePicker5.Visible = checkBox1.Checked; 
            label9.Visible = checkBox1.Checked; 
            label10.Visible = checkBox1.Checked;
        }
        private async void button3_Click(object sender, EventArgs e)
        {
                if (comboBox4.Text == "")
                {
                    DialogResult resoult = ChestVault.Me.MessageBox("يرجي إختيار أسم المستخدم", "بحث", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }

            List<RecitesSchema> byuserrecites = new List<RecitesSchema>();

            List<WorkSchedule> worked = await db.GetWorkScheduleByUser(comboBox4.Text);
            if(worked.Count != 0)
            for(int i = 0; i < worked[worked.Count - 1].RecitesSold.Count;i++)
            {
                List<RecitesSchema> checkrecite = await db.GetRecite(worked[worked.Count - 1].RecitesSold[i]);
                byuserrecites.Add(checkrecite[0]);
            }
           
            LoadDataGrid(byuserrecites);
        }

        private void Controls_ReciteSold_TextChanged(object sender, EventArgs e)
        {
            ChestVault.Me.SoldRecitesWindow = this;
            this.Enabled = false;

            Controls_StatsAnalyytics newform = new Controls_StatsAnalyytics();

            List<RecitesSchema> selectedrecite = new List<RecitesSchema>();
            selectedrecite.Add(SavedRecites[dataGrid.DoubleClick + (dataGrid.CurrentPage * dataGrid.DisplayLimit)]);
            newform.Show();
            newform.LoadData(selectedrecite);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (SavedRecites.Count == 0)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("لا يوجد إحصائيات للعرض", "إحصائيات", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            ChestVault.Me.SoldRecitesWindow = this;
            this.Enabled = false;

            Controls_StatsAnalyytics newform = new Controls_StatsAnalyytics();

            newform.Show();
            newform.LoadData(SavedRecites);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChestVault.Me.MainForm.FillWithMainMenu();
        }
    }
}
