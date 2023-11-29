using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChestVault
{
    public partial class SearchMenu : Form
    {
        public SearchMenu()
        {
            InitializeComponent();
        }
        Point LocalPosition;
        Size LocalScale;
        public List<Label> labels = new List<Label>();

        public Form SelectedForm;

        public string SelectedValue;
        public void Setup(Point Position,Size Scale, Form form)
        {
            labels.Add(label1);
            labels.Add(label2);
            labels.Add(label3);
            labels.Add(label4);
            labels.Add(label5);
            labels.Add(label6);
            labels.Add(label7);
            labels.Add(label8);
            labels.Add(label9);
            labels.Add(label10);
            foreach (Label label in labels)
            {
                label.Text = "";
                label.BackColor = ChestVault.Me.customizeableUsers.SpecialPanel;
                label.ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            }

            LocalPosition = Position;
            LocalScale = Scale;
            SelectedForm = form;

            this.TopMost = true;
            this.Owner = form;
            this.BackColor = ChestVault.Me.customizeableUsers.FormBackGround;
        }
        public void DisplayText(List<string> Values)
        {
            int LastSpot = 0;
            for(int i = 0; i < 10; i++)
            {
                if(i < Values.Count)
                {
                    labels[i].Text = Values[i];
                    LastSpot = i;
                }
                else
                {
                    labels[i].Text = "";
                }
            }

            if (LastSpot == 0) LastSpot = 1;
            Size thisSize = new Size();
            thisSize.Width = this.Width;

            thisSize.Height = LastSpot * 25;

            this.Size = thisSize;
        }
        private void SearchMenu_Shown(object sender, EventArgs e)
        {
            this.Left = LocalPosition.X - (LocalScale.Width / 2);
            this.Top = LocalPosition.Y + LocalScale.Height;
        }
        private void LabelClick(object sender, EventArgs e)
        {
            SelectedValue = (sender as Label).Text;
            SelectedForm.Text = "Search";
        }
        private void label1_MouseEnter(object sender, EventArgs e)
        {
            for(int i = 0; i < labels.Count; i++) 
            {
                if (labels[i] != (sender as Label))
                {
                    (sender as Label).BackColor = ChestVault.Me.customizeableUsers.SpecialPanel;
                    (sender as Label).ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;
                }
                else
                {
                    (sender as Label).BackColor = ChestVault.Me.customizeableUsers.SpecialPanel;
                    (sender as Label).ForeColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                }
            }
        }
    }
}
