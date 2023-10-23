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
    public partial class Private_QRCode : Form
    {
        public Private_QRCode()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }
        CRUD db = new CRUD();
        List<ItemsSchema> SearchedItem;
        private void Private_QRCode_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Check();
            textBox1.Select();
        }
        public async void Check()
        {
            if (ChestVault.Me.QrCodeItem == "") return;
            SearchedItem = await db.GetItemByQR(ChestVault.Me.QrCodeItem);

            if (SearchedItem.Count == 0) return;
            if (SearchedItem[0].QRcode.Count <= 1) return;

            for (int i = 1; i < SearchedItem[0].QRcode.Count; i++)
            {
                listBox1.Items.Add(SearchedItem[0].QRcode[i]);
                
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddQRcode();
        }


        public async void AddQRcode()
        {
            if (textBox1.Text == "")
            {
                return;
            }
            if (textBox1.Text.Contains(" "))
            {
                textBox1.Text = "";
                return;
            }
            foreach (var a in listBox1.Items)
            {
                if (textBox1.Text == a.ToString()) return;
            }
            List<ItemsSchema> checkqrcode = await db.GetItemByQR(textBox1.Text);
            if (checkqrcode.Count > 0)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("هذا البار كود موجود في صنف اخر", "بحث", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            listBox1.Items.Add(textBox1.Text);
            textBox1.Text = "";
            textBox1.Select();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ChestVault.Me.ItemsMenu.Enabled = true;
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.Items.Count > 0)
            {
                ChestVault.Me.qrCodes = new List<string>();
                foreach(string qrcode in listBox1.Items)
                {
                    ChestVault.Me.qrCodes.Add(qrcode);
                }
                ChestVault.Me.ItemsMenu.AddQrCodes();
            }
            listBox1.Items.Clear();
            ChestVault.Me.ItemsMenu.Enabled = true;
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
          if(e.KeyCode == Keys.Enter)
            {
                AddQRcode();
            }
        }
    }
}
