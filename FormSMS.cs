using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Windows;
using System.Data.OleDb;
using System.Xml;



namespace datagrid
{
    public partial class FormSMS : Form
    {

        OleDbConnection cn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\\Weighbridge\\DataFile.mdb;Jet OLEDB:Database Password=mniaz1;");
        
        public FormSMS()
        {
            InitializeComponent();
        }


        private String sendRequest(String url)
        {
            String response = null;
            try
            {
                var client = new WebClient();
                response = client.DownloadString(url);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(response);
                XmlNodeList responseType = xmldoc.GetElementsByTagName("response");
                XmlNodeList data = xmldoc.GetElementsByTagName("data");
                if (responseType.Equals("Error"))
                {
                    return null;
                }
                response = data[0].InnerText;
                return response;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //Console.WriteLine(e.Message);
            }
            return null;
        }

        private String getSessionId(String msisdn, String password)
        {
            String url = "https://telenorcsms.com.pk:27677/corporate_sms2/api/auth.jsp?msisdn=" +
           msisdn + "&password=" + password;
            return sendRequest(url);
        }

        private String sendQuickMessage(String sessionId, String messageText, String to, String mask)
        {
            String url = "https://telenorcsms.com.pk:27677/corporate_sms2/api/sendsms.jsp?session_id=" + sessionId +
           "&text=" + messageText + "&to=" + to;
            //String url = "https://techrapidsolution.com/rapidbulksms/Api/send_message" + sessionId + "&to=" + to;

            if (mask != null)
            {
                url = url += "&mask=" + mask;
            }
            return sendRequest(url);
        }
        private void FormSMS_Load(object sender, EventArgs e)
        {

            cn.Open();
            OleDbCommand cmd3 = new OleDbCommand("select Login,Pass,WAAPI from Logins where LoginID=@SR ", cn);
            cmd3.Parameters.AddWithValue("@SR", txtlg.Text);
            OleDbDataReader dr3;
            dr3 = cmd3.ExecuteReader();

            if (dr3.Read())
            {
                txtUser.Text = dr3["Login"].ToString();
                txtPass.Text = dr3["Pass"].ToString();
                txtWhatsapp.Text = dr3["WAAPI"].ToString();







            }
            else
            {
                txtUser.Text = "";
                txtPass.Text = "";





            }

            cn.Close();




            cn.Open();
            OleDbCommand cmd2 = new OleDbCommand("select mask from tblmask where ID=@SR ", cn);
            cmd2.Parameters.AddWithValue("@SR", txtam.Text);
            OleDbDataReader dr2;
            dr2 = cmd2.ExecuteReader();

            if (dr2.Read())
            {
                txtBrand.Text = dr2["mask"].ToString();
                comboBox1.Items.Add(dr2["mask"].ToString());






            }
            else
            {
                txtBrand.Text = "NIAZ";





            }

            cn.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                String messageText, to, mask, userName, password, sessionId, messageIds;

                userName = txtUser.Text;
                password = txtPass.Text ;
                messageText = txtMsg.Text;
                mask = txtBrand.Text;
                to = txtMobile.Text;

                sessionId = getSessionId(userName, password);
                if (sessionId != null)
                {
                    messageIds = sendQuickMessage(sessionId, messageText, to, mask);
                    if (messageIds.Contains("Error"))
                    {
                        MessageBox.Show("Messages Not Sent!!", "Acknowledgement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      

                        //Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show("Messages Sent!!", "Acknowledgement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).TextLength == 4)
                SendKeys.Send("{Tab}");
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
           
  
           
            }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).TextLength == 4)
                SendKeys.Send("{Tab}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void txtam_TextChanged(object sender, EventArgs e)
        {
            cn.Open();
            OleDbCommand cmd2 = new OleDbCommand("select mask from tblmask where ID=@SR ", cn);
            cmd2.Parameters.AddWithValue("@SR", txtam.Text);
            OleDbDataReader dr2;
            dr2 = cmd2.ExecuteReader();

            if (dr2.Read())
            {
                txtBrand.Text = dr2["mask"].ToString();







            }
            else
            {
                txtBrand.Text = "NIAZ";







            }

            cn.Close();
        }

        private void txtBrand_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            cn.Open();
            OleDbCommand cmd3 = new OleDbCommand("select Login,Pass from Logins where LoginID=@SR ", cn);
            cmd3.Parameters.AddWithValue("@SR", txtlg.Text);
            OleDbDataReader dr3;
            dr3 = cmd3.ExecuteReader();

            if (dr3.Read())
            {
                txtUser.Text = dr3["Login"].ToString();
                txtPass.Text = dr3["Pass"].ToString();







            }
            else
            {
                txtUser.Text = "";
                txtPass.Text = "";





            }

            cn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox2.Show();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            pictureBox2.Hide();
        }

        private void txtLng_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLng_Click(object sender, EventArgs e)
        {
            if (txtLng.Text == "English")
            {

                txtLng.Text = "Urdu";

            }
            else if (txtLng.Text == "Urdu")
            {
                txtLng.Text = "English";

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {

            if (txtLng.Text == "English")
            {

                txtLng.Text = "Urdu";

            }
            else if (txtLng.Text == "Urdu")
            {
                txtLng.Text = "English";

            }


            if (checkBox1.Text == "Urdu")
            {

                checkBox1.Text = "English";

            }
            else if (checkBox1.Text == "English")
            {
                checkBox1.Text = "Urdu";

            }

        }

        private void txtam_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtMobile_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txtMsg_KeyDown(object sender, KeyEventArgs e)
        {
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                WebClient clnt = new WebClient();
                //   string api = "http://api." + TxtAPI.Text + ".com/api/sendsms?id=" + txtUser.Text + "&pass=" + txtPass1.Text + "&mobile=" + txtMobile.Text + "&brandname=" + txtBrand.Text + "&msg=" + txtMain.Text + "&language=English&network=1;";
                string api = "http://msgpk.com/api/send.php?api_key=" + txtWhatsapp.Text + "&mobile=" + txtMobile.Text + "&priority=0&message=" + txtMsg.Text + "";

                clnt.OpenRead(api);
                MessageBox.Show("Message Sent Successfuly");

            }
            catch (Exception)
            {
                MessageBox.Show("Please Check Your INTERNET OR CALL 0333 3003033");

            }
        }

        

        
        }
    }

