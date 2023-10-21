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
    public partial class Controls_AllItems : Form
    {
        public Controls_AllItems()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);

        }
        List<ItemsSchema> Items;

        DataGrid dataGrid = new DataGrid();
        private bool Saved;
        CRUD db = new CRUD();
        
        public void LoadDataGrid()
        {
            DataGridColumn name = new DataGridColumn();
            name.LabelSize = new Size(200, 40);
            name.HeaderTitle = "أسم الصنف";
            name.dataType = DataGridColumn.DataType.Text;

            DataGridColumn BarCode = new DataGridColumn();
            BarCode.LabelSize = new Size(200, 40);
            BarCode.HeaderTitle = "بار كود";
            BarCode.dataType = DataGridColumn.DataType.Text;

            DataGridColumn Amount = new DataGridColumn();
            Amount.LabelSize = new Size(200, 40);
            Amount.HeaderTitle = "مخزون";
            Amount.dataType = DataGridColumn.DataType.Double;

            DataGridColumn SellPrice = new DataGridColumn();
            SellPrice.LabelSize = new Size(200, 40);
            SellPrice.HeaderTitle = "سعر البيع";
            SellPrice.dataType = DataGridColumn.DataType.Double;

            foreach (ItemsSchema a in Items)
            {
                name.Text.Add(a.Name);
                string barcode = (a.QRcode.Count > 0)? a.QRcode[0] : "بدون بار كود";
                BarCode.Text.Add(barcode);

                double amount = 0;
                if(a.Info.Count > 0)
                foreach(ItemInfo info in a.Info)
                {
                    amount += info.Amount;
                }

                Amount.Text.Add(amount.ToString());
                SellPrice.Text.Add(a.SellPrice.ToString());
            }
            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(name);
                dataGrid.Column.Add(BarCode);
                dataGrid.Column.Add(Amount);
                dataGrid.Column.Add(SellPrice);
            }
            else
            {
                dataGrid.Column[0].Text = name.Text;
                dataGrid.Column[1].Text = BarCode.Text;
                dataGrid.Column[2].Text = Amount.Text;
                dataGrid.Column[3].Text = SellPrice.Text;
            }

            if (dataGrid.Column[0].Label.Count > 0) dataGrid.ReloadDataGrid();
            else dataGrid.FormDesign();
        }
        private async void Controls_AllItems_Load(object sender, EventArgs e)
        {
            Items = await db.GetAllItems();
            panel1.Controls.Add(dataGrid.DisplayForm(this));

            LoadDataGrid();
            DrawGraphs();
            chart1.Visible = false;
        }

        public async void DrawGraphs()
        {
            // mid value
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            List<int> mid = await db.GetRecitesMonthMed(DateTime.Now, true,null);
            List<int > returns = await db.GetRecitesMonthMed(DateTime.Now, false,null);
            int count = 0;
            double midmix = 0;
            foreach (int midItem in mid)
            {
                int date = (midItem / 1000000);
                chart1.Series[0].Points.AddXY((date / 100).ToString() + "/" + (date % 100).ToString(), midItem % 1000000);
                chart1.Series[0].Points[count].Label = (midItem % 1000000).ToString();
                midmix += midItem % 1000000;
                count++;
            }
            int x = 0;
            for(int i = 0; i < returns.Count; i++)
            {
                int date = (returns[i] / 1000000);
                if ((mid[x] / 1000000).ToString() == (returns[i] / 1000000).ToString())
                {
                    chart1.Series[1].Points.AddXY((date / 100).ToString() + "/" + (date % 100).ToString(), returns[i] % 1000000);
                    chart1.Series[1].Points[x].Label = (returns[i] % 1000000).ToString();
                }
                else
                {
                    chart1.Series[1].Points.AddXY((date / 100).ToString() + "/" + (date % 100).ToString(), "0");
                    chart1.Series[1].Points[x].Label = "0";
                    i--;
                }
                x++;
            }
            midmix = midmix / count == 0 ? 1 : count;
            Graphics myGraphics = panel2.CreateGraphics();
            Pen myPen = new Pen(ChestVault.Me.customizeableUsers.ButtHoverColor, 5);
            myGraphics.DrawEllipse(myPen, 30, 100, 150, 150);
            label3.Text = midmix.ToString();


            myGraphics.DrawEllipse(myPen, 30, 295, 150, 150);
            int today = await db.GetRecitesDay(DateTime.Now, true, null);
            label4.Text = today.ToString();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (Saved)
            {
                ChestVault.Me.MainForm.SidePanel.CloseMiniMenu();
            }
            ChestVault.Me.MainForm.Enabled = true;
            this.Hide();
        }
        #region Draggable Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void TitlePanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            if (!Saved)
            {
                bool Checkavailable = ChestVault.Me.MainForm.SidePanel.AddMiniMenus("كافة الاصناف", this);
                if (Checkavailable)
                {
                    Saved = true;
                    this.Hide();
                }
            }
            else
            {
                this.Hide();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chart1.Visible = !chart1.Visible;
            if (chart1.Visible) button5.Text = ">";
            else button5.Text = "<";
        }

        private void Controls_AllItems_TextChanged(object sender, EventArgs e)
        {
            Controls_Items form = new Controls_Items();
            form.Show();
            form.SearchItem(Items[dataGrid.DoubleClick].Name, false);
        }
    }
}
