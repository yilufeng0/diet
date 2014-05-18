using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace dietcal
{
    public partial class Login : Form
    {
#region 
        private SqlConnection sqlconn;
        private Size oldSize = new Size(380, 251);
        private Size newSize = new Size(380, 394);
        private string conString = "";
        private string ipadress="127.0.0.1", database="DietCal", dbuser="admin", dbpwd="admin";
#endregion
        public Login()
        {
            InitializeComponent();
            sqlconn = new SqlConnection();
            this.textBox_userInput.Text = "";
            this.textBox_passwdInput.Text = "";
            //this.button_logIn.Enabled = false;
            this.label_Auction.Visible = false;            
            textBox_passwdInput.PasswordChar = '*';
            this.Size = oldSize;
            this.textBox_userPasswd.PasswordChar = '*';
            this.textBox_userInput.Text = "admin";
            this.textBox_passwdInput.Text = "admin";
            try
            {
                string server = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\DIETCAL", "SERVER", "127.0.0.1").ToString();
                string dbname = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\DIETCAL", "DATABASE", "dietcal").ToString();
                string dbuser = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\DIETCAL", "USERNAME", "admin").ToString();
                string dbpasswd = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\DIETCAL", "USERPASSWD", "admin").ToString();
                textBox_hostAddr.Text = server;
                textBox_dbName.Text = dbname;
                textBox_userName.Text = dbuser;
                textBox_userPasswd.Text = dbpasswd;
            }
            catch (System.Exception ex)
            {
                Microsoft.Win32.RegistryKey rklocalMachie = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey rksoftware = rklocalMachie.OpenSubKey("SOFTWARE", true);
                Microsoft.Win32.RegistryKey rkDietCal = rksoftware.CreateSubKey("DIETCAL");
                rkDietCal.SetValue("SERVER", "127.0.0.1", Microsoft.Win32.RegistryValueKind.String);
                rkDietCal.SetValue("DATABASE", "dietcal", Microsoft.Win32.RegistryValueKind.String);
                rkDietCal.SetValue("USERNAME", "admin", Microsoft.Win32.RegistryValueKind.String);
                rkDietCal.SetValue("USERPASSWD", "admin", Microsoft.Win32.RegistryValueKind.String);
                this.Size = newSize;
                button_hideDisp.Visible = true;
                button_dbset.Visible = false;
            }

        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            this.textBox_userInput.Text = "";
            this.textBox_passwdInput.Text = "";
            
        }

        private void button_logIn_Click(object sender, EventArgs e)
        {
            if (this.textBox_passwdInput.Text.Trim()==String.Empty || this.textBox_userInput.Text.Trim()==String.Empty)
            {
                textBox_userInput.Focus();
                return;
            }
            //登录信息
            string loginUser = this.textBox_userInput.Text.Trim();
            string loginPasswd = this.textBox_passwdInput.Text.Trim();
            if (sqlconn.State != ConnectionState.Open)
            {
                if (ipadress == string.Empty || database == string.Empty || dbuser == string.Empty || dbpwd == string.Empty)
                {
                    MessageBox.Show("请输入完整数据库信息...");
                    return;
                }
                conString = "Data Source = " + ipadress + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID = " + dbuser + ";Password = " + dbpwd + ";";
                sqlconn.ConnectionString = conString;

                try
                {
                    sqlconn.Open();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("数据库连接失败!");
                    this.Size = newSize;
                    textBox_hostAddr.Focus();
                    return;
                }
            }
            string QueryResult="";
            string SelectComm = "select userpasswd from [user] where username ='" + loginUser + "'";
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.Connection = sqlconn;
            sqlcomm.CommandText = SelectComm;
            sqlcomm.CommandType = CommandType.Text;
            SqlDataReader sdatareader = sqlcomm.ExecuteReader();
            sdatareader.Read();
            QueryResult = sdatareader["userpasswd"].ToString().Trim();
            sdatareader.Close();
            if (QueryResult == loginPasswd)
            {
                this.Visible = false;
                this.label_Auction.Visible = false;
                this.button_testConn.Enabled = false;
                this.button_dbset.Enabled=false;
                //sqlconn.Close();
                Form1 dealform = new Form1(loginUser,loginPasswd,conString);
                dealform.ShowDialog();
                this.Visible = true;
                //this.textBox_passwdInput.Text = "";

            }
            else
            {
                this.label_Auction.Visible = true;
            }
            
        }

        private void button_dbset_Click(object sender, EventArgs e)
        {
            this.Size = newSize;
            this.button_dbset.Visible = false;
            this.button_hideDisp.Visible = true;
        }

        private void button_hideDisp_Click(object sender, EventArgs e)
        {
            this.Size = oldSize;
            this.button_dbset.Visible = true;
            this.button_hideDisp.Visible = false;
        }

        private void button_testConn_Click(object sender, EventArgs e)
        {
            if (sqlconn.State == ConnectionState.Open)
            {
                return;
            }
            if (ipadress == string.Empty || database == string.Empty || dbuser == string.Empty || dbpwd == string.Empty)
            {
                MessageBox.Show("请输入完整信息...");
                return;
            }
            
            try
            {
                conString = "Data Source = " + ipadress + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID = " + dbuser + ";Password = " + dbpwd + ";";
                sqlconn.ConnectionString = conString;
                sqlconn.Open();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("数据库连接错误!");
            }
            if (sqlconn.State == ConnectionState.Open)
            {
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\DIETCAL", "SERVER", textBox_hostAddr.Text, Microsoft.Win32.RegistryValueKind.String);
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\DIETCAL", "DATABASE", textBox_dbName.Text, Microsoft.Win32.RegistryValueKind.String);
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\DIETCAL", "USERNAME", textBox_userName.Text, Microsoft.Win32.RegistryValueKind.String);
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\DIETCAL", "USERPASSWD", textBox_userPasswd.Text, Microsoft.Win32.RegistryValueKind.String);
                this.Size = oldSize;
                this.button_dbset.Visible = true;
                this.button_dbset.Enabled = false;
                MessageBox.Show("数据库连接成功!");
            } 
            
        }

        private void textBox_hostAddr_TextChanged(object sender, EventArgs e)
        {
            
            ipadress = textBox_hostAddr.Text.Trim();
                         
        }

        private void textBox_dbName_TextChanged(object sender, EventArgs e)
        {
            database = textBox_dbName.Text.Trim();
        }

        private void textBox_userName_TextChanged(object sender, EventArgs e)
        {
            dbuser = textBox_userName.Text.Trim();
        }

        private void textBox_userPasswd_TextChanged(object sender, EventArgs e)
        {
            dbpwd = textBox_userPasswd.Text.Trim();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlconn.Close();
            }
        }

    }
}
