using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChestVault.Schemas;
namespace ChestVault
{
    public partial class Controls_Clients : Form
    {
        public Controls_Clients()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }

        public CRUD db = new CRUD();
        public async void LoadCustomersComboBox()
        {
            string[] names = await db.GetAllCustomers();

            comboBox3.Items.Clear();
            for (int i = 0; i < names.Count(); i++)
            {
                comboBox3.Items.Add(names[i]);
            }
        }
        private void Controls_Clients_Load(object sender, EventArgs e)
        {
            LoadCustomersComboBox();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
            this.Hide();
        }
    }
}
