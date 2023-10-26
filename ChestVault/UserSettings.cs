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
using static ChestVault.UserSettings;

namespace ChestVault
{
    public partial class UserSettings : Form
    {
        public UserSettings()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }

        CRUD db = new CRUD();
        public enum CurrentForm
        {
            Colors,
            Access
        }
        public CurrentForm currentForm;

        public UserSettings_Colors ColorForm;
        public UserSettings_Access AccessForm;
        public async void loadUsers()
        {
            comboBox1.Items.Clear();
            List<UsersSchema> users = await db.GetAllUsers();
            foreach (UsersSchema a in users)
            {
                comboBox1.Items.Add(a.Name);
            }
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if(currentForm == CurrentForm.Colors)
            {
                LoadColorForm();
            }
            else if(currentForm == CurrentForm.Access)
            {
                LoadAccess();
            }
        }
        private void UserSettings_Load(object sender, EventArgs e)
        {
            currentForm = CurrentForm.Colors;
            loadUsers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
            this.Hide();
        }


        public void LoadColorForm()
        {
            if (comboBox1.Text == "") return;

            if (AccessForm != null) AccessForm.Hide();
            if (ColorForm == null) ColorForm = new UserSettings_Colors();
            ColorForm.TopLevel = false;
            ColorForm.AutoScroll = true;
            panel1.Controls.Add(ColorForm);
            ColorForm.Show();
            ColorForm.Dock = DockStyle.Fill;

            ColorForm.LoadColors(comboBox1.Text);
            currentForm = CurrentForm.Colors;
        }
        public void LoadAccess()
        {
            if (comboBox1.Text == "") return;

            if (ColorForm != null) ColorForm.Hide();
            if (AccessForm == null) AccessForm = new UserSettings_Access();
            AccessForm.TopLevel = false;
            AccessForm.AutoScroll = true;
            panel1.Controls.Add(AccessForm);
            AccessForm.Show();
            AccessForm.Dock = DockStyle.Fill;

            AccessForm.Enabled = (ChestVault.Me.CurrentUser.Accessibility == "admin")?true : false;
            currentForm = CurrentForm.Access;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            LoadColorForm();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChestVault.Me.MainForm.FillNewWindow(ChestVault.Me.MainForm.SidePanel);
        }
        // Save User Info
        private async void button8_Click(object sender, EventArgs e)
        {
            if(ColorForm != null) ColorForm.UpdateColer();

            List<UsersSchema> a = await db.GetUsers(ChestVault.Me.CurrentUser.Name);
            ChestVault.Me.CurrentUser = a[0];

            #region Change System Colors
            ChestVault.Me.customizeableUsers.TitlePanelColor = Color.FromArgb(ChestVault.Me.CurrentUser.UserColor[0].red, ChestVault.Me.CurrentUser.UserColor[0].green, ChestVault.Me.CurrentUser.UserColor[0].blue);
            ChestVault.Me.customizeableUsers.FormBackGround = Color.FromArgb(ChestVault.Me.CurrentUser.UserColor[1].red, ChestVault.Me.CurrentUser.UserColor[1].green, ChestVault.Me.CurrentUser.UserColor[1].blue);
            ChestVault.Me.customizeableUsers.SpecialPanel = Color.FromArgb(ChestVault.Me.CurrentUser.UserColor[2].red, ChestVault.Me.CurrentUser.UserColor[2].green, ChestVault.Me.CurrentUser.UserColor[2].blue);

            ChestVault.Me.customizeableUsers.HeaderFontColor = Color.FromArgb(ChestVault.Me.CurrentUser.UserColor[3].red, ChestVault.Me.CurrentUser.UserColor[3].green, ChestVault.Me.CurrentUser.UserColor[3].blue);
            ChestVault.Me.customizeableUsers.TextColorTheme = Color.FromArgb(ChestVault.Me.CurrentUser.UserColor[4].red, ChestVault.Me.CurrentUser.UserColor[4].green, ChestVault.Me.CurrentUser.UserColor[4].blue);

            ChestVault.Me.customizeableUsers.SideBackGroundColor = Color.FromArgb(ChestVault.Me.CurrentUser.UserColor[5].red, ChestVault.Me.CurrentUser.UserColor[5].green, ChestVault.Me.CurrentUser.UserColor[5].blue);

            #endregion

            ChestVault.Me.MessageBox("تم حقظ التغيرات", "تم الحفظ", Controls_Dialogue.ButtonsType.Ok);
            ChestVault.Me.ChangeDesign(this);

            ChestVault.Me.MainForm.SidePanel.ChangeDesign();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadAccess();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UserSettings_AddUser form = new UserSettings_AddUser();
            ChestVault.Me.MainForm.Enabled = false;
            if (comboBox1.Text == "")
            {
                form.Show();
            }
            else
            {
                form.LoadData(comboBox1.Text);
                form.Show();
            }
        }
    }
}
