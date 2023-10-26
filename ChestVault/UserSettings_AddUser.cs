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
using ChestVault;
using Microsoft.VisualBasic.ApplicationServices;

namespace ChestVault
{
    public partial class UserSettings_AddUser : Form
    {
        public UserSettings_AddUser()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }

        CRUD db = new CRUD();

        public UsersSchema EditUser = null;

        public async void LoadData(string Username)
        {
            List<UsersSchema> list = await db.GetUsers(Username);
            EditUser = list[0];

            textBox1.Text = EditUser.Name;
            textBox3.Text = EditUser.Password;
            comboBox2.Text = EditUser.Accessibility;

            if (EditUser.Name == "admin") textBox1.Enabled = false;
            comboBox2.Enabled = false;
        }
        private async void button11_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox3.Text == "" || comboBox2.Text == "")
            {
                ChestVault.Me.MessageBox("هناك خانة فارغة", "تنبيه", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            if (textBox1.Text[0].ToString() == " " || textBox3.Text[0].ToString() == " ")
            {
                ChestVault.Me.MessageBox("لا يمكن ان يكون بداية الاسم او كلمة المرور فراغ", "تنبيه", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            if (char.IsDigit(textBox3.Text[0]))
            {
                ChestVault.Me.MessageBox("لا يمكن ان يكون بداية اسم المستخدم رقم", "تنبيه", Controls_Dialogue.ButtonsType.Ok);
                return;
            }


            List<UsersSchema> Users = await db.GetAllUsers();

            if (EditUser == null)
            {
                UsersSchema newUser = new UsersSchema();

                foreach (UsersSchema a in Users)
                {
                    if (textBox1.Text == a.Name)
                    {
                        ChestVault.Me.MessageBox("هذا الاسم موجود بالفعل", "تنبيه", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }
                    if (a.Name == "admin") newUser.UserColor = a.UserColor;
                }
                newUser.Name = textBox3.Text;
                newUser.Password = textBox1.Text;
                newUser.Accessibility = comboBox2.Text;
                
                await db.AddUser(newUser);
                ChestVault.Me.MessageBox("تم أضافة بيانات المستخدم بنجاح", "أضافة مستخدم", Controls_Dialogue.ButtonsType.Ok);
                ChestVault.Me.MainForm.Enabled = true;
                (ChestVault.Me.MainForm.FilledWindow as UserSettings).loadUsers();
                this.Hide();
            }
            else
            {
                foreach (UsersSchema a in Users)
                {
                    if (textBox1.Text == a.Name && a.Name != EditUser.Name)
                    {
                        ChestVault.Me.MessageBox("هذا الاسم موجود بالفعل", "تنبيه", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }
                }

                EditUser.Name = textBox1.Text;
                EditUser.Password = textBox3.Text;
                EditUser.Accessibility = comboBox2.Text;

                await db.UpdateUser(EditUser);
                ChestVault.Me.MessageBox("تم تعديل بيانات المستخدم بنجاح", "تعديل البيانات", Controls_Dialogue.ButtonsType.Ok);
                (ChestVault.Me.MainForm.FilledWindow as UserSettings).loadUsers();
                ChestVault.Me.MainForm.Enabled = true;
                this.Hide();
            }

        }

        private void UserSettings_AddUser_Load(object sender, EventArgs e)
        {
            textBox3.Select();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
            this.Hide();
        }

        private void UserSettings_AddUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
            this.Hide();
        }
    }
}
