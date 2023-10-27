using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChestVault
{
    public partial class Addon_PriceHistory : Form
    {
        public Addon_PriceHistory()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChestVault.Me.MainForm.Enabled = true;
            this.Hide();
        }
    }
}
