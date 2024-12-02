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
    public partial class FrmCPass : Form
    {
        public FrmCPass()
        {
            InitializeComponent();
        }

        private void TxtPass_TextChanged(object sender, EventArgs e)
        {
            
            if (TxtPass.Text == "super")
            {

                FrmUpdation fu = new FrmUpdation();
                fu.Show();
                this.Hide();

            }
            else
            {
                lblError.Text = "Enter Correct Password";
                lblError.ForeColor = Color.Green;

                btnExit.Visible = true;


            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f1 = new Form1();
            f1.Show();
        }
    }
}
