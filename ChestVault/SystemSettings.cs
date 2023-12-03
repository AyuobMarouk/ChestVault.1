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
    public partial class SystemSettings : Form
    {
        public SystemSettings()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }

        public List<SettingsSchema> settings = new List<SettingsSchema>();
        CRUD db =   new CRUD();
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChestVault.Me.MainForm.FillWithMainMenu();
        }

        private async void SystemSettings_Load(object sender, EventArgs e)
        {
            settings = await db.getSetting();

            if (settings[0].ShopName == null || settings[0].ShopName == "") textBox1.Text = settings[0].ShopName;

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private async void button8_Click(object sender, EventArgs e)
        {
            settings[0].ShopName = textBox1.Text;

            await db.UpdateSetting(settings[0]);
            ChestVault.Me.CurrentSettings = settings[0];
        }
    }
}
