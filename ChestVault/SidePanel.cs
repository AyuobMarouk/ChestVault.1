using ChestVault.Schemas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ChestVault
{
    public partial class SidePanel : Form
    {
        public SidePanel()
        {
            InitializeComponent();
        }

        public List<MainMenuSearch> QuickSearch;
        public List<MainMenuSearch> ReturnDefaultValues()
        {
            List<MainMenuSearch> mainMenuSearches = new List<MainMenuSearch>();

            MainMenuSearch sellPoint = new MainMenuSearch();
            sellPoint.SearchValue = "نقطة بيع";
            sellPoint.formType = ChestVault.Me.SellingPoint;
            mainMenuSearches.Add(sellPoint);

            MainMenuSearch soldrecite = new MainMenuSearch();
            soldrecite.SearchValue = "إدارة الفواتير المباعة";
            soldrecite.formType = new Controls_ReciteSold();
            mainMenuSearches.Add(soldrecite);

            MainMenuSearch bougthrecite = new MainMenuSearch();
            bougthrecite.SearchValue = "إدارة الفواتير المشترية";
            bougthrecite.formType = new Controls_Recites_Bougth();
            mainMenuSearches.Add(bougthrecite);


            return mainMenuSearches;

        }
        public void SearchCLicked()
        {
            switch(SearchingMenu.SelectedValue)
            {
                case "نقطة بيع":
                    ChestVault.Me.MainForm.FillNewWindow(ChestVault.Me.MainForm.sellingpoint);
                    break;
                case "إدارة الفواتير المشترية":
                    Controls_Recites_Bougth recitebougthform = new Controls_Recites_Bougth();
                    ChestVault.Me.MainForm.FillNewWindow(recitebougthform);
                    break;
                case "إدارة الفواتير المباعة":
                    Controls_ReciteSold recitesoldform = new Controls_ReciteSold();
                    ChestVault.Me.MainForm.FillNewWindow(recitesoldform);
                    break;
            }
            SearchingMenu.Hide();
        }
        public SearchMenu SearchingMenu = new SearchMenu();

        public List<Form> MinimizedForms = new List<Form>();
        public List<System.Windows.Forms.Button> MiniButtons = new List<System.Windows.Forms.Button>();

        public List<System.Windows.Forms.Button> MainButtons = new List<System.Windows.Forms.Button>();
        public List<System.Windows.Forms.Button> SideButtons = new List<System.Windows.Forms.Button>();

        CRUD db = new CRUD();
        private void timer1_Tick(object sender, EventArgs e)
        {
            label7.Text = DateTime.Now.ToString();
            label1.Text = ChestVault.Me.CurrentUser.Name;
            label2.Text = ChestVault.Me.CurrentUser.Accessibility;

        }
        public void TimerState(bool state)
        {
            if(state) timer1.Start();
            else timer1.Stop();
        }
        private void SidePanel_Load(object sender, EventArgs e)
        {
            QuickSearch = ReturnDefaultValues();
            Point location = this.PointToScreen(textBox1.Location);
            SearchingMenu.Setup(location, textBox1.Size, this);


            TimerState(true);
            for(int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].GetType() == typeof(System.Windows.Forms.Button)) 
                {
                    if (this.Controls[i].MinimumSize.Width == 1)
                    {
                        MainButtons.Add((System.Windows.Forms.Button)this.Controls[i]);
                    }
                    else if (this.Controls[i].MinimumSize.Width > 1 && this.Controls[i].MinimumSize.Width < 10)
                    {
                        SideButtons.Add((System.Windows.Forms.Button)this.Controls[i]);
                        this.Controls[i].Visible = false;
                    }
                }
             }
            foreach(Control a in panel4.Controls)
            {
                if (a.MinimumSize.Width == 10)
                {
                    MiniButtons.Add((System.Windows.Forms.Button)a);
                }
            }


            ChangeDesign();
        }
        public void ChangeDesign()
        {

            this.BackColor = ChestVault.Me.customizeableUsers.FormBackGround;
            this.FormBorderStyle = FormBorderStyle.None;

            for (int i = 0; i < MainButtons.Count; i++)
            {
                MainButtons[i].BackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                MainButtons[i].FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                MainButtons[i].FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            }
            for (int i = 0; i < SideButtons.Count; i++)
            {
                SideButtons[i].BackColor = ChestVault.Me.customizeableUsers.SideBackGroundColor;
                SideButtons[i].FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.SideBackGroundColor;
                SideButtons[i].FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            }

            foreach (Control a in panel4.Controls)
            {
                if (a.GetType() == typeof(System.Windows.Forms.Button))
                {
                    if (a.MinimumSize.Width == 10)
                    {
                        System.Windows.Forms.Button s = a as System.Windows.Forms.Button;
                        s.BackColor = ChestVault.Me.customizeableUsers.ExtraButtonBG;
                        s.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ExtraButtonBG;
                        s.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;

                    }
                }
            }
            foreach (Control a in this.Controls)
            {
                if (a.GetType() == typeof(Label))
                {
                    if (a.MinimumSize.Width == 1) a.ForeColor = ChestVault.Me.customizeableUsers.HeaderFontColor;
                    else a.ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;

                    a.BackColor = Color.Transparent;
                }
                else if (a.GetType() == typeof(System.Windows.Forms.TextBox))
                {

                }

                else if ((a.GetType() == typeof(Panel)))
                {
                    if (a.Name == "panel4")
                    {
                        a.BackColor = ChestVault.Me.customizeableUsers.TitlePanelColor;
                    }
                }
                else if (a.GetType() == typeof(System.Windows.Forms.Button))
                {
                    if (a.MinimumSize.Width == 10)
                    {
                        System.Windows.Forms.Button s = a as System.Windows.Forms.Button;
                        s.BackColor = ChestVault.Me.customizeableUsers.ExtraButtonBG;
                        s.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ExtraButtonBG;
                        s.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;

                    }
                }
            }
        }
        public void ArrangeMap(object sender, EventArgs e)
        {
            int Clicked = 0;
            int AddCounter = 0;
            for(int i = 0; i < MainButtons.Count;i++)
            {
                if(sender == MainButtons[i])
                {
                    MainButtons[i].Left = 912;
                    MainButtons[i].Top = 274;
                    Clicked = i; 
                }
                else
                {
                    MainButtons[i].Left = 912  - ( 100 * AddCounter);
                    MainButtons[i].Top = 380;
                    AddCounter++;
                }
            }

            // Arrange Side Butons
            for(int i = 0;i < SideButtons.Count;i++)
            {
                if (SideButtons[i].MinimumSize.Width == MainButtons[Clicked].MinimumSize.Height)
                {
                    SideButtons[i].Visible = true;
                    SideButtons[i].Top = 274;
                    SideButtons[i].Left = 806 - (SideButtons[i].MinimumSize.Height * 105);
                }
                else { SideButtons[i].Visible = false;}
            }
        }
        public void ButtonsMouseOver(object sender, EventArgs e)
        {
            label3.Visible = true;
            foreach(Control a in this.Controls)
            {
                if(a.GetType() == typeof(System.Windows.Forms.Button))
                {
                    if(a == sender)
                    {
                        label3.Text = a.AccessibleDescription;
                        break;
                    }
                }
            }
        }
        public void MiniButtonsMouseOver(object sender, EventArgs e)
        {
            label3.Visible = true;
            label3.Text = (sender as System.Windows.Forms.Button).Text;
        }
        public void LeaveMouseOver(object sender, EventArgs e)
        {
            label3.Visible = false;
        }
        #region Minimized Menus Functions
        public bool AddMiniMenus(string formname, Form SendForm)
        {
            if (MinimizedForms.Count == 10)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("وصلت الحد الاقصي للشاشات الجانبية", "الشاشات الجانبية", Controls_Dialogue.ButtonsType.Ok);
                return false; 
            }
            MinimizedForms.Add(SendForm);
            ChestVault.Me.MainForm.ClearComboBox();

            for (int i = 0; i < MiniButtons.Count; i++)
            {
                if(i < MinimizedForms.Count && MinimizedForms[i] != null)
                {
                    if(MinimizedForms.Count -1 == i) MiniButtons[i].Text = formname;
                    MiniButtons[i].Visible = true;
                    ChestVault.Me.MainForm.addComboBox(MiniButtons[i].Text);
                }
                else
                {
                    MiniButtons[i].Visible = false;
                }
            }
            return true;
        }
        public void CloseMiniMenu()
        {
            MinimizedForms[ChestVault.Me.DisplayedMiniWindow] = null;
            MiniButtons[ChestVault.Me.DisplayedMiniWindow].Visible = false;

            ChestVault.Me.MainForm.ClearComboBox();
            for(int i = 0; i < MiniButtons.Count ; i++)
            {
               if (MiniButtons[i].Visible == true) ChestVault.Me.MainForm.addComboBox(MiniButtons[i].Text);
            }

           
            for(int x = 0; x < MinimizedForms.Count; x++)
            {
                if(MinimizedForms[x] != null)
                {
                    return;
                }
            }
            MinimizedForms.Clear();
        }
        public void MiniButtonsClick(object sender, EventArgs e)
        {
            for (int i = 0; i < MiniButtons.Count; i++)
            {
                if(sender == MiniButtons[i])
                {
                    if (MiniButtons[i].Text == "فاتروة مشتريات جديدة") ChestVault.Me.EditReciteSchema = true;
                    else if (MiniButtons[i].Text == "كافة الاصناف") (MinimizedForms[i] as Controls_AllItems).DrawGraphs();
                    MinimizedForms[i].Show();
                    ChestVault.Me.DisplayedMiniWindow = i;
                }
            }
        }
        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            Controls_Items form = new Controls_Items();
            form.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Controls_ReciteSold form = new Controls_ReciteSold();
            ChestVault.Me.MainForm.FillNewWindow(form);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Controls_Recites_Bougth form = new Controls_Recites_Bougth();
            ChestVault.Me.MainForm.FillNewWindow(form);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Controls_AllItems form = new Controls_AllItems();
            form.Show();
        }
        private void button16_Click(object sender, EventArgs e)
        {
            Controls_Activites form = new Controls_Activites();
            form.Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
        
        }

        private void button14_Click(object sender, EventArgs e)
        {
            UserSettings form = new UserSettings();
            ChestVault.Me.MainForm.FillNewWindow(form);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChestVault.Me.MainForm.FillNewWindow(ChestVault.Me.MainForm.sellingpoint);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Controls_Accountant form = new Controls_Accountant();
            ChestVault.Me.MainForm.FillNewWindow(form);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult resoult = ChestVault.Me.MessageBox("هل أنتا متأكد من إغلاق البرنامج", "إغلاق البرنامج", Controls_Dialogue.ButtonsType.SureCancel);
            if(resoult == DialogResult.OK)
            {
                Application.Exit();
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult resoult = ChestVault.Me.MessageBox("هل أنتا متأكد من تبديل المستخدم", "تبديل المستخدم", Controls_Dialogue.ButtonsType.SureCancel);
            if (resoult == DialogResult.OK)
            {
                ChestVault.Me.MainForm.Enabled = false;
                Controls_LoginForm Login = new Controls_LoginForm();
                Login.Show();
            }
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            List<UsersSchema> list = new List<UsersSchema>();
            DialogResult resoult = ChestVault.Me.InputField("يرجي تأكيد الصلاحيات", InputField.InputFieldType.PasswordChecker);

            if(resoult == DialogResult.OK) 
            {
                list = await db.GetUsers("admin");

                if (list[0].Password == ChestVault.Me.InputFieldWindow)
                {
                    SystemSettings form = new SystemSettings();
                    ChestVault.Me.MainForm.FillNewWindow(form);
                }
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            SearchingMenu.Show();
            if (textBox1.Text == "بحث")
            {
                textBox1.Text = "";
            }

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "بحث";
            }
            SearchingMenu.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (MainMenuSearch a in QuickSearch)
            {
                if (a.SearchValue.Contains(textBox1.Text))
                {
                    list.Add(a.SearchValue);
                    if (list.Count == 10)
                    {
                        SearchingMenu.DisplayText(list);
                        break;
                    }
                }
            }
            SearchingMenu.DisplayText(list);
        }

        private void SidePanel_TextChanged(object sender, EventArgs e)
        {
            if(this.Text == "Search")
            {
                SearchCLicked();
            }
            this.Text = "Chest Vault";
        }
    }

    public class MainMenuSearch
    {
        public Form formType;
        public string SearchValue;
    }
}
