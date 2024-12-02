using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace datagrid
{
    public partial class FrmPass : Form
    {
        public FrmPass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f1 = new Form1();
            f1.Show();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          if (TxtPass.Text== "admin")

            {
                FormSMS fsms = new FormSMS();
                fsms.Show();
                this.Hide();
            }
            
            else
          {
              lblError.Text = "Enter Correct Password";
              lblError.ForeColor = Color.Green;

              btnExit.Visible = true;


          }
        }

        private void lblError_Click(object sender, EventArgs e)
        {

        }

        private void FrmPass_Load(object sender, EventArgs e)
        {

        }
    }
}
