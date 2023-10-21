﻿using System;
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
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace ChestVault
{
    public partial class Controls_Buy_Recite : Form
    {
        public Controls_Buy_Recite()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }


        private bool Saved;

        CRUD db = new CRUD();

        public PurchaseSchema ReciteInWorkEdit;

        public List<BoughtItemsSchema> Recite_items_ToAdd = new List<BoughtItemsSchema>();

        private List<ItemsSchema> SearchedItem = new List<ItemsSchema>();
        DataGrid dataGrid = new DataGrid();

        private async void SearchItem(string Value)
        {
            if(checkBox1.Checked) SearchedItem = await db.GetItem(Value);
            else SearchedItem = await db.GetItemByQR(Value);

            if (SearchedItem.Count == 0)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("هذا الصنف غير موجود", "أضافة صنف للفاتورة", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            ChestVault.Me.BuyNewRecite = this;

            BoughtItemsSchema item = new BoughtItemsSchema();
            item.Name = SearchedItem[0].Name;
            item.Id = SearchedItem[0].Id;
            item.Amount = 0;
            item.SellPrice = 0;
            item.BuyPrice = 0;
            item.ExpDate = DateTime.Now;
            ValuesSender_BougthItem form = new ValuesSender_BougthItem();
            form.LoadData(item,this);
            this.Enabled = false;
            form.Show();

        }
        public void LoadDataGrid()
        {

            DataGridColumn itemname = new DataGridColumn();
            itemname.LabelSize = new Size(200, 40);
            itemname.HeaderTitle = "أسم الصنف";
            itemname.dataType = DataGridColumn.DataType.Text;

            DataGridColumn amount = new DataGridColumn();
            amount.LabelSize = new Size(100, 40);
            amount.HeaderTitle = "الكمية";
            amount.dataType = DataGridColumn.DataType.Double;

            DataGridColumn HeadMoney = new DataGridColumn();
            HeadMoney.LabelSize = new Size(150, 40);
            HeadMoney.HeaderTitle = "سعر الشراء";
            HeadMoney.dataType = DataGridColumn.DataType.Double;

            DataGridColumn Total = new DataGridColumn();
            Total.LabelSize = new Size(150, 40);
            Total.HeaderTitle = "الاجمالي";
            Total.dataType = DataGridColumn.DataType.Double;

            DataGridColumn expdate = new DataGridColumn();
            expdate.LabelSize = new Size(200, 40);
            expdate.HeaderTitle = "الصلاحية";
            expdate.dataType = DataGridColumn.DataType.Date;

            foreach (BoughtItemsSchema a in ReciteInWorkEdit.Items)
            {
                itemname.Text.Add(a.Name);
                amount.Text.Add(a.Amount.ToString());
                HeadMoney.Text.Add(a.BuyPrice.ToString());
                Total.Text.Add((a.Amount * a.BuyPrice).ToString());
                expdate.Text.Add(a.ExpDate.ToString());
            }

            if (dataGrid.Column.Count <= 0)
            {
                dataGrid.Column = new List<DataGridColumn>();
                dataGrid.Column.Add(itemname);
                dataGrid.Column.Add(amount);
                dataGrid.Column.Add(HeadMoney);
                dataGrid.Column.Add(Total);
                dataGrid.Column.Add(expdate);
            }
            else
            {
                dataGrid.Column[0].Text = itemname.Text;
                dataGrid.Column[1].Text = amount.Text;
                dataGrid.Column[2].Text = HeadMoney.Text;
                dataGrid.Column[3].Text = Total.Text;
                dataGrid.Column[4].Text = expdate.Text;
            }

            if (dataGrid.Column[0].Label.Count > 0) dataGrid.ReloadDataGrid();
            else dataGrid.FormDesign();
        }
        public void RemoveItem(string name)
        {
            for(int i = 0; i < Recite_items_ToAdd.Count; i ++)
            {
                if(Recite_items_ToAdd[i].Name == name)
                {
                    Recite_items_ToAdd.RemoveAt(i);
                    break;
                }
            }
                ReciteInWorkEdit.Items = Recite_items_ToAdd;
                LoadDataGrid();
            ControlLabels();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchItem(textBox1.Text);
            }
        }
        private void Controls_Buy_Recite_Load(object sender, EventArgs e)
        {
            panel1.Controls.Add(dataGrid.DisplayForm(this));
            LoadComboBox();
            if (ChestVault.Me.EditReciteSchema)
            {
                ReciteInWorkEdit = ChestVault.Me.EditingSchema;
                Recite_items_ToAdd = ReciteInWorkEdit.Items;
                button1.Visible = true;
                button5.Visible = true;
                comboBox1.Text = ReciteInWorkEdit.supplier;
                comboBox1.Enabled = false;
            }
            else
            {
                ReciteInWorkEdit = new PurchaseSchema();
                ReciteInWorkEdit.Items = new List<BoughtItemsSchema>();
                button1.Visible = false;
                button5.Visible = false;
                comboBox1.Enabled = true;
            }
            LoadDataGrid();
            ControlLabels();
        }
        public async void LoadComboBox()
        {
            // Suppliers Load
            comboBox1.Items.Clear();
            string[] suppliers = await db.GetSuppliers();
            comboBox1.Items.Add("شركة عامة");
            comboBox1.Items.Add("مسترجعات");

            if(suppliers.Length > 0)
            foreach (string a in suppliers)
            {
                comboBox1.Items.Add(a);
            }
        }
        public async void ReturnItems()
        {
            // Get All Recite To Give Number
            List<PurchaseSchema> AllRecites = new List<PurchaseSchema>();

            AllRecites = await db.GetAllPurches();
            ///
            DialogResult resoult;
            // Adding Recite
            PurchaseSchema NewRecite = new PurchaseSchema();

            NewRecite.Number = AllRecites[AllRecites.Count - 1].Number + 1;
            NewRecite.Paid = double.Parse(textBox2.Text);
            NewRecite.Total = double.Parse(label3.Text);
            NewRecite.Date = (DateTime.Now.Year * 10000) + (DateTime.Now.Month * 100) + DateTime.Now.Day;
            NewRecite.Dept = NewRecite.Total - NewRecite.Paid;
            NewRecite.supplier = comboBox1.Text;
            NewRecite.Items = Recite_items_ToAdd;
            NewRecite.User = ChestVault.Me.CurrentUser.Name;
            List<ItemsSchema> tmp = new List<ItemsSchema>();
            foreach (BoughtItemsSchema a in NewRecite.Items)
            {
                List<ItemsSchema> items = await db.GetItem(a.Name);
                if (items[0].Info.Count == 0)
                {
                    resoult = ChestVault.Me.MessageBox("لايمكن حفظ الفاتورة", "أضافة فاتورة", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }
                double sum = a.Amount;
                for (int i = items[0].Info.Count - 1; i >= 0; i--)
                {
                    items[0].Info[i].Amount -= sum;
                    if (items[0].Info[i].Amount < 0)
                    {
                        sum = items[0].Info[i].Amount * -1;
                        items[0].Info.RemoveAt(i);
                    }
                    else
                    {
                        sum = 0;
                        break;
                    }
                }
                if (sum < 0)
                {
                    resoult = ChestVault.Me.MessageBox("لايمكن حفظ الفاتورة", "أضافة فاتورة", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }
                tmp.Add(items[0]);
            }
            foreach (ItemsSchema a in tmp)
            {
                await db.UpdateItem(a);
            }

            ChestVault.Me.AddActivity("تمت أضافة فاتورة رقم " + NewRecite.Number.ToString(), "Add Bougth Recite");

            await db.AddPurches(NewRecite);
            resoult = ChestVault.Me.MessageBox("تم حفظ الفاتورة بنجاح", "حفظ فاتورة", Controls_Dialogue.ButtonsType.Ok); 
        }
        public void AddNewItemToRecite(BoughtItemsSchema item)
        {
            if (Recite_items_ToAdd == null) Recite_items_ToAdd = new List<BoughtItemsSchema>();
            bool Add = true;
            for (int i = 0; i < Recite_items_ToAdd.Count; i++)
            {
                if (item.Name == Recite_items_ToAdd[i].Name)
                {
                    Recite_items_ToAdd[i] = item;
                    Add = false;
                    break;
                }
            }
            if (Add) Recite_items_ToAdd.Add(item);

            ReciteInWorkEdit.Items = Recite_items_ToAdd;
            ControlLabels();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (Saved)
            {
                ChestVault.Me.MainForm.SidePanel.CloseMiniMenu();
            }
            ChestVault.Me.EditReciteSchema = false;
            ChestVault.Me.MainForm.Enabled = true;
            this.Hide();
        }

        public void ControlLabels()
        {
            if(Recite_items_ToAdd.Count > 0)
            {
                textBox2.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label6.Visible = true;
                label7.Visible = true;

                double Sum = 0;
                foreach(BoughtItemsSchema a in Recite_items_ToAdd)
                {
                    Sum += a.Amount * a.BuyPrice;
                }

                label3.Text = Sum.ToString();

                if (!ChestVault.Me.EditReciteSchema)
                {
                    if (double.Parse(textBox2.Text) > double.Parse(label3.Text))
                    {
                        textBox2.Text = "0";
                        DialogResult resoult = ChestVault.Me.MessageBox("لا يمكنك دفع أكثر من المطلوب عليك في الفاتورة", "تعديل فاتورة", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }

                    label6.Text = (double.Parse(label3.Text) - double.Parse(textBox2.Text)).ToString();
                }
                else
                {
                    if ((double.Parse(textBox2.Text) + ReciteInWorkEdit.Paid) > double.Parse(label3.Text))
                    {
                        textBox2.Text = "0";
                        DialogResult resoult = ChestVault.Me.MessageBox("لا يمكنك دفع أكثر من المطلوب عليك في الفاتورة", "تعديل فاتورة", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }

                    label6.Text = (double.Parse(label3.Text) - (ReciteInWorkEdit.Paid + double.Parse(textBox2.Text))).ToString();
                }
            }
            else
            {
                textBox2.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label6.Visible = false;
                label7.Visible = false;

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (!Saved)
            {
                bool Checkavailable = ChestVault.Me.MainForm.SidePanel.AddMiniMenus("فاتروة مشتريات جديدة", this);
                if (Checkavailable)
                {
                    ChestVault.Me.EditReciteSchema = false;
                    Saved = true;
                    ChestVault.Me.MainForm.Enabled = true;
                    this.Hide();
                }
            }
            else
            {
                ChestVault.Me.EditReciteSchema = false;
                ChestVault.Me.MainForm.Enabled = true;
                this.Hide();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "") { textBox2.Text = "0"; return;}
            if (!ChestVault.Me.EditReciteSchema)
            {
                if (double.Parse(textBox2.Text) > double.Parse(label3.Text))
                {
                    textBox2.Text = "0";
                    DialogResult resoult = ChestVault.Me.MessageBox("لا يمكنك دفع أكثر من المطلوب عليك في الفاتورة", "تعديل فاتورة", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }

                label6.Text = (double.Parse(label3.Text) - double.Parse(textBox2.Text)).ToString();
            }
            else
            {
                if ((double.Parse(textBox2.Text) + ReciteInWorkEdit.Paid) > double.Parse(label3.Text))
                {
                    textBox2.Text = "0";
                    DialogResult resoult = ChestVault.Me.MessageBox("لا يمكنك دفع أكثر من المطلوب عليك في الفاتورة", "تعديل فاتورة", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }

                label6.Text = (double.Parse(label3.Text) - (ReciteInWorkEdit.Paid + double.Parse(textBox2.Text))).ToString();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == "")
            {
                DialogResult resoult = ChestVault.Me.MessageBox("لا يوجد موزع", "حفظ فاتورة", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            if(Recite_items_ToAdd.Count <= 0)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("لا يمكنك حفظ فاتورة خالية", "حفظ فاتورة", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            if (!ChestVault.Me.EditReciteSchema)
            {
                if (comboBox1.Text == "مسترجعات")
                {
                    ReturnItems();
                    return;
                }
                // Get All Recite To Give Number
                List<PurchaseSchema> AllRecites = new List<PurchaseSchema>();

                AllRecites = await db.GetAllPurches();
                ///

                // Adding Recite
                PurchaseSchema NewRecite = new PurchaseSchema();
                if (AllRecites.Count == 0)
                    NewRecite.Number = 1;
                else
                    NewRecite.Number = AllRecites[AllRecites.Count - 1].Number + 1;

                NewRecite.Paid = double.Parse(textBox2.Text);
                NewRecite.Total = double.Parse(label3.Text);
                NewRecite.Date = (DateTime.Now.Year * 10000) + (DateTime.Now.Month * 100) + DateTime.Now.Day;
                NewRecite.Dept = NewRecite.Total - NewRecite.Paid ;
                NewRecite.supplier = comboBox1.Text;
                NewRecite.Items = Recite_items_ToAdd;
                NewRecite.User = ChestVault.Me.CurrentUser.Name;

                await db.AddPurches(NewRecite);
                ChestVault.Me.AddActivity("تمت أضافة فاتورة رقم " + NewRecite.Number.ToString(), "Add Bougth Recite");
                foreach (BoughtItemsSchema a in NewRecite.Items)
                {
                    ItemInfo adding = new ItemInfo();
                    adding.Amount = a.Amount;
                    adding.BuyPrice = a.BuyPrice;
                    adding.ExpDate = a.ExpDate;

                    List<ItemsSchema> updateitem = await db.GetItem(a.Name);

                    if (updateitem[0].Info == null) updateitem[0].Info = new List<ItemInfo>();

                    updateitem[0].Info.Add(adding);

                    await db.UpdateItem(updateitem[0]);
            
                }
                DialogResult resoult = ChestVault.Me.MessageBox("تم حفظ الفاتورة بنجاح", "حفظ فاتورة", Controls_Dialogue.ButtonsType.Ok);
            }
            else
            {
                if (comboBox1.Text == "مسترجعات")
                {
                    DialogResult resoult = ChestVault.Me.MessageBox("لا يمكنك تعديل علي فواتير المسترجعات", "تعديل فاتورة", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }
                PurchaseSchema EditRecite = ReciteInWorkEdit;

                EditRecite.Total = double.Parse(label3.Text);
                EditRecite.Paid = (ReciteInWorkEdit.Paid + double.Parse(textBox2.Text));
                EditRecite.Dept = EditRecite.Total - EditRecite.Paid;
                EditRecite.Items = Recite_items_ToAdd;

                bool Adding = await db.UpdatePurches(EditRecite);
                ChestVault.Me.AddActivity("تم التعديل علي الفاتورة رقم " + EditRecite.Number.ToString(), "Update Bougth Recite");
                if (Adding)
                {
                    foreach (BoughtItemsSchema a in EditRecite.Items)
                    {
                        bool NewInfo = true;
                        for (int i = 0; i < ReciteInWorkEdit.Items.Count; i++)
                        {
                            if (a.Name == ReciteInWorkEdit.Items[i].Name)
                            {
                                NewInfo = false;
                                break;
                            }
                        }

                        if (!NewInfo) continue;

                        ItemInfo adding = new ItemInfo();
                        adding.Amount = a.Amount;
                        adding.BuyPrice = a.BuyPrice;
                        adding.ExpDate = a.ExpDate;

                        List<ItemsSchema> updateitem = await db.GetItem(a.Name);

                        if (updateitem[0].Info == null) updateitem[0].Info = new List<ItemInfo>();

                        updateitem[0].Info.Add(adding);

                        await db.UpdateItem(updateitem[0]);

                    }
                    DialogResult resoult = ChestVault.Me.MessageBox("تم حفظ الفاتورة بنجاح", "تعديل فاتورة", Controls_Dialogue.ButtonsType.Ok);
                }

                else
                {
                    DialogResult resoult = ChestVault.Me.MessageBox("لا يمكن تعديل الفاتورة", "تعديل فاتورة", Controls_Dialogue.ButtonsType.Ok);

                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (ReciteInWorkEdit.Items == null) ReciteInWorkEdit.Items = new List<BoughtItemsSchema>();

            bool reply = await db.DeletePurches(ReciteInWorkEdit);
            if (reply)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("تم حذف الفاتورة", "حذف فاتورة", Controls_Dialogue.ButtonsType.Ok);
                ChestVault.Me.AddActivity("تمت حذف الفاتورة رقم " + ReciteInWorkEdit.Number.ToString(), "Pay Bougth Recite");
            }
            else
            {
                DialogResult resoult = ChestVault.Me.MessageBox("لا يمكن حذف الفاتورة", "حذف فاتورة", Controls_Dialogue.ButtonsType.Ok);

            }
        }
        private async void button5_Click(object sender, EventArgs e)
        {
            // pay debt
            bool reply = await db.PayDept(ReciteInWorkEdit, double.Parse(textBox2.Text));
            if (reply)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("تم دفع المبلغ بنجاح", "تعديل فاتورة", Controls_Dialogue.ButtonsType.Ok);
                ChestVault.Me.AddActivity("تم الدفع علي الفاتورة رقم " + ReciteInWorkEdit.Number.ToString() + " بمبلغ يقدر بي " + textBox2.Text, "Pay Bougth Recite");
            }
            else
            {
                DialogResult resoult = ChestVault.Me.MessageBox("لا يمكن دفع أكثر من المطلوب له من الفاتورة", "تعديل فاتورة", Controls_Dialogue.ButtonsType.Ok);

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) pictureBox2.Image = Properties.Resources.Artboard_110;
            else pictureBox2.Image = Properties.Resources.tagblack;
        }

        private void Controls_Buy_Recite_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
        }

        private void Controls_Buy_Recite_TextChanged(object sender, EventArgs e)
        {
            ChestVault.Me.BuyNewRecite = this;

            BoughtItemsSchema item = Recite_items_ToAdd[dataGrid.DoubleClick];
            ValuesSender_BougthItem form = new ValuesSender_BougthItem();
            form.LoadData(item, this);
            this.Enabled = false;
            form.Show();
        }
        #region Draggable Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            Controls_Items form = new Controls_Items();
            form.Show();
        }
    }
}
