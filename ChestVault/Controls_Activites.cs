using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChestVault;
using ChestVault.Schemas;
namespace ChestVault
{
    public partial class Controls_Activites : Form
    {
        public Controls_Activites()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }
        CRUD db = new CRUD();
        DataGrid dataGrid = new DataGrid();

        List<ActivitiesSchema> activities = new List<ActivitiesSchema>();
        public async void LoadComboBox()
        {
            // reasons
            comboBox3.Items.Clear();
            
            //users load
            comboBox4.Items.Clear();
            List<UsersSchema> users = await db.GetAllUsers();
            foreach (UsersSchema a in users)
            {
                comboBox4.Items.Add(a.Name);
            }
            comboBox3.Items.Clear();
            string[] reasons = await db.GetReasons();
            for(int i = 0; i < reasons.Length; i ++)
            {
                comboBox3.Items.Add(reasons[i]);
            }

            comboBox1.Text = "الكل";
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "الكل":
                    panel6.Visible = false;
                    panel7.Visible = false;
                           break;
                case "حسب المستخدم":
                    panel6.Visible = false;
                    panel7.Visible = true;
                    break;
                case "حسب السبب":
                    panel6.Visible = true;
                    panel7.Visible = false;
                    break;
            }
        }

        private void Controls_Activites_Load(object sender, EventArgs e)
        {
            panel2.Controls.Add(dataGrid.DisplayForm(this));

            LoadComboBox();
            LoadDataGrid();
        }

        public void LoadDataGrid()
        {
            DataGridColumn activ = new DataGridColumn();
            activ.LabelSize = new Size(465, 40);
            activ.HeaderTitle = "النشاط";
            activ.dataType = DataGridColumn.DataType.Text;

            DataGridColumn user = new DataGridColumn();
            user.LabelSize = new Size(200, 40);
            user.HeaderTitle = "أسم المستخدم";
            user.dataType = DataGridColumn.DataType.Text;

            DataGridColumn date = new DataGridColumn();
            date.LabelSize = new Size(200, 40);
            date.HeaderTitle = "التاريخ";
            date.dataType = DataGridColumn.DataType.Date;

            foreach (ActivitiesSchema a in activities)
            {
                activ.Text.Add(a.Message);
                user.Text.Add(a.User);
                date.Text.Add(a.Time.ToString());
            }

            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(activ);
                dataGrid.Column.Add(user);
                dataGrid.Column.Add(date);
            }
            else
            {
                dataGrid.Column[0].Text = activ.Text;
                dataGrid.Column[1].Text = user.Text;
                dataGrid.Column[2].Text = date.Text;
            }

             dataGrid.ReloadDataGrid();
        }
        private async void button8_Click(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "الكل":
                    activities = await db.GetAllActivites();
                    LoadDataGrid();
                    break;
                case "حسب المستخدم":
                    if (comboBox4.Text == "")
                    {
                        DialogResult resoult = ChestVault.Me.MessageBox("يرجي اختيار أسم المستخدم", "أسم المستخدم خاطئ", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }
                    activities = await db.GetAcitivitesByUser(comboBox4.Text);
                    LoadDataGrid();
                    break;
                case "حسب السبب":
                    if (comboBox3.Text == "")
                    {
                        DialogResult resoult = ChestVault.Me.MessageBox("يرجي أختيار سبب للبحث", "بحث", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }
                    activities = await db.GetActivityByReason(comboBox3.Text);
                    LoadDataGrid();
                    break;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
            this.Hide();
        }
        private void Controls_Activites_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
        }
        #region Draggable Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion
    }
}
