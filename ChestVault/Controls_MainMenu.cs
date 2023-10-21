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
    public partial class Controls_MainMenu : Form
    {
        public Controls_MainMenu()
        {
            InitializeComponent();
        }
        CRUD db = new CRUD();
        public SidePanel SidePanel = new SidePanel();
        public Controls_SellingPoint sellingpoint;
        public Form FilledWindow;
        public async void StartUp()
        {
            this.WindowState = FormWindowState.Maximized;
            ChestVault.Me = new ChestVault();
            ChestVault.Me.MainForm = this;
            this.Enabled = false;
            FillWithMainMenu();
            try
            {
                List<SettingsSchema> settings = await db.getSetting();
                if (settings.Count > 0)
                {
                    Controls_LoginForm Login = new Controls_LoginForm();
                    Login.Show();
                }
                else
                {
                    ActivateSystem activate = new ActivateSystem();
                    activate.Show();
                }
                sellingpoint = new Controls_SellingPoint();
            }
            catch(Exception e)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("لا يوجد قاعدة بيانات", "Time Out", Controls_Dialogue.ButtonsType.Ok);
                Application.Exit();
            }
        }

        public void AccessibilityChanged()
        {
            if(ChestVault.Me.CurrentUser.Accessibility != "admin")
            {
                
            }
            else
            {
               
            }
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {
            StartUp();

        }

        #region Fill Panel With Forms Function
        public void FillNewWindow(Form Display)
        {
            if (FilledWindow != null) FilledWindow.Hide();
            FilledWindow = Display;
            FilledWindow.TopLevel = false;
            FilledWindow.AutoScroll = true;
            panel3.Controls.Add(FilledWindow);
            FilledWindow.Show();
            FilledWindow.Dock = DockStyle.Fill;
        }
        public void FillWithMainMenu()
        {
            if (FilledWindow != null) FilledWindow.Hide();
            FilledWindow = SidePanel;
            FilledWindow.TopLevel = false;
            FilledWindow.AutoScroll = true;
            panel3.Controls.Add(FilledWindow);
            FilledWindow.Show();
            FilledWindow.Dock = DockStyle.Fill;
        }

        public void ClearComboBox()
        {
            comboBox1.Items.Clear();
        }
        public void addComboBox(string str)
        {
            comboBox1.Items.Add(str);
        }

        #endregion

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "") return;
            if (SidePanel.MiniButtons[comboBox1.SelectedIndex].Text == "فاتروة مشتريات جديدة") ChestVault.Me.EditReciteSchema = true;
            else if (SidePanel.MiniButtons[comboBox1.SelectedIndex].Text == "كافة الاصناف") (SidePanel.MinimizedForms[comboBox1.SelectedIndex] as Controls_AllItems).DrawGraphs();
            SidePanel.MinimizedForms[comboBox1.SelectedIndex].Show();
            ChestVault.Me.DisplayedMiniWindow = comboBox1.SelectedIndex;
        }
    }
}
