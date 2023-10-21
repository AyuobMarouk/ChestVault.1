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
    public partial class Controls_Dialogue : Form
    {
        public Controls_Dialogue()
        {
            InitializeComponent();

            ChestVault.Me.ChangeDesign(this);
        }

        public enum ButtonsType
        {
            Ok,
            OkCancel,
            SureCancel
        }

        public void LoadData(string Message, string Title, ButtonsType buttonsType)
        {
            switch (buttonsType)
            {
                case ButtonsType.Ok:
                    button1.Visible = false;
                    button7.Dock = DockStyle.Fill;
                    button7.Text = "حسنا";
                    break;
                    case ButtonsType.OkCancel:
                    button1.Text = "إلغاء";
                    button7.Text = "حسنا";
                    break;
                    case ButtonsType.SureCancel:
                    button1.Text = "إلغاء";
                    button7.Text = "تأكيد";
                    break;
            }

            label1.Text = Title;
            label2.Text = Message;

        }
    }
}
