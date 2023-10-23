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

        private Controls_Items items;
        private void Private_QRCode_Load(object sender, EventArgs e)
        {
            textBox1.Select();
        }
        public void Check(List<string> SubCodes, Controls_Items form)
        {
            listBox1.Items.Clear();
            items = form;
            if (SubCodes.Count == 0) return;

            for (int i = 0; i < SubCodes.Count; i++)
            {
                    listBox1.Items.Add(SubCodes[i]);
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
                if (textBox1.Text == a.ToString())
                {
                    ChestVault.Me.MessageBox("هذا البار موجود في القائمة", "تنبيه", Controls_Dialogue.ButtonsType.Ok);
                    return;
                }
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
            items.Enabled = true;
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> sendover = new List<string>();
            if(listBox1.Items.Count > 0)
            {
                foreach(string a in listBox1.Items)
                {
                    sendover.Add(a);
                }
                items.AddQrCodes(sendover);
            }
            items.Enabled = true;
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
        private void Private_QRCode_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
