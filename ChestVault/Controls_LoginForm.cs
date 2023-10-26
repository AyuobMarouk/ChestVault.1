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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ChestVault
{
    public partial class Controls_LoginForm : Form
    {
        public Controls_LoginForm()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }

        CRUD db = new CRUD();

        List<UsersSchema> users;

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            users = await db.GetAllUsers();

            comboBox1.Items.Clear();
            foreach (UsersSchema u in users)
            {
                comboBox1.Items.Add(u.Name);
            }
            comboBox1.Select();
        }
        public async void CloseWorkSchedules(bool All)
        {
            List<WorkSchedule> work = new List<WorkSchedule>();

            work = await db.GetAllOpenSchedules();

            if (work.Count == 0) return;

            for (int i = 0; i < work.Count; i++)
            {
                if (All)
                {
                    work[i].Open = false;
                    work[i].CloseDate = DateTime.Now;
                    await db.UpdateWorkSchedule(work[i]);
                }
                else if (work[i].User != ChestVault.Me.CurrentUser.Name)
                {
                    work[i].Open = false;
                    work[i].CloseDate = DateTime.Now;
                    await db.UpdateWorkSchedule(work[i]);

                }
            }
        }
        public async void login()
        {
            bool pass = false; // used to display a label is value
            foreach (var us in users)
            {
                if (comboBox1.Text == us.Name)
                {
                    pass = true;
                    if (textBox2.Text == us.Password)
                    {
                        ChestVault.Me.CurrentUser = us;
                        ChestVault.Me.AddActivity("تم تسجيل الدخول", "Login");

                        ChestVault.Me.customizeableUsers.TitlePanelColor = Color.FromArgb(us.UserColor[0].red, us.UserColor[0].green, us.UserColor[0].blue);
                        ChestVault.Me.customizeableUsers.FormBackGround = Color.FromArgb(us.UserColor[1].red, us.UserColor[1].green, us.UserColor[1].blue);
                        ChestVault.Me.customizeableUsers.SpecialPanel = Color.FromArgb(us.UserColor[2].red, us.UserColor[2].green, us.UserColor[2].blue);

                        ChestVault.Me.customizeableUsers.HeaderFontColor = Color.FromArgb(us.UserColor[3].red, us.UserColor[3].green, us.UserColor[3].blue);
                        ChestVault.Me.customizeableUsers.TextColorTheme = Color.FromArgb(us.UserColor[4].red, us.UserColor[4].green, us.UserColor[4].blue);

                        ChestVault.Me.customizeableUsers.SideBackGroundColor = Color.FromArgb(us.UserColor[5].red, us.UserColor[5].green, us.UserColor[5].blue);
                        ChestVault.Me.MainForm.SidePanel.ChangeDesign();
                        CloseWorkSchedules(false);

                        List<WorkSchedule> lastworkSchedule = await db.GetScheduleByName(us.Name);

                        if (lastworkSchedule.Count > 0)
                        {
                            if (lastworkSchedule[lastworkSchedule.Count - 1].Open == false)
                            {

                                WorkSchedule newwork = new WorkSchedule();
                                newwork.User = us.Name;
                                newwork.Open = true;
                                newwork.RecitesBougth = new List<int>();
                                newwork.RecitesSold = new List<int>();
                                newwork.OpenDate = DateTime.Now;
                                await db.AddSchedule(newwork);
                            }
                        }
                        else
                        {
                            WorkSchedule newwork = new WorkSchedule();
                            newwork.User = us.Name;
                            newwork.Open = true;
                            newwork.RecitesBougth = new List<int>();
                            newwork.RecitesSold = new List<int>();
                            newwork.OpenDate = DateTime.Now;
                            await db.AddSchedule(newwork);
                        }
                        ChestVault.Me.MainForm.Enabled = true;
                        ChestVault.Me.MainForm.SidePanel.TimerState(true);
                        this.Hide();
                    }
                }
            }
            if (!pass) label2.ForeColor = Color.Red;
            else { label3.ForeColor = Color.Red; label2.ForeColor = Color.Black; }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                login();
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void TitlePanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
                 login();
        }

        public void CheatCodes()
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
