using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChestVault;
using ChestVault.Schemas;
namespace ChestVault
{
    public partial class ActivateSystem : Form
    {
        public ActivateSystem()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }
        CRUD db = new CRUD();
        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(textBox1.Text == ((DateTime.Now.Year * 10000) + (DateTime.Now.Month * 100) + DateTime.Now.Day).ToString())
                {
                    SettingsSchema newSettings = new SettingsSchema();
                    newSettings.NotificationAmount = 7;
                    newSettings.SignUpDate = DateTime.Now;

                    await db.AddSetting(newSettings);

                    List<UsersSchema> users = await db.GetAllUsers();

                    if (users.Count == 0)
                    {
                        UsersSchema user = new UsersSchema();
                        user.Name = "admin";
                        user.Password = "admin";
                        user.Accessibility = "admin";
                        user.UserColor = new List<SavedColors>();
                        for (int i = 0; i < 6; i++)
                        {
                            user.UserColor.Add(new SavedColors());
                        }
                        user.UserColor[0].red = 50;
                        user.UserColor[0].green = 49;
                        user.UserColor[0].blue = 71;

                        user.UserColor[1].red = 29;
                        user.UserColor[1].green = 28;
                        user.UserColor[1].blue = 50;

                        user.UserColor[2].red = 49;
                        user.UserColor[2].green = 47;
                        user.UserColor[2].blue = 72;

                        user.UserColor[3].red = 255;
                        user.UserColor[3].green = 117;
                        user.UserColor[3].blue = 73;

                        user.UserColor[4].red = 173;
                        user.UserColor[4].green = 57;
                        user.UserColor[4].blue = 43;

                        user.UserColor[4].red = 255;
                        user.UserColor[4].green = 151;
                        user.UserColor[4].blue = 73;
                        await db.AddUser(user);
                    }

                    Controls_LoginForm Login = new Controls_LoginForm();
                    Login.Show();

                    this.Hide();
                }
                else
                {
                    DialogResult resoult = ChestVault.Me.MessageBox("الرمز خاطئ", "تأكيد البرنامج", Controls_Dialogue.ButtonsType.Ok);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult resoult = ChestVault.Me.MessageBox("هل أنتا متأكد من إغلاق البرنامج", "إغلاق البرنامج", Controls_Dialogue.ButtonsType.SureCancel);
            if (resoult == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void ActivateSystem_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
