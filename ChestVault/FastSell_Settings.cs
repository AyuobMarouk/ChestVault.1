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
    public partial class FastSell_Settings : Form
    {
        public FastSell_Settings()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }
        CRUD db = new CRUD();

        List<ItemsSchema> item = new List<ItemsSchema>();
        private void FastSell_Settings_Load(object sender, EventArgs e)
        {
            comboBox3.Text = "أضافة";
            LoadlistBox();
            loadItems();
        }
        private async void loadItems()
        {
            item = await db.GetAllItems();
        }
        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            if(comboBox3.Text == "أضافة")
            {
                panel1.Visible = true;
                panel2.Visible = false;
            }
            else
            {
                panel1.Visible = false;
                panel2.Visible = true;
                LoadlistBox();
            }
        }
        public async void LoadlistBox()
        {

            List<FastSellSchema> items = new List<FastSellSchema>();
            items = await db.GetAllFastSells();
            label8.Text = "أصناف البيع السريع";
            listBox1.Items.Clear();
            foreach(FastSellSchema a in items)
            {
                listBox1.Items.Add(a.Name);
            }    
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            DialogResult resoult;
            if (textBox2.Text == "")
            {
                resoult = ChestVault.Me.MessageBox("خانة الاسم فارغة", "خانة فارغة", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            if (comboBox2.Text == "")
            {
                resoult = ChestVault.Me.MessageBox("خانة القائمة فارغة", "خانة فارغة", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            List<ItemsSchema> checkName = await db.GetItem(textBox2.Text);

            if (checkName.Count == 0)
            {
                resoult = ChestVault.Me.MessageBox("هذا الصنف غير موجود", "صنف غير موجود", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
            List<FastSellSchema> items = new List<FastSellSchema>();
            items = await db.GetFastSellsByName(textBox2.Text);

            if(items.Count > 0)
            {
                resoult = ChestVault.Me.MessageBox("هذا الصنف موجود في قائمة البيع السريع", "قائمة البيع السريع", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            FastSellSchema newFASTSell = new FastSellSchema();

            newFASTSell.Name = textBox2.Text;
            newFASTSell.Menu = comboBox2.Text;

            await db.AddFastSellItem(newFASTSell);
            resoult = ChestVault.Me.MessageBox("تمت أضافة بيع سريع للصنف", "قائمة البيع السريع", Controls_Dialogue.ButtonsType.Ok);
            LoadlistBox();

        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex >=0)
            {
                if (comboBox3.Text == "تعديل")
                {
                    label7.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                    List<FastSellSchema> items = new List<FastSellSchema>();
                    items = await db.GetFastSellsByName(label7.Text);

                    comboBox4.Text = items[0].Menu;
                }
                else if (label8.Text == "كافة الاصناف")
                {
                    textBox2.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                    LoadlistBox();
                }
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
               DialogResult resoult = ChestVault.Me.MessageBox("يرجي أختيار صنف للحذف", "حذف الصنف", Controls_Dialogue.ButtonsType.Ok);
                return;
            }
                List<FastSellSchema> items = new List<FastSellSchema>();
                items = await db.GetFastSellsByName(label7.Text);

            if (items[0].Menu == "بدون باركود")
            {
                DialogResult resoult = ChestVault.Me.MessageBox("لا يمكن حذف الاصناف التي لا تحتوي علي بار كود", "حذف الصنف", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            await db.DeleteFastSell(items[0]);
            ChestVault.Me.AddActivity("تمت حذف بيع سريع للصنف " + items[0].Name, "Fast Sell");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                DialogResult resoult = ChestVault.Me.MessageBox("يرجي أختيار صنف للتعديل", "تعديل الصنف", Controls_Dialogue.ButtonsType.Ok);                return;
            }
            List<FastSellSchema> items = new List<FastSellSchema>();
            items = await db.GetFastSellsByName(label7.Text);

            if (items[0].Menu == "بدون باركود")
            {
                DialogResult resoult = ChestVault.Me.MessageBox("لا يمكن تعديل الاصناف التي لا تحتوي علي بار كود", "تعديل الصنف", Controls_Dialogue.ButtonsType.Ok);
                return;
            }

            items[0].Menu = comboBox4.Text;
            await db.UpdateFastSell(items[0]);
            ChestVault.Me.AddActivity("تمت تعديل بيع سريع للصنف " + items[0].Name, "Fast Sell");
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                LoadlistBox();
                return;
            }
            label8.Text = "كافة الاصناف";
            listBox1.Items.Clear();
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].Name.Contains(textBox2.Text)) listBox1.Items.Add(item[i].Name);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
