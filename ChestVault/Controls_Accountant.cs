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
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;
using ChestVault;
using static MongoDB.Driver.WriteConcern;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Security.Cryptography;

namespace ChestVault
{
    public partial class Controls_Accountant : Form
    {
        public Controls_Accountant()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);

        }

        public GraphAcount resoult = new GraphAcount();
        CRUD db = new CRUD();

        DataGrid UsersGrid = new DataGrid();
        DataGrid ItemsGrid = new DataGrid();
        public bool SoldItems = true;
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChestVault.Me.MainForm.FillWithMainMenu();
        }
        public async void SpecificSearch(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.BackColor == ChestVault.Me.customizeableUsers.ButtHoverColor) return;
            int amount = (sender as Button).MinimumSize.Height;
            resoult = await db.Accountant(DateTime.Now, amount, DateTime.MinValue);
            ActiveButtons(sender, e);
            DrawGraph();
        }

        public void ActiveButtons(object sender, EventArgs e)
        {
           foreach(Control a in this.Controls)
            {
                if (a is Button && a == sender)
                {
                    Button btn = a as Button;
                    btn.BackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
                    btn.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
                    btn.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                   
                }
                else
                {
                    if (a is Button && a.MinimumSize.Height != 10)
                    {
                        Button btn = a as Button;
                        btn.BackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                        btn.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
                        btn.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
                    }
                }
            }
        }
        public void DrawGraph()
        {
            // CustomizeColors
            chart2.BackColor = ChestVault.Me.customizeableUsers.TitlePanelColor;
            chart2.ChartAreas[0].BackColor = ChestVault.Me.customizeableUsers.TitlePanelColor;

            chart1.BackColor = ChestVault.Me.customizeableUsers.TitlePanelColor;
            chart1.ChartAreas[0].BackColor = ChestVault.Me.customizeableUsers.TitlePanelColor;
            chart1.Series[0].BorderColor = ChestVault.Me.customizeableUsers.TitlePanelColor;

            chart3.BackColor = ChestVault.Me.customizeableUsers.TitlePanelColor;
            chart3.ChartAreas[0].BackColor = ChestVault.Me.customizeableUsers.TitlePanelColor;

            chart3.Series[0].Color = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            chart3.Series[0].BackSecondaryColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            chart3.Legends[0].ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart3.Legends[0].BackColor = Color.Transparent;
            chart3.Series[0].BorderColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;

            chart3.ChartAreas[0].AxisX.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart3.ChartAreas[0].AxisY.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;

            chart3.ChartAreas[0].AxisX.MajorTickMark.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart3.ChartAreas[0].AxisY.MajorTickMark.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;

            chart3.ChartAreas[0].AxisX.LabelStyle.ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart3.ChartAreas[0].AxisY.LabelStyle.ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;

            chart3.ChartAreas[0].AxisX.MajorGrid.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart3.ChartAreas[0].AxisY.MajorGrid.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;


            // change the color of the gradiant as well
            chart2.Series[0].Color = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            chart2.Series[0].BackSecondaryColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            chart2.Legends[0].ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart2.Legends[0].BackColor = Color.Transparent;
            chart2.Series[0].BorderColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;

            chart2.ChartAreas[0].AxisX.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart2.ChartAreas[0].AxisY.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;

            chart2.ChartAreas[0].AxisX.MajorTickMark.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart2.ChartAreas[0].AxisY.MajorTickMark.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;

            chart2.ChartAreas[0].AxisX.LabelStyle.ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart2.ChartAreas[0].AxisY.LabelStyle.ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;

            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = ChestVault.Me.customizeableUsers.TextColorTheme;

            chart1.Series[0].Color = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            chart1.Series[0].BackSecondaryColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            chart1.Legends[0].ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart1.Legends[0].BackColor = Color.Transparent;

            chart3.Series[0].Color = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            chart3.Series[0].BackSecondaryColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            chart3.Legends[0].ForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;
            chart3.Legends[0].BackColor = Color.Transparent;

            // end of customized colors
            chart2.Series[0].Points.Clear();
            chart1.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();

            chart2.Series[0].Points.AddXY(" ",0);
            chart2.Series[0].Points[0].Label = " ";

            label2.Text = resoult.CountSell.ToString();

            foreach (int a in resoult.Sold)
            {
                int date = (a / 1000000);
                chart2.Series[0].Points.AddXY((date / 100).ToString() + "/" + (date % 100).ToString(), a % 1000000);
                chart2.Series[0].Points[chart2.Series[0].Points.Count - 1].LabelForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;
                chart2.Series[0].Points[chart2.Series[0].Points.Count - 1].Label = (a % 1000000).ToString();
            }
            /*
            int x = 0;
            for (int i = 0; i < resoult.Retuned.Count; i++)
            {
                int date = (resoult.Retuned[i] / 1000000);
                if ((resoult.Retuned[x] / 1000000).ToString() == (resoult.Retuned[i] / 1000000).ToString())
                {
                    chart2.Series[1].Points.AddXY((date / 100).ToString() + "/" + (date % 100).ToString(), resoult.Retuned[i] % 1000000);
                    chart2.Series[1].Points[x].Label = (resoult.Retuned[i] % 1000000).ToString();
                }
                else
                {
                    chart2.Series[1].Points.AddXY((date / 100).ToString() + "/" + (date % 100).ToString(), "0");
                    chart2.Series[1].Points[x].Label = "0";
                    i--;
                }
                x++;
            }
            */
            chart1.Series[0].Points.AddXY("رأس المال :" + resoult.buySell.ToString(), resoult.buySell);
            chart1.Series[0].Points[0].LabelForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;

            chart1.Series[0].Points.AddXY("المكسب :" + resoult.NetSell.ToString(), resoult.NetSell);
            chart1.Series[0].Points[1].LabelForeColor = ChestVault.Me.customizeableUsers.TextColorTheme;

            string[] days = { "احد", "أثنين", "ثلاثاء", "أربعاء", "خميس", "جمعة", "سبت" };
            for (int i = 6; i >= 0; i--)
            {
                chart3.Series[0].Points.AddXY(days[i], resoult.WeekMid.Sell[i]);
            }

            label5.Text = resoult.TotalSell.ToString();
            label7.Text = resoult.NetSell.ToString();

            LoadUsersGrid();
            LoadItemsGrid();
        }
        public void LoadUsersGrid()
        {
            UsersGrid.SelectAble = false;
            UsersGrid.DisplayLimit = 3;

            UsersGrid.HeaderFontColor = Color.White;
            UsersGrid.HeaderBackGroundColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            UsersGrid.FontColor = Color.White;
            UsersGrid.backGroundColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            UsersGrid.Back2Color = ChestVault.Me.customizeableUsers.ExtraButtonBG;

            DataGridColumn name = new DataGridColumn();
            name.LabelSize = new Size(200, 40);
            name.HeaderTitle = "أسم المستخدم";
            name.dataType = DataGridColumn.DataType.Text;

            DataGridColumn TotalSold = new DataGridColumn();
            TotalSold.LabelSize = new Size(184, 40);
            TotalSold.HeaderTitle = "الاجمالي";
            TotalSold.dataType = DataGridColumn.DataType.Double;

            foreach (UsersLog a in resoult.usersLogs)
            {
                name.Text.Add(a.Name);
                TotalSold.Text.Add(a.TotalSell.ToString());
            }
            if (UsersGrid.Column.Count <= 0)
            {
                UsersGrid.Column = new List<DataGridColumn>();
                UsersGrid.Column.Add(name);
                UsersGrid.Column.Add(TotalSold);
            }
            else
            {
                UsersGrid.Column[0].Text = name.Text;
                UsersGrid.Column[1].Text = TotalSold.Text;
            }

            UsersGrid.ReloadDataGrid();

            UsersGrid.Sort(1, 2);
            UsersGrid.Sort(1, 2);
        }
        public void LoadItemsGrid()
        {
            ItemsGrid.SelectAble = false;
            ItemsGrid.DisplayLimit = 10;

            ItemsGrid.HeaderFontColor = Color.White;
            ItemsGrid.HeaderBackGroundColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            ItemsGrid.FontColor = Color.White;
            ItemsGrid.backGroundColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            ItemsGrid.Back2Color = ChestVault.Me.customizeableUsers.ExtraButtonBG;

            DataGridColumn name = new DataGridColumn();
            name.LabelSize = new Size(200, 40);
            name.HeaderTitle = "أسم الصنف";
            name.dataType = DataGridColumn.DataType.Text;

            DataGridColumn TotalSold = new DataGridColumn();
            TotalSold.LabelSize = new Size(184, 40);
            TotalSold.HeaderTitle = "الاكثر مبيعا";
            TotalSold.dataType = DataGridColumn.DataType.Double;

            if (SoldItems)
            {
                foreach (itemslog a in resoult.itemslog)
                {
                    name.Text.Add(a.Name);
                    TotalSold.Text.Add(a.AmountSell.ToString());

                }
            }
            else
            {
                foreach (RemainedItemsInfo a in resoult.RemainedItems)
                {
                    name.Text.Add(a.Name);
                    TotalSold.Text.Add(a.count.ToString());

                }
            }
            if (ItemsGrid.Column.Count <= 0)
            {
                ItemsGrid.Column = new List<DataGridColumn>();
                ItemsGrid.Column.Add(name);
                ItemsGrid.Column.Add(TotalSold);
            }
            else
            {
                ItemsGrid.Column[1].HeaderTitle = (SoldItems) ? "الاكثر مبيعا" : "المخزون";
                ItemsGrid.Column[0].Text = name.Text;
                ItemsGrid.Column[1].Text = TotalSold.Text;
            }

            ItemsGrid.ReloadDataGrid();

            ItemsGrid.Sort(1, 2);
            ItemsGrid.Sort(1, 2);
        }
        private async void button4_Click(object sender, EventArgs e)
        {
            resoult = await db.Accountant(dateTimePicker1.Value, 0, dateTimePicker2.Value);
            ActiveButtons(sender, e);
            DrawGraph();
        }

        private async void Controls_Accountant_Load(object sender, EventArgs e)
        {
            panel4.Controls.Add(UsersGrid.DisplayForm(this));
            panel5.Controls.Add(ItemsGrid.DisplayForm(this));

            button1.BackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            button1.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            button1.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;

            resoult = await db.Accountant(DateTime.Now, 7, DateTime.MinValue);
            DrawGraph();
            label6.Text = dateTimePicker1.Text;
            label13.Text = dateTimePicker2.Text;
            SoldItems = true;

            button5.BackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            button5.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            button5.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Select();
            SendKeys.Send("%{DOWN}");
        }

        private void label13_Click(object sender, EventArgs e)
        {
            dateTimePicker2.Select();
            SendKeys.Send("%{DOWN}");
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            label6.Text = dateTimePicker1.Text;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            label13.Text = dateTimePicker2.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.BackColor == ChestVault.Me.customizeableUsers.ButtHoverColor) return;
            SoldItems = true;
            button5.BackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            button5.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            button5.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;

            button6.BackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            button6.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            button6.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;

            LoadItemsGrid();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.BackColor == ChestVault.Me.customizeableUsers.ButtHoverColor) return;
            SoldItems = false;
            button6.BackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            button6.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            button6.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;

            button5.BackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            button5.FlatAppearance.MouseDownBackColor = ChestVault.Me.customizeableUsers.ButtBackGroundColor;
            button5.FlatAppearance.MouseOverBackColor = ChestVault.Me.customizeableUsers.ButtHoverColor;
            LoadItemsGrid();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }
    }
}
