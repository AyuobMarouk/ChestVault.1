using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using ChestVault;
using ChestVault.Schemas;
using System.Drawing.Drawing2D;
using static ChestVault.Controls_Dialogue;
using System.Windows.Forms.VisualStyles;
using static ChestVault.InputField;

namespace ChestVault
{
    class ChestVault
    {
        public string Version = "1.0.6";

        public CustomizeableUsers customizeableUsers = new CustomizeableUsers();

        public static ChestVault Me;
        public CRUD db = new CRUD();

        public Controls_MainMenu MainForm;

        public UsersSchema CurrentUser = new UsersSchema();
        public SettingsSchema CurrentSettings = new SettingsSchema();
        public int DisplayedMiniWindow;

        public bool Checking = false;

        public bool EditReciteSchema = false;
        public PurchaseSchema EditingSchema;
        public Controls_Buy_Recite BuyNewRecite;


        public Controls_SellingPoint SellingPoint;

        public Controls_ReciteSold SoldRecitesWindow;

        public string InputFieldWindow;

        #region Data Base Related Functions
        public async void AddActivity(string Message,string Reason)
        {
            ActivitiesSchema activity = new ActivitiesSchema();

            activity.User = CurrentUser.Name;
            activity.Time = DateTime.Now;
            activity.Reason = Reason;
            activity.Message = Message;

            await db.AddActivity(activity);
        }

        #endregion

        #region Side Functions

        public DialogResult MessageBox(string Message, string Title, ButtonsType buttonsType)
        {
            Controls_Dialogue newDialogue = new Controls_Dialogue();

            newDialogue.LoadData(Message, Title, buttonsType);

            DialogResult resoult = newDialogue.ShowDialog();

            return resoult;
        }

        public DialogResult InputField(string Message, InputFieldType fieldType)
        {
            InputField checkDialogue = new InputField();

            checkDialogue.LoadData(Message, fieldType) ;

            DialogResult resoult = checkDialogue.ShowDialog();

            return resoult;
        }
        #endregion

        #region FormDesign

        public void ChangeDesign(Form form)
        {
            if (form == null) return;
            List<Control> controls = new List<Control>();

            form.BackColor = customizeableUsers.FormBackGround;
            form.FormBorderStyle = FormBorderStyle.None;

            // Adding Form Controls To The Design
            foreach (Control control in form.Controls)
            {
                controls.Add(control);
                if (control.GetType() == typeof(Panel))
                {
                    List<Control> tmp = RecursivePanelCheck(control as Panel);
                    foreach (Control c in tmp) controls.Add(c);
                }
            }
            foreach (Control control in form.Controls)
            {
                if (control.GetType() == typeof(Panel))
                {
                    if (control.Name == "TitlePanel") control.BackColor = customizeableUsers.TitlePanelColor;
                    else if (control.Name == "Special") control.BackColor = customizeableUsers.SpecialPanel;
                    else control.BackColor = customizeableUsers.FormBackGround;
                }
            }

            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i].GetType() == typeof(Label))
                {
                    if (controls[i].MinimumSize.Width == 1) controls[i].ForeColor = customizeableUsers.HeaderFontColor;
                    else controls[i].ForeColor = customizeableUsers.TextColorTheme;

                    controls[i].BackColor = Color.Transparent;
                }
                else if(controls[i].GetType() == typeof(TextBox))
                {
                    if (controls[i].MinimumSize.Width == 1)
                    {
                        controls[i].BackColor = customizeableUsers.TextColorTheme;
                        controls[i].ForeColor = Color.Black;
                    }
                    else
                    {
                        controls[i].BackColor = customizeableUsers.FormBackGround;
                        controls[i].ForeColor = customizeableUsers.TextColorTheme;
                    }
                }
                else if(controls[i].GetType() == typeof(Button))
                {
                    Button btn = controls[i] as Button;
                    if (controls[i].MinimumSize.Width == 1)
                    {
                        btn.BackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                        btn.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                        btn.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
                    }
                    else if(controls[i].MinimumSize.Width == 2)
                    {
                        btn.BackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
                        btn.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
                        btn.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                    }
                    else if(controls[i].MinimumSize.Width == 3)
                    {
                        btn.BackColor = ChestVault.Me.customizeableUsers.ExtraButtonBG;
                        btn.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ExtraButtonBG;
                        btn.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;

                    }
                    else if (controls[i].Text == "X" || controls[i].Text == "<" || controls[i].Text == ">" || controls[i].Text == "")
                    {
                        btn.BackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                        btn.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                        btn.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
                        btn.ForeColor = customizeableUsers.TitlePanelColor;
                    }
                    else if (controls[i].Text == "-")
                    {
                        btn.BackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
                        btn.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
                        btn.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                        btn.ForeColor = customizeableUsers.TitlePanelColor;
                    }
                    btn.ForeColor = customizeableUsers.TextColorTheme;

                }
                else if (controls[i].GetType() == typeof(CheckBox))
                {
                    controls[i].BackColor = Color.Transparent;
                    controls[i].ForeColor = customizeableUsers.TextColorTheme;
                }
                else if (controls[i].GetType() == typeof(Panel))
                {
                }
                else if (controls[i].GetType() == typeof(PictureBox))
                {
                    controls[i].BackColor= controls[i].Parent.BackColor;
                }
            }
        }

        public List<Control> RecursivePanelCheck(Panel panel)
        {
            List<Control> controls = new List<Control>();
            foreach(Control control in panel.Controls)
            {
                controls.Add(control);
                if(control.GetType() == typeof(Panel))
                {
                    List<Control> tmp = (RecursivePanelCheck(control as Panel));
                    foreach(Control c in tmp)
                    {
                        controls.Add(c);
                    }
                }
            }

            return controls;
        }

        #endregion
    }
    public class CustomizeableUsers
    {
        //Form
        public Color TitlePanelColor = Color.FromArgb(50, 49, 71);
        public Color FormBackGround = Color.FromArgb(29, 28, 50);

        public Color SpecialPanel = Color.FromArgb(49, 47, 72);
        // Labels And Buttons
        public Color TextColorTheme = Color.White; // minimuesize = 3 same color as background color ;
        public Color HeaderFontColor = Color.FromArgb(255, 117, 73); // MaximSize.x = 1
        public Color SideBackGroundColor = Color.FromArgb(173, 57, 43);
        public Color ExtraButtonBG = Color.FromArgb(255, 151, 73);
        // Buttons
        public Color ButtBackGroundColor
        {
            get { return HeaderFontColor; }
        }
        public Color ButtHoverColor
        {
            get { return SideBackGroundColor; }
        }
    }
    public class SavedColors
    {
        public int red;
        public int green;
        public int blue;
    }
}
