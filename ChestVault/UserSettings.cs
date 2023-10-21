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
        async void loadUsers()
        {
            comboBox1.Items.Clear();
            List<UsersSchema> users = await db.GetAllUsers();
            foreach (UsersSchema a in users)
            {
                comboBox1.Items.Add(a.Name);
            }
        }

        private async void comboBox1_TextChanged(object sender, EventArgs e)
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

        private async void button1_Click(object sender, EventArgs e)
        {
           /* foreach (var a in comboBox1.Items)
            {
                if (a.ToString() == textBox1.Text)
                {
                    return;
                }
            }

            UsersSchema newUser = new UsersSchema();
            newUser.Name = textBox1.Text;
            newUser.Password = textBox2.Text;

            switch (comboBox4.Text)
            {
                case "بياع":
                    newUser.Accessibility = "Cashier";
                    break;
                case "مدير مشتريات":
                    newUser.Accessibility = "Manager";
                    break;
                case "تحليل بيانات":
                    newUser.Accessibility = "Analysist";
                    break;
                case "مسؤل":
                    newUser.Accessibility = "admin";
                    break;
            }
            await db.AddUser(newUser);
            DialogResult resoult = ChestVault.Me.MessageBox("تم أضافة مستخدم جديد", "مستخدم جديد", Controls_Dialogue.ButtonsType.Ok);
            loadUsers();*/
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
            //AccessForm.LoadColors(comboBox1.Text);
            currentForm = CurrentForm.Access;
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            LoadColorForm();
            /* if(comboBox1.Text == "")
             {
                 DialogResult resoult = ChestVault.Me.MessageBox("يرجي اختيار مستخدم لتعديل", "تعديل مستخدم", Controls_Dialogue.ButtonsType.Ok);
                 return;
             }
             List<UsersSchema> users = await db.GetUsers(comboBox1.Text);
             bool exit = false;
             foreach(UsersSchema a in users)
             {
                 if(a.Name != oldUsername)
                 {
                     exit = true;
                 }
             }
             if(exit)
             {

                 DialogResult resoult = ChestVault.Me.MessageBox("هذا الاسم موجود بالفعل", "هذا الاسم موجود بالفعل", Controls_Dialogue.ButtonsType.Ok);
                 return;
             }

             List<UsersSchema> updateuser = await db.GetUsers(oldUsername);
             updateuser[0].Name = comboBox1.Text;
             updateuser[0].Password = textBox3.Text;

             if(updateuser[0].Name != "admin")
             switch (comboBox2.Text)
             {
                 case "بياع":
                     updateuser[0].Accessibility = "Cashier";
                     break;
                 case "مدير مشتريات":
                     updateuser[0].Accessibility = "Manager";
                     break;
                 case "تحليل بيانات":
                     updateuser[0].Accessibility = "Analysist";
                     break;
                 case "مسؤل":
                     updateuser[0].Accessibility = "admin";
                     break;
             }
             await db.UpdateUser(updateuser[0]);
             loadUsers();*/
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

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
