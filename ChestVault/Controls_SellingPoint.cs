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
    public partial class Controls_SellingPoint : Form
    {
        public Controls_SellingPoint()
        {
            InitializeComponent();

            List<Control> list  = new List<Control> ();

            foreach(Control c in Special.Controls) 
            {
               list.Add(c);
            }

            foreach (Control c in panel1.Controls)
            {
                list.Add(c);
            }

            ChestVault.Me.ChangeDesign(this);
        }
        public int CurrentReceit = 0;

        public Button[] ReciteButtons = new Button[5];
        CRUD db = new CRUD();

        public enum FormState
        {
            SellingPoint,
            Search
        }
        public FormState CurrentState;
        public SellingPoint_Display sellingPoint;
        public SellingPoint_Search SearchPoint;
        public List<ReceitsInSell> inSellReceit = new List<ReceitsInSell>();
        int RemoveSpot;

        public string CustomerName;
        private void Controls_SellingPoint_Load(object sender, EventArgs e)
        {
            sellingPoint = new SellingPoint_Display();
            SearchPoint = new SellingPoint_Search();
            CurrentState = FormState.SellingPoint;
            sellingPoint.TurnOn();
            SearchPoint.TurnOff();
            FillMainPanel();
            //LoadFastSellinG();

            // testing the new form
            //
            LoadCustomersComboBox();

            ReceitsInSell newrecite = new ReceitsInSell();
            newrecite.inSellReceit = new List<SoldItemsSchema>();
            inSellReceit.Add(newrecite);
            CurrentReceit = 0;
            FirstReciteLoader();
            ChestVault.Me.SellingPoint = this;
            ReciteButtons[0] = button17;
            ReciteButtons[1] = button16;
            ReciteButtons[2] = button15;
            ReciteButtons[3] = button8;
            ReciteButtons[4] = button13;
            sellingPoint.LoadDataGrid(new List<SoldItemsSchema>());
           SelectTextBox();
        }

        public void FillMainPanel()
        {
            Form fillForm;
            if (CurrentState == FormState.SellingPoint)
            {
                SearchPoint.Visible = false;
                fillForm = sellingPoint;
            }
            else
            {
                sellingPoint.Visible = false;
                fillForm = SearchPoint;
            }
            fillForm.Visible = true;
            fillForm.TopLevel = false;
            fillForm.AutoScroll = true;
            panel2.Controls.Add(fillForm);
            fillForm.Show();
            fillForm.Dock = DockStyle.Fill;
           SelectTextBox();
        }
        public async void FirstReciteLoader()
        {
            List<RecitesSchema> Recites = new List<RecitesSchema>();
            Recites = await db.GetAllRecites();

            if (Recites.Count == 0) inSellReceit[CurrentReceit].ReciteNumber = 1;
            else inSellReceit[CurrentReceit].ReciteNumber = Recites[Recites.Count - 1].Number + 1;

            ReloadReciteButtons();
            DisplayReciteNumber();
        }

        public void ChangeReciteNumbers(int Next)
        {
            for(int i = 0; i < inSellReceit.Count; i++)
            { 
                inSellReceit[i].ReciteNumber = Next + i + 1;
            }
        }
        public void DisplayReciteNumber()
        {
            label3.Text = "رقم الفاتورة : " + inSellReceit[CurrentReceit].ReciteNumber.ToString();
        }
        public void RemoveReceit()
        {
            inSellReceit.RemoveAt(CurrentReceit);

            if(inSellReceit.Count == 0)
            {
                ReceitsInSell newrecite = new ReceitsInSell();
                newrecite.inSellReceit = new List<SoldItemsSchema>();
                inSellReceit.Add(newrecite);
                CurrentReceit = 0;
                return;
            }
            else if(CurrentReceit > 0)
            {
                CurrentReceit -= 1;
                SwitchRecites(CurrentReceit);

                DisplayReciteNumber();
            }
        }
        public void SwitchRecites(int SwitchTo)
        {
            CurrentReceit = SwitchTo;
            sellingPoint.LoadDataGrid(inSellReceit[SwitchTo].inSellReceit);
            ReloadReciteButtons();
            SelectTextBox();
            Calculate();
        }
        public async void LoadCustomersComboBox()
        {
            List<DeptSchema> names = await db.GetAllCustomers();

            comboBox3.Items.Clear();
            comboBox3.Items.Add("زبون عام");
            comboBox3.Items.Add("مسترجعات");
            for (int i = 0; i < names.Count; i++)
            {
                comboBox3.Items.Add(names[i].Name);
            }
            comboBox3.Text = "زبون عام";
        }
        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<ItemsSchema> item = await db.GetItemByQR(textBox1.Text);
                if (item.Count == 0)
                {
                    textBox1.Text = "";
                    DialogResult resoult = ChestVault.Me.MessageBox("هذا الصنف غير موجود", "غير موجود", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }
                SearchItem(item[0].Name,1);
                textBox1.Text = "";
            }
        }
        public async void SearchItem(string Value, int amount)
        {
            List<ItemsSchema> SearchedItem = new List<ItemsSchema>();
            SearchedItem = await db.GetItem(Value);

            if (SearchedItem.Count == 0)
            {
                textBox1.Text = "";
                SelectTextBox();
                DialogResult resoult = ChestVault.Me.MessageBox("هذا الصنف غير موجود", "غير موجود", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            if(SearchedItem[0].Info.Count == 0 && comboBox3.Text != "مسترجعات")
            {
                textBox1.Text = "";
                SelectTextBox();
                DialogResult resoult = ChestVault.Me.MessageBox("لا يوجد مخزون للصنف", "المخزون", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            double TotalStorage = 0;

            foreach(ItemInfo a in SearchedItem[0].Info)
            {
                TotalStorage += a.Amount;
            }

            SoldItemsSchema newitem = new SoldItemsSchema();

            newitem.Id = SearchedItem[0].Id;
            newitem.Name = SearchedItem[0].Name;
            newitem.SellPrice = SearchedItem[0].SellPrice;
            newitem.Amount = amount;
            newitem.Storage = TotalStorage;
            bool Add = true;
            for (int i = 0; i < inSellReceit[CurrentReceit].inSellReceit.Count; i++)
            {
                if (newitem.Name == inSellReceit[CurrentReceit].inSellReceit[i].Name)
                {
                    if (TotalStorage < newitem.Amount + inSellReceit[CurrentReceit].inSellReceit[i].Amount && comboBox3.Text != "مسترجعات")
                    {
                        textBox1.Text = "";
                       SelectTextBox();
                        DialogResult resoult = ChestVault.Me.MessageBox("مخزون الصنف لا يفوق هذا العدد", "لا يوجد مخزون", Controls_Dialogue.ButtonsType.Ok);
                        return;
                    }

                    newitem.Amount += inSellReceit[CurrentReceit].inSellReceit[i].Amount;
                    inSellReceit[CurrentReceit].inSellReceit[i].Amount += amount;
                    inSellReceit[CurrentReceit].inSellReceit[i].Total = inSellReceit[CurrentReceit].inSellReceit[i].Amount * inSellReceit[CurrentReceit].inSellReceit[i].SellPrice;

                    newitem.info = new List<SoldInfoSchema>();
                    double tmpadd = newitem.Amount;
                    for (int x = 0; x < SearchedItem[0].Info.Count; x++)
                    {
                        double ad = (tmpadd > SearchedItem[0].Info[x].Amount) ? SearchedItem[0].Info[x].Amount : tmpadd;
                        tmpadd -= ad;
                        SoldInfoSchema addingnew = new SoldInfoSchema();
                        addingnew.Amount = ad;
                        addingnew.BuyPrice = SearchedItem[0].Info[x].BuyPrice;
                        newitem.info.Add(addingnew);
                    }
                    inSellReceit[CurrentReceit].inSellReceit[i].info = newitem.info;
                    Add = false;
                    RemoveSpot = i;
                    textBox1.Text = "";
                    SelectTextBox();
                    break;

                }
            }

            newitem.Total = SearchedItem[0].SellPrice * newitem.Amount;

            if (Add)
            {
                newitem.info = new List<SoldInfoSchema>();
                double tmpadd = newitem.Amount;
                for (int x = 0; x < SearchedItem[0].Info.Count; x++)
                {
                    double ad = (tmpadd > SearchedItem[0].Info[x].Amount) ? SearchedItem[0].Info[x].Amount : tmpadd;
                    tmpadd -= ad;
                    SoldInfoSchema addingnew = new SoldInfoSchema();
                    addingnew.Amount = ad;
                    addingnew.BuyPrice = SearchedItem[0].Info[x].BuyPrice;
                    newitem.info.Add(addingnew);
                }
                inSellReceit[CurrentReceit].inSellReceit.Add(newitem);

            }

                if(!Add && newitem.Amount <= 0)
                {
                    inSellReceit[CurrentReceit].inSellReceit.RemoveAt(RemoveSpot);
                }
            Calculate();
            sellingPoint.LoadDataGrid(inSellReceit[CurrentReceit].inSellReceit);
        }
        public async void ReturnItems()
        {
            RecitesSchema newRecite = new RecitesSchema();

            double sum = 0;
            for (int i = 0; i < inSellReceit[CurrentReceit].inSellReceit.Count; i++)
            {
                sum += inSellReceit[CurrentReceit].inSellReceit[i].Total;
            }


            // get last recite number
            List<RecitesSchema> Recites = new List<RecitesSchema>();
            Recites = await db.GetAllRecites();

            if (Recites.Count == 0)
                newRecite.Number = 1;
            else
                newRecite.Number = Recites[Recites.Count - 1].Number + 1;

            newRecite.items = inSellReceit[CurrentReceit].inSellReceit;
            newRecite.Consumer = comboBox3.Text;
            newRecite.Paid = sum;
            newRecite.Total = sum;
            newRecite.Date = (DateTime.Now.Year * 10000) + (DateTime.Now.Month * 100) + DateTime.Now.Day;
            newRecite.User = ChestVault.Me.CurrentUser.Name;


            List<WorkSchedule> work = new List<WorkSchedule>();
            work = await db.GetAllOpenSchedules();
            work[0].RecitesSold.Add(newRecite.Number);
            ChestVault.Me.AddActivity("تمت بيع فاتورة جديدة رقم " + newRecite.Number, "Sold Receit");

            foreach (SoldItemsSchema item in inSellReceit[CurrentReceit].inSellReceit)
            {
                List<ItemsSchema> items = await db.GetItem(item.Name);
                // check if its empty
                if (items[0].Info.Count == 0)
                {
                    items[0].Info = new List<ItemInfo>();
                    ItemInfo newinfo = new ItemInfo();
                    newinfo.Amount = item.Amount;

                    BoughtItemsSchema lastbougth = await db.GetSoldItem(item.Name);

                    if (lastbougth == null) return; // needs meme
                    else
                    {
                        newinfo.ExpDate = lastbougth.ExpDate;
                        newinfo.BuyPrice = lastbougth.BuyPrice;
                    }
                    items[0].Info.Add(newinfo);
                    await db.UpdateItem(items[0]);
                }
                //
                else
                {
                    items[0].Info[items[0].Info.Count - 1].Amount += item.Amount;
                    await db.UpdateItem(items[0]);
                }
            }

            await db.UpdateWorkSchedule(work[0]);

            await db.AddRecit(newRecite);
            comboBox3.Text = "زبون عام";
            RemoveReceit();
            ChangeReciteNumbers(newRecite.Number);
            SwitchRecites(CurrentReceit);
        }
        public void Calculate()
        {
            double sum = 0;

            for(int i = 0; i < inSellReceit[CurrentReceit].inSellReceit.Count;i++)
            {
                sum += inSellReceit[CurrentReceit].inSellReceit[i].Total;
            }

            sellingPoint.ChangeTotalPrice(sum);
        }
        //Save
        private async void button1_Click(object sender, EventArgs e)
        {
            if(CurrentState != FormState.SellingPoint)
            {
                ChestVault.Me.MessageBox("يرجي فتح الفاتورة", "الفائمة مغلقة", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            if(inSellReceit[CurrentReceit].inSellReceit.Count == 0)
            {
                ChestVault.Me.MessageBox("لا يمكن حفظ فاتورة خالية", "فاتورة خالية", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            if (comboBox3.Text == "مسترجعات")
            {
                ReturnItems();
                return;
            }
            RecitesSchema newRecite = new RecitesSchema();

            double sum = 0;
            for (int i = 0; i < inSellReceit[CurrentReceit].inSellReceit.Count; i++)
            {
                sum += inSellReceit[CurrentReceit].inSellReceit[i].Total;
            }


            // get last recite number
            List<RecitesSchema> Recites = new List<RecitesSchema>();
            Recites = await db.GetAllRecites();


            if (Recites.Count == 0)
                newRecite.Number = 1;
            else
                newRecite.Number = Recites[Recites.Count - 1].Number + 1;
            newRecite.items = inSellReceit[CurrentReceit].inSellReceit;
            newRecite.Consumer = comboBox3.Text;
            if (comboBox3.Text == "زبون عام") newRecite.Paid = sum;
            else newRecite.Paid = sellingPoint.PaidPrice;
            newRecite.Total = sum;
            newRecite.Date = (DateTime.Now.Year * 10000) + (DateTime.Now.Month * 100) + DateTime.Now.Day;
            newRecite.User = ChestVault.Me.CurrentUser.Name;


            List<WorkSchedule> work = new List<WorkSchedule>();
            work = await db.GetAllOpenSchedules();
            work[0].RecitesSold.Add(newRecite.Number);

            await db.UpdateWorkSchedule(work[0]);

            await db.AddRecit(newRecite);
            ChestVault.Me.AddActivity("تمت بيع فاتورة جديدة رقم " + newRecite.Number, "Sold Receit");
            if (comboBox3.Text != "زبون عام")
            {
                if (newRecite.Total != newRecite.Paid)
                {
                    List<DeptSchema> customer = await db.GetCustomer(comboBox3.Text);

                    
                    if (customer.Count == 0)
                    {
                        customer = new List<DeptSchema>();
                        DeptSchema newCustomer = new DeptSchema();
                        newCustomer.Paid = 0;
                        newCustomer.Dept = 0;
                        newCustomer.Total = 0;
                        newCustomer.Name = comboBox3.Text;
                        newCustomer.Title = "زبون";
                        newCustomer.Info = new List<DeptInfo>();

                        customer.Add(newCustomer);
                        await db.AddCustomer(newCustomer);
                    }
                        DeptInfo newinfo = new DeptInfo();
                        newinfo.Date = DateTime.Now;
                        newinfo.Amount = newRecite.Total;
                        newinfo.Paid = newRecite.Paid;
                        newinfo.User = ChestVault.Me.CurrentUser.Name;
                        newinfo.Dept = newinfo.Amount - newinfo.Paid;
                        newinfo.Type = "بضاعة";
                        newinfo.ReciteNumber = newRecite.Number;

                        if (customer[0].Info == null) customer[0].Info = new List<DeptInfo>();
                        customer[0].Info.Add(newinfo);
                        customer[0].Paid += newinfo.Paid;
                        customer[0].Total += newinfo.Amount;

                        customer[0].Dept = customer[0].Total - customer[0].Paid;
                        await db.UpdateCustomer(customer[0]);
                }
                else
                {
                    DeptSchema newCustomer = new DeptSchema();
                    newCustomer.Paid = 0;
                    newCustomer.Dept = 0;
                    newCustomer.Total = 0;
                    newCustomer.Name = comboBox3.Text;
                    newCustomer.Title = "زبون";
                    newCustomer.Info = new List<DeptInfo>();

                    await db.AddCustomer(newCustomer);
                }
            }
            foreach (SoldItemsSchema item in inSellReceit[CurrentReceit].inSellReceit)
            {
                List<ItemsSchema> items = await db.GetItem(item.Name);
                while (items[0].Info.Count != 0 && item.Amount > 0)
                {
                    if (items[0].Info[items[0].Info.Count - 1].Amount - item.Amount <= 0)
                    {
                        item.Amount -= (int)items[0].Info[items[0].Info.Count - 1].Amount;
                        items[0].Info.RemoveAt(items[0].Info.Count - 1);
                    }
                    else
                    {
                        items[0].Info[items[0].Info.Count - 1].Amount -= item.Amount;
                        item.Amount = 0;
                    }
                }
                await db.UpdateItem(items[0]);
            }

            RemoveReceit();
            ChangeReciteNumbers(newRecite.Number);
            SwitchRecites(CurrentReceit);
            LoadCustomersComboBox();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (inSellReceit.Count < 5)
            {
                ReceitsInSell newrecite = new ReceitsInSell();
                newrecite.inSellReceit = new List<SoldItemsSchema>();
                newrecite.ReciteNumber = inSellReceit[inSellReceit.Count - 1].ReciteNumber + 1;
                inSellReceit.Add(newrecite);
                int tmp = inSellReceit.Count - 1;
                SwitchRecites(tmp);
                DisplayReciteNumber();
                ReloadReciteButtons();
            }
            else
            {
                DialogResult resoult = ChestVault.Me.MessageBox("يمكنك فتح 5 فواتير كحد أقصي", "فاتورة جديد", Controls_Dialogue.ButtonsType.Ok);
            }
           SelectTextBox();
        }
        public void ReloadReciteButtons()
        {
            for(int i = 0; i < 5;i++)
            {
                if( i < inSellReceit.Count)
                {
                    ReciteButtons[i].Text = "فاتورة رقم " + inSellReceit[i].ReciteNumber;
                    ReciteButtons[i].Visible = true;
                    if(i == CurrentReceit)
                    {
                        ReciteButtons[i].Enabled = false;
                    }
                    else ReciteButtons[i].Enabled = true;
                }
                else
                {
                    ReciteButtons[i].Visible = false;
                }
            }
            DisplayReciteNumber();
        }
        public void ReciteChanger_Click(object sender, EventArgs e)
        {
            int Number = int.Parse((sender as Button).AccessibleName);
            SwitchRecites(Number);
        }

        public async void PlusItem(int Amount)
        {
            if (sellingPoint.dataGrid.Selected < 0)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("يرجي تحديد خيار للتعديل", "تحديد خانة", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            List<ItemsSchema> itemselected = new List<ItemsSchema>();

            itemselected = await db.GetItem(inSellReceit[CurrentReceit].inSellReceit[sellingPoint.dataGrid.Selected + (sellingPoint.dataGrid.CurrentPage * sellingPoint.dataGrid.DisplayLimit)].Name);
            SearchItem(itemselected[0].Name, Amount);
        }
        public async void MinusItems(int Amount)
        {
            if (sellingPoint.dataGrid.Selected < 0)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("يرجي تحديد خيار للتعديل", "تحديد خانة", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            List<ItemsSchema> itemselected = new List<ItemsSchema>();

            itemselected = await db.GetItem(inSellReceit[CurrentReceit].inSellReceit[sellingPoint.dataGrid.Selected + (sellingPoint.dataGrid.CurrentPage * sellingPoint.dataGrid.DisplayLimit)].Name);

            if (Amount + inSellReceit[CurrentReceit].inSellReceit[sellingPoint.dataGrid.Selected + (sellingPoint.dataGrid.CurrentPage * sellingPoint.dataGrid.DisplayLimit)].Amount <= 0)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("هل أنتا متأكد من مسح الصنف من الفاتورة", "مسح الصنف من الفاتورة", Controls_Dialogue.ButtonsType.SureCancel);
                if (resoult == DialogResult.OK)
                {
                    SearchItem(itemselected[0].Name, Amount);
                    return;
                }
                else return;
            }
            SearchItem(itemselected[0].Name, Amount);
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            if (CustomerName == "مسترجعات")
            {

            }
            CustomerName = comboBox3.Text;
            sellingPoint.ChangeCustomer();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChestVault.Me.MainForm.FillWithMainMenu();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (CurrentState == FormState.SellingPoint) return;
            CurrentState = FormState.SellingPoint;
            FillMainPanel();
            sellingPoint.TurnOn();
            SearchPoint.TurnOff();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (CurrentState == FormState.Search) return;
            CurrentState = FormState.Search;
            FillMainPanel();
            sellingPoint.TurnOff();
            SearchPoint.TurnOn();
        }

        #region Selecting BarCode Textbox Measurments
        private void button4_Click(object sender, EventArgs e)
        {
            SelectTextBox();
            RemoveReceit();
            SwitchRecites(CurrentReceit);
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
           SelectTextBox();
        }
        private void comboBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
               SelectTextBox();
            }
        }
        private void panel1_Enter(object sender, EventArgs e)
        {
            SelectTextBox();
        }
        private void Controls_SellingPoint_Enter(object sender, EventArgs e)
        {
            SelectTextBox();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            SelectTextBox();
        }

        public void SelectTextBox()
        {
            if (textBox1.Text.Length == 0) textBox1.Select();
            else textBox1.SelectAll();
        }
        #endregion
    }

    public class ReceitsInSell
    {
        public int ReciteNumber;
        public List<SoldItemsSchema> inSellReceit = new List<SoldItemsSchema>();
    }
}
