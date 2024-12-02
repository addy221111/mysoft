using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace datagrid
{
    public partial class FrmUpdation : Form
    {


        OleDbConnection cn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\\Weighbridge\\DataFile.mdb;Jet OLEDB:Database Password=mniaz1;");
        OleDbCommand cmd;
        OleDbCommand cmd1;
       
        OleDbDataReader dr;
        public FrmUpdation()
        {
            InitializeComponent();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void FrmUpdation_Load(object sender, EventArgs e)
        {
            cn.Open();
            OleDbCommand cmd2 = new OleDbCommand("select Login,Pass,address,WAAPI from Logins where LoginID=@SR ", cn);
            OleDbCommand cmdm = new OleDbCommand("select mask from tblmask where ID=@SR1", cn);
            cmdm.Parameters.AddWithValue("@SR1", txtm.Text);
            cmd2.Parameters.AddWithValue("@SR", txt4.Text);
            OleDbDataReader dr1;
            OleDbDataReader dr2;
            dr1 = cmd2.ExecuteReader();
            dr2 = cmdm.ExecuteReader();

            if (dr1.Read())
            {
                txtUser.Text = dr1["Login"].ToString();
                txtPass.Text = dr1["Pass"].ToString();
                txtAddress.Text = dr1["address"].ToString();
                txtWhatsapp.Text = dr1["WAAPI"].ToString();

            }

            if (dr2.Read())
            {
                txtBrand.Text = dr2["mask"].ToString();

            }
            cn.Close();


        }

        private void txtm_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "Update Logins Set Login=@Login,Pass=@Pass,address=@address,WAAPI=@WAAPI Where LoginID=@id";
            string query1 = "Update tblmask Set mask=@mask Where ID=@mid ";
            cmd = new OleDbCommand(query, cn);
            cmd1 = new OleDbCommand(query1, cn);
            cmd.Parameters.AddWithValue("@Login", txtUser.Text);
            cmd.Parameters.AddWithValue("@Pass", txtPass.Text);
            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@WAAPI", txtWhatsapp.Text);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txt4.Text));

            cmd1.Parameters.AddWithValue("@mask", txtBrand.Text);
            cmd1.Parameters.AddWithValue("@mid", Convert.ToInt32(txtm.Text));

            cn.Open();
            cmd.ExecuteNonQuery();
            cmd1.ExecuteNonQuery();
            cn.Close();
            LblError.Text = "Successfully Updated";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtBrndUpdate.Text != "")
            {

                string query = "Insert into tblmask (mask) values (@mask)";

                OleDbCommand cmd = new OleDbCommand(query, cn);
                cmd.Parameters.AddWithValue("@mask", txtBrndUpdate.Text);
                cn.Open();
                cmd.ExecuteNonQuery();
                LblError.Text = "Succesfully Added";
                LblError.ForeColor = Color.Blue;
                cn.Close();
                txtBrndUpdate.Text = "";
            }
            else
            {
                LblError.Text = "Please fill All Record";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
