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
    public partial class UserSettings_Access : Form
    {
        public UserSettings_Access()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }
    }
}
