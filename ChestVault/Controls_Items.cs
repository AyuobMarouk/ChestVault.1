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
using System.Windows.Forms.DataVisualization.Charting;
using ChestVault;
using ChestVault.Schemas;

namespace ChestVault
{
    public partial class Controls_Items : Form
    {
        public Controls_Items()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }

        CRUD db = new CRUD();

        DataGrid dataGrid = new DataGrid();
        List<ItemsSchema> SearchedItem = new List<ItemsSchema>();

        private List<string> QRCodes = new List<string>();
        private bool Saved;
        private ItemsSchema idInWork;
        #region buttons focus and leave effect
        private void textBox1_Leave(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.Artboard_110;
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.smallbarcode;
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.tagblack;
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.tagwhite;
        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.storageblack;
            if (textBox3.Text == "") textBox3.Text = "0";
        }
        private void textBox3_Enter(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.storagewhite;
        }
        private void textBox4_Leave(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.amountblack;
            if (textBox4.Text == "") textBox4.Text = "0";
        }
        private void textBox4_Enter(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.amountwhite;
        }
        private void textBox5_Leave(object sender, EventArgs e)
        {
            pictureBox6.Image = Properties.Resources.priceblack;
            if (textBox5.Text == "") textBox5.Text = "0";
        }
        private void textBox5_Enter(object sender, EventArgs e)
        {
            pictureBox6.Image = Properties.Resources.pricewhite;
        }
        public async void DrawGraphs(string name)
        {
            // mid value
            List<int> mid = await db.GetRecitesMonthMed(DateTime.Now, true, name);

            int midmix = 0, countmid = 0 ;
            foreach(int i in mid)
            {
                midmix += i % 1000000;
                countmid++;
            }
            countmid = countmid != 0 ? countmid : 1;
            midmix = midmix / countmid;

            Graphics myGraphics = Special.CreateGraphics();
            Pen myPen = new Pen(ChestVault.Me.customizeableUsers.ButtHoverColor, 5);
            myGraphics.DrawEllipse(myPen, 50, 150, 100, 100);
            label9.Text = midmix.ToString();


            myGraphics.DrawEllipse(myPen, 50, 300, 100, 100);
            int today = await db.GetRecitesDay(DateTime.Now, true, name);
            label11.Text = today.ToString();
        }
        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            if(Saved)
            {
                ChestVault.Me.MainForm.SidePanel.CloseMiniMenu();
            }
            ChestVault.Me.MainForm.Enabled = true;
            this.Hide();

        }
        public void LoadDataGrid(List<ItemInfo> info)
        {
            DataGridColumn price = new DataGridColumn();
            price.LabelSize = new Size(150, 40);
            price.HeaderTitle = "سعر الشراء";
            price.dataType = DataGridColumn.DataType.Double;

            DataGridColumn amount = new DataGridColumn();
            amount.LabelSize = new Size(150, 40);
            amount.HeaderTitle = "الكمية";
            amount.dataType = DataGridColumn.DataType.Double;

            DataGridColumn date = new DataGridColumn();
            date.LabelSize = new Size(300, 40);
            date.HeaderTitle = "تاريخ الصلاحية";
            date.dataType = DataGridColumn.DataType.Date;

            foreach (ItemInfo a in info)
            {
                price.Text.Add(a.BuyPrice.ToString());
                amount.Text.Add(a.Amount.ToString());
                date.Text.Add(a.ExpDate.ToString());
            }

            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(price);
                dataGrid.Column.Add(amount);
                dataGrid.Column.Add(date);
            }
            else
            {
                dataGrid.Column[0].Text = price.Text;
                dataGrid.Column[1].Text = amount.Text;
                dataGrid.Column[2].Text = date.Text;
            }

            if (dataGrid.Column[0].Label.Count > 0) dataGrid.ReloadDataGrid();
            else dataGrid.FormDesign();
        }
        public async void SearchItem(string Value,bool qrcode)
        {
            if(qrcode) SearchedItem = await db.GetItemByQR(Value);
            else SearchedItem = await db.GetItem(Value);

            if (SearchedItem.Count == 0) return;

            button7.Enabled = false;
            button9.Visible = true;
            button5.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;

            idInWork = SearchedItem[0];

            textBox1.Text = SearchedItem[0].QRcode[0];
            textBox2.Text = SearchedItem[0].Name;

            double total = 0;

            LoadDataGrid(SearchedItem[0].Info);

            textBox3.Text = total.ToString();
            textBox5.Text = SearchedItem[0].SellPrice.ToString();
            textBox6.Text = SearchedItem[0].BoxSellPrice.ToString();
            textBox4.Text = SearchedItem[0].BoxSize.ToString();
            comboBox1.Text = SearchedItem[0].Type; 
            QRCodes.Clear();

            if(SearchedItem[0].QRcode.Count > 1)
                for(int i = 1; i < SearchedItem[0].QRcode.Count; i ++)
                {
                    QRCodes.Add(SearchedItem[0].QRcode[i]);
                }
            ChestVault.Me.QrCodeItem = textBox1.Text;
            DrawGraphs(textBox2.Text);
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchItem(textBox2.Text,false);
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Saved)
            {
                bool Checkavailable = ChestVault.Me.MainForm.SidePanel.AddMiniMenus("بطاقة صنف", this);
                if (Checkavailable)
                {
                    Saved = true;
                    this.Hide();
                }
            }
            else
            {
                ChestVault.Me.MainForm.Enabled = true;
                this.Hide();
            }
        }
        private void Controls_Items_Load(object sender, EventArgs e)
        {
            ResetFields();
            panel1.Controls.Add(dataGrid.DisplayForm(this));
            DrawGraphs("");
        }
        private void ResetFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            label9.Text = "0";
            label11.Text = "0";
            QRCodes.Clear();

            button7.Enabled = true;
            button5.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            textBox1.Select();
        }
        private async void button7_Click(object sender, EventArgs e)
        {
            DialogResult resoult;
            if (textBox2.Text == "")
            {
                resoult = ChestVault.Me.MessageBox("خانة الاسم فارغة", "أضافة صنف", Controls_Dialogue.ButtonsType.Ok);                return;
            }
            if (textBox2.Text[0] == ' ' || textBox1.Text[0] == ' ')
            {
                resoult = ChestVault.Me.MessageBox("لا يمكن ان يكون الحرف الاول فراغ", "أضافة صنف", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            // Search First
            List<ItemsSchema> checkName = await db.GetItem(textBox2.Text);

            if (checkName.Count > 0)
            {
                resoult = ChestVault.Me.MessageBox("هذا الصنف موجود", "أضافة صنف", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            // Search BarCode
            List<ItemsSchema> CheckBarCode = await db.GetItemByQR(textBox1.Text);

            if (CheckBarCode.Count > 0)
            {
                resoult = ChestVault.Me.MessageBox("هذا البار كود موجود في احد الاصناف", "أضافة صنف", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            // Search Listbox BarCodes

            List<ItemsSchema> CheckBarCodes;
            foreach (string c in QRCodes)
            {
                CheckBarCodes = new List<ItemsSchema>();
                CheckBarCodes = await db.GetItemByQR(c);

                if (CheckBarCodes.Count > 0)
                {
                    DialogResult resoul = ChestVault.Me.MessageBox("إحدي البار الكود الفرعي موجود في صنف أخر", "أضافة صنف", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }
            }
            //Adding the item
            ItemsSchema newItem = new ItemsSchema();
            newItem.Name = textBox2.Text;
            newItem.SellPrice = double.Parse(textBox5.Text);
            newItem.BoxSellPrice = double.Parse(textBox6.Text);
            newItem.BoxSize = double.Parse(textBox4.Text);
            newItem.QRcode = new List<string>();
            newItem.Info = new List<ItemInfo>();
            newItem.Type = (comboBox1.Text != "")? comboBox1.Text : "غدائية";
            if(textBox1.Text != "") newItem.QRcode.Add(textBox1.Text);
            if (QRCodes.Count > 0)
            {
                for (int i = 1; i <= QRCodes.Count; i++)
                {
                    newItem.QRcode.Add(QRCodes[i - 1].ToString());
                }
            }
            ChestVault.Me.AddActivity("تمت أضافة صنف " + newItem.Name, "Add Item");
            await db.Additem(newItem);
            if(newItem.QRcode.Count ==0)
            {
                List<FastSellSchema> items = new List<FastSellSchema>();
                items = await db.GetFastSellsByName(newItem.Name);

                if(items.Count == 0)
                {
                    FastSellSchema newFASTSell = new FastSellSchema();

                    newFASTSell.Name = newItem.Name;
                    newFASTSell.Menu = "بدون باركود";

                    await db.AddFastSellItem(newFASTSell);
                }

            }
            resoult = ChestVault.Me.MessageBox("تمت أضافة صنف جديد", "أضافة صنف", Controls_Dialogue.ButtonsType.Ok);
            ResetFields();

        }
        public void AddQrCodes()
        {
            QRCodes.Clear();
            foreach (string qrcode in ChestVault.Me.qrCodes)
            {
                QRCodes.Add(qrcode);  
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            // Search First
            List<ItemsSchema> checkName = await db.GetItem(textBox2.Text);

            if (checkName.Count > 0 && checkName[0].Id != idInWork.Id)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("هذا الصنف موجود", "تعديل الصنف", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            // Search BarCode
            List<ItemsSchema> CheckBarCode = await db.GetItemByQR(textBox1.Text);

            if (CheckBarCode.Count > 0 && CheckBarCode[0].Id != idInWork.Id)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("هذا البار كود موجود في احد الاصناف", "تعديل الصنف", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            // Search Listbox BarCodes

            List<ItemsSchema> CheckBarCodes;
            foreach (string c in QRCodes)
            {
                CheckBarCodes = new List<ItemsSchema>();
                CheckBarCodes = await db.GetItemByQR(c);

                if (CheckBarCodes.Count > 0 && CheckBarCodes[0].Id != idInWork.Id)
                {
                    DialogResult resoult = ChestVault.Me.MessageBox("إحدي البار الكود الفرعي موجود في صنف أخر", "تعديل الصنف", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }
            }
            //updating the item
            ItemsSchema updateitem = new ItemsSchema();
            updateitem = idInWork;
            updateitem.Name = textBox2.Text;
            updateitem.SellPrice = double.Parse(textBox5.Text);
            updateitem.BoxSellPrice = double.Parse(textBox6.Text);
            updateitem.BoxSize = double.Parse(textBox4.Text);
            updateitem.Type = (comboBox1.Text != "") ? comboBox1.Text : "غدائية";
            if (updateitem.Info == null) updateitem.Info = new List<ItemInfo>();
            updateitem.QRcode = new List<string>();
            updateitem.QRcode.Add(textBox1.Text);
            if (QRCodes.Count > 0)
            {
                for (int i = 1; i <= QRCodes.Count; i++)
                {
                    updateitem.QRcode.Add(QRCodes[i - 1].ToString());
                }
            }
            await db.UpdateItem(updateitem);
            ChestVault.Me.AddActivity("تمت تعديل صنف " + updateitem.Name, "Edit Item");
            ResetFields();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchItem(textBox1.Text, true);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button7.Enabled = true;
            button9.Visible = false;
            button5.Enabled = false;
            button8.Enabled = false;
            ResetFields();
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            bool resoult = await db.DeleteItem(idInWork);
            if (resoult)
            {
                ChestVault.Me.AddActivity("تمت حذف صنف " + idInWork.Name, "Delete Item");
                ResetFields();
                return;
            }
            DialogResult resoul = ChestVault.Me.MessageBox("لا يمكن حذف هذا الصنف لان المخزون اكبر من 0", "حذف الصنف", Controls_Dialogue.ButtonsType.Ok);
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Controls_Items_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
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

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            ChestVault.Me.ItemsMenu = this;
            Private_QRCode form = new Private_QRCode();
            form.Show();
            this.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // print
        }
    }
}
