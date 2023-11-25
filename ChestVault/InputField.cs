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
    public partial class InputField : Form
    {
        public InputField()
        {
            InitializeComponent();
            ChestVault.Me.ChangeDesign(this);
        }
        public enum InputFieldType
        {
            PasswordChecker,
            String,
            Integer,
            Double
        }

        public InputFieldType FieldType;
        public void LoadData(string Message,InputFieldType fieldType)
        {
            label3.Text = Message;

            switch(fieldType)
            {
                case InputFieldType.PasswordChecker:
                    textBox2.PasswordChar = '*';
                    break; 
                    case InputFieldType.String:

                    break;
                    case InputFieldType.Integer:

                    break;
                        case InputFieldType.Double:

                    break;
            }
            FieldType = fieldType;
        }

        private void Dialogue_Password_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(FieldType == InputFieldType.Integer)
            {

            }
            else if(FieldType == InputFieldType.Double)
            {

            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (FieldType == InputFieldType.Integer)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            else if (FieldType == InputFieldType.Double)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                           (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ChestVault.Me.InputFieldWindow = textBox2.Text;
        }
    }
}
