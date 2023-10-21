using ChestVault.Schemas;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ChestVault
{
    public partial class UserSettings_Colors : Form
    {
        public UserSettings_Colors()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }
        CRUD db = new CRUD();
        private UsersSchema Username;

        List<PictureBox> pictureBoxes = new List<PictureBox>();
        private void button4_Click(object sender, EventArgs e)
        {
           DialogResult a =  colorDialog1.ShowDialog();
            if (a != DialogResult.OK) return;
           pictureBox1.BackColor =  colorDialog1.Color;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult a = colorDialog1.ShowDialog();
            if (a != DialogResult.OK) return;
            pictureBox2.BackColor = colorDialog1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult a = colorDialog1.ShowDialog();
            if (a != DialogResult.OK) return;
            pictureBox3.BackColor = colorDialog1.Color;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult a = colorDialog1.ShowDialog();
            if (a != DialogResult.OK) return;
            pictureBox4.BackColor = colorDialog1.Color;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult a = colorDialog1.ShowDialog();
            if (a != DialogResult.OK) return;
            pictureBox5.BackColor = colorDialog1.Color;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult a = colorDialog1.ShowDialog();
            if (a != DialogResult.OK) return;
            pictureBox6.BackColor = colorDialog1.Color;
        }

        public async void UpdateColer()
        {
            List<SavedColors> colors = new List<SavedColors>();
            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                SavedColors s = new SavedColors();
                s.red = pictureBoxes[i].BackColor.R;
                s.blue = pictureBoxes[i].BackColor.B;
                s.green = pictureBoxes[i].BackColor.G;
                colors.Add(s);
            }
            Username.UserColor = colors;
            await db.UpdateUser(Username);
        }
        private void UserSettings_Colors_Load(object sender, EventArgs e)
        {
           foreach(Control a in this.Controls)
            {
                if(a.GetType() == typeof(PictureBox))
                {
                    pictureBoxes.Add(a as PictureBox);
                }
            }
           for(int i = 0; i < pictureBoxes.Count;i++)
            {
                for(int x = i; x < pictureBoxes.Count;x++)
                {
                    if (pictureBoxes[i].MinimumSize.Height > pictureBoxes[x].MinimumSize.Height)
                    {
                        PictureBox trans = pictureBoxes[i];
                        pictureBoxes[i] = pictureBoxes[x];
                        pictureBoxes[x] = trans;
                    }
                }
            }

        }
        public async void LoadColors(string User)
        {
            List<UsersSchema> users = await db.GetAllUsers();

            foreach (UsersSchema user in users)
            {
                if(user.Name == User)
                {
                    if (user.UserColor == null)
                    {
                        List<SavedColors> colors = new List<SavedColors>();
                        user.UserColor = colors;
                        pictureBox1.BackColor = ChestVault.Me.customizeableUsers.TitlePanelColor;
                        pictureBox2.BackColor = ChestVault.Me.customizeableUsers.FormBackGround;
                        pictureBox3.BackColor = ChestVault.Me.customizeableUsers.SpecialPanel;

                        pictureBox4.BackColor = ChestVault.Me.customizeableUsers.HeaderFontColor;
                        pictureBox5.BackColor = ChestVault.Me.customizeableUsers.TextColorTheme;

                        pictureBox6.BackColor = ChestVault.Me.customizeableUsers.SideBackGroundColor;
                        for (int i = 0; i < pictureBoxes.Count; i++)
                        {
                                SavedColors s = new SavedColors();
                                s.red = pictureBoxes[i].BackColor.R;
                                s.blue = pictureBoxes[i].BackColor.B;
                                s.green = pictureBoxes[i].BackColor.G;
                                colors.Add(s);
                        }
                        UpdateColer();
                    }
                    else
                    {
                        for(int i = 0; i < pictureBoxes.Count; i++)
                        {
                            pictureBoxes[i].BackColor = Color.FromArgb(user.UserColor[i].red, user.UserColor[i].green, user.UserColor[i].blue) ;
                        }

                    }
                    Username = user;
                }
            }

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text == "Default Theme")
            {
                CustomizeableUsers user = new CustomizeableUsers();
                pictureBox1.BackColor = user.TitlePanelColor;
                pictureBox2.BackColor = user.FormBackGround;
                pictureBox3.BackColor = user.SpecialPanel;

                pictureBox4.BackColor = user.HeaderFontColor;
                pictureBox5.BackColor = user.TextColorTheme;

                pictureBox6.BackColor = user.SideBackGroundColor;
            }
        }
    }
}
