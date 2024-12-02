using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Xml;





namespace datagrid
{
    public partial class Form1 : Form
    {








        string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Weighbridge\\DataFile.mdb;Jet OLEDB:Database Password=mniaz1;";


        OleDbConnection cn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Weighbridge\\DataFile.mdb;Jet OLEDB:Database Password=mniaz1;");
        OleDbCommand cmd;
        OleDbDataAdapter da;
        OleDbDataReader dr;

        public Form1()
        {
            InitializeComponent();
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            retrive();
            dataGridView1.Visible = true;
            button2.Visible = true;
            txtMobile.Visible = true;
            button5.Visible = false;
            txtMobile1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = true;

        }
        private void retrive()
        {

            string sql = "SELECT * FROM weight";
            OleDbConnection con = new OleDbConnection(conString);
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql,con);
            DataSet ds = new DataSet();
            con.Open();
            adapter.Fill(ds, "weight");
           

            con.Close();

            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "weight";

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewCell cell = null;

            foreach(DataGridViewCell selectedCell in dataGridView1.SelectedCells)
            {
                cell = selectedCell;
                break;
              

            }
            if (cell != null)
            {

                DataGridViewRow row = cell.OwningRow;
                txt1.Text = row.Cells[0].Value.ToString();
                txt2.Text = row.Cells[1].Value.ToString();
                txt3.Text = row.Cells[3].Value.ToString();
                txt4.Text = row.Cells[6].Value.ToString();
                txt5.Text = row.Cells[9].Value.ToString();
                txt6.Text = row.Cells[12].Value.ToString();
                txtWeight.Text = row.Cells[14].Value.ToString();
                txt8.Text = row.Cells[15].Value.ToString();
                txt9.Text = row.Cells[12].Value.ToString();
                txtMobile.Text = row.Cells[18].Value.ToString();
          
                txtSerial.Text = row.Cells[0].Value.ToString();

            }
  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                String messageText, to, mask, userName, password, sessionId, messageIds;

                userName = txtUser.Text;
                password = txtPass1.Text;
                messageText = txtMain.Text;
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
                        MessageBox.Show("Messages Sent Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

   

     
  

     
     
     
  

      

     

     

         


        private void Form1_Load(object sender, EventArgs e)
        {
            retrive();

            cn.Open();
            OleDbCommand cmd1 = new OleDbCommand("select Login,Pass,address,WAAPI from Logins where LoginID=@LID ", cn);
            OleDbCommand cmd2 = new OleDbCommand("select mask from tblmask where ID=@SR ", cn);
            OleDbCommand cmd3 = new OleDbCommand("select title,extra from labels where left=@SR1 ", cn);
            cmd1.Parameters.AddWithValue("@LID", txtUP.Text);
            cmd2.Parameters.AddWithValue("@SR", txtam.Text);
            cmd3.Parameters.AddWithValue("@SR1", txtam1.Text);
            OleDbDataReader dr2;
            OleDbDataReader dr1;
            OleDbDataReader dr3;
            dr1 = cmd1.ExecuteReader();
            dr2 = cmd2.ExecuteReader();
            dr3 = cmd3.ExecuteReader();

            if (dr2.Read())
            {
                txtBrand.Text = dr2["mask"].ToString();

            }
            if (dr3.Read())
            {
                txtCy.Text = dr3["title"].ToString();
                txtcPH.Text = dr3["extra"].ToString();
              

            }
                if (dr1.Read())
                {
                    txtUser.Text = dr1["Login"].ToString();
                    txtPass1.Text = dr1["Pass"].ToString();
                    txtAddress.Text = dr1["address"].ToString();
                    txtWhatsapp.Text = dr1["WAAPI"].ToString();
                }
            else
            {
                txtBrand.Text = "MNIAZ";





            }

            cn.Close();


            LabelCompany.Text = "" + txtCy.Text + " " + txtAddress.Text + " " + txtcPH.Text + "";
            txtCompany.Text = "" + txtCy.Text + " " + txtAddress.Text + "  " + txtcPH.Text + "";


        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtKgD.Text = (float.Parse(txtKgs.Text) / float.Parse(txt100.Text) * float.Parse(txt40.Text)).ToString();
          
                var str = txtWeight.Text;
                char[] seperator = { '.' };
                string[] strarr = null;
                strarr = str.Split(seperator);

                string mnds = strarr[0];
                string kgs = strarr[1];
                txtMnds.Text = mnds;
                txtKgs.Text = kgs;
       
        }

     
       
        private void txtMnds_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void txtMain_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSrch.Text = txt1.Text;






           

        }
        

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void txt40_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt100_TextChanged(object sender, EventArgs e)
        {

        }

       
        private void txt2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt4_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt8_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMnds_MouseClick(object sender, MouseEventArgs e)
        {
         
        }

        private void label3_DoubleClick(object sender, EventArgs e)
        {
            txtBrand.Visible = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            txtBrand.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            txtBrand.Visible = false;
            TxtAPI.Visible = false;

        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            txtBrand.Visible = true;
            TxtAPI.Visible = true;


        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtSrch.Text = txt1.Text;
            

        }

        private void sendNewMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPass fp = new FrmPass();
            fp.Show();
            this.Hide();

        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            FrmPass fp = new FrmPass();
            fp.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            cn.Open();
            OleDbCommand cmd1 = new OleDbCommand("select sr_no,vehicle_no,party_name,first_weight,second_weight,net_weight,mds2 from weight where sr_no=@SR ", cn);
            cmd1.Parameters.AddWithValue("@SR", txtSrch.Text);
            OleDbDataReader dr1;
            dr1 = cmd1.ExecuteReader();

            if (dr1.Read())
            {
                txt1.Text = dr1["sr_no"].ToString();
                txt2.Text = dr1["vehicle_no"].ToString();
                txt3.Text = dr1["party_name"].ToString();
                txt4.Text = dr1["first_weight"].ToString();
                txt5.Text = dr1["second_weight"].ToString();
                txt6.Text = dr1["net_weight"].ToString();
                txtWeight.Text = dr1["mds2"].ToString();
                txt8.Text = "";
              






            }
            else
            {
                txt1.Text = "";
                txt2.Text = "";
                txt3.Text = "";
                txt4.Text = "";
                txt5.Text = "";
                txt6.Text = "";
                txtWeight.Text = "";
                txt8.Text = "";
               


            }

            cn.Close();




        }

        private void txtSrch_TextChanged(object sender, EventArgs e)
        {
            if (txtSrch.Text == "")
            {
                txtMain.Text = "";
                txtMobile.Text = "";
            }


            if (txtSrch.Text != "")
            {
                cn.Open();
                OleDbCommand cmd1 = new OleDbCommand("select sr_no,vehicle_no,party_name,first_weight,second_weight,net_weight,mds2,comments from weight where sr_no=@SR ", cn);
                cmd1.Parameters.AddWithValue("@SR", txtSrch.Text);
                OleDbDataReader dr1;
                dr1 = cmd1.ExecuteReader();

                if (dr1.Read())
                {
                    txt1.Text = dr1["sr_no"].ToString();
                    txt2.Text = dr1["vehicle_no"].ToString();
                    txt3.Text = dr1["party_name"].ToString();
                    txt4.Text = dr1["first_weight"].ToString();
                    txt5.Text = dr1["second_weight"].ToString();
                    txt6.Text = dr1["net_weight"].ToString();
                    txtWeight.Text = dr1["mds2"].ToString();
                    txt8.Text = "";
                    txtMobile.Text = dr1["comments"].ToString();
                    txtMobile1.Text = dr1["comments"].ToString();

                    txtMain.Text = "Dear " + txt3.Text + "!\r\n\r\nWeight Info:\r\n\r\nSr. No " + txt1.Text + "\r\nVehicle No: " + txt2.Text + "\r\nGross Wt: " + txt4.Text + "\r\nTare Wt: " + txt5.Text + "\r\n\r\nNet Wt: " + txt6.Text + "\r\n" + txtWeight.Text + " Mnds\r\n\r\n" + txtCompany.Text + "\r\n\r\n.";
                   


                }
                else
                {
                    txt1.Text = "";
                    txt2.Text = "";
                    txt3.Text = "";
                    txt4.Text = "";
                    txt5.Text = "";
                    txt6.Text = "";
                    txtWeight.Text = "";
                    txt8.Text = "";
                    txtMobile.Text = "";
                    txtMobile1.Text = "";
                    txtMain.Text = "";



                }

                cn.Close();
            }

            
        }

        private void txtSrch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    String messageText, to, mask, userName, password, sessionId, messageIds;

                    userName = txtUser.Text;
                    password = txtPass1.Text;
                    messageText = txtMain.Text;
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
        }

        private void txtSrch_Enter(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            txtMobile1.Visible = false;
          
            txtMobile.Visible = true;
            label1.Visible = false;
            button5.Visible = false;
            pictureBox2.Visible = true;
            pictureBox3.Visible = false;
            txtSrch.Text = "";


        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            button5.Visible = true;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                WebClient clnt = new WebClient();
                string api = "http://api." + TxtAPI.Text + ".com/api/sendsms?id=niaz&pass=niaz1&mobile=" + txtMobile.Text + "&brandname=" + txtBrand.Text + "&msg=" + txtMain.Text + "&language=English&network=1;";
                clnt.OpenRead(api);
                MessageBox.Show("Message Sent Successfuly");

            }
            catch (Exception)
            {
                MessageBox.Show("Please Check Your INTERNET");

            }
        }

        private void txtMobile_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMobile_Enter(object sender, EventArgs e)
        {
            button2.Visible = true;

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            txtSrch.Text = txt1.Text;
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            txtSrch.Text = txt1.Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void sMSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
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
                txtBrand.Text = "SMS All";
               




            }

            cn.Close();

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
                txtBrand.Text = "SMS All";





            }

            cn.Close();

        }

        private void txtBrand_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCompany_TextChanged(object sender, EventArgs e)
        {

        }

        private void companyInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCPass fcp = new FrmCPass();
            fcp.Show();
            this.Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void txtKgD_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtKgs_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
    
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient clnt = new WebClient();
                
              //  string api = "https://telenorcsms.com.pk:27677/corporate_sms2/api/sendsms.jsp?session_id=8fe47262805840318726186802fd86ef&to=923333003033&text=Test&mask=NIAZ";
               string api = "https://telenorcsms.com.pk:27677/corporate_sms2/api/sendsms.jsp?session_id=" + txtUser.Text + "&to=" + txtMobile.Text + "&text=" + txtMain.Text + "&mask=" + txtBrand.Text + "";

              //  string api = "http://api." + TxtAPI.Text + ".com/api/sendsms?id=" + txtUser.Text + "&pass=" + txtPass1.Text + "&mobile=" + txtMobile.Text + "&brandname=" + txtBrand.Text + "&msg=" + txtMain.Text + "&language=English&network=1;";
             //   string api = "http://api." + TxtAPI.Text + ".com/api/sendsms?id=" + txtUser.Text + "&pass=" + txtPass1.Text + "&mobile=" + txtMobile.Text + "&brandname=" + txtBrand.Text + "&msg=" + txtMain.Text + "&language=English&network=1;";
                clnt.OpenRead(api);
                MessageBox.Show("Message Sent Successfuly");

            }
            catch (Exception)
            {
                MessageBox.Show("Please Check Your INTERNET");

            }
        }

        private void button4_Click_3(object sender, EventArgs e)
        {
            try
            {
                WebClient clnt = new WebClient();
                //   string api = "http://api." + TxtAPI.Text + ".com/api/sendsms?id=" + txtUser.Text + "&pass=" + txtPass1.Text + "&mobile=" + txtMobile.Text + "&brandname=" + txtBrand.Text + "&msg=" + txtMain.Text + "&language=English&network=1;";
                string api = "http://msgpk.com/api/send.php?api_key=" + txtWhatsapp.Text + "&mobile=" + txtMobile.Text + "&priority=0&message=" + txtMain.Text + "";

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

