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
    public partial class DataGridForm : Form
    {
        public DataGridForm()
        {
            InitializeComponent();
        }
        public DataGrid ThisDataGrid;

        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (ThisDataGrid.CurrentPage < ThisDataGrid.LastPage - 1)
            {
                ThisDataGrid.CurrentPage++;
                ThisDataGrid.ReloadDataGrid();
                FixData();
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (ThisDataGrid.CurrentPage != 0)
            {
                ThisDataGrid.CurrentPage--;
                ThisDataGrid.ReloadDataGrid();
                FixData();
            }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                int Page = int.Parse(textBox1.Text) - 1;
                if(Page >= 0 && Page < ThisDataGrid.LastPage)
                {
                    ThisDataGrid.CurrentPage = Page;
                    ThisDataGrid.ReloadDataGrid();
                }
            }
        }
        public void FixData()
        {
            textBox1.Text = (ThisDataGrid.CurrentPage + 1).ToString();
            // needs math
            label1.Text =  (ThisDataGrid.CurrentPage * ThisDataGrid.DisplayLimit).ToString() + "/" + (ThisDataGrid.LastPage * ThisDataGrid.DisplayLimit).ToString();
            //
            label1.Left = this.Size.Width - label1.Size.Width;

            textBox1.Left = (this.Size.Width / 2) - (textBox1.Size.Width / 2);
            button23.Left = (this.Size.Width / 2) + (textBox1.Size.Width / 2);

            button22.Left = (this.Size.Width / 2) - ((textBox1.Size.Width / 2) + button22.Size.Width);

        }
    }
}
