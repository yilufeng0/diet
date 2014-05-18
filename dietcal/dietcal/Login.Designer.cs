namespace dietcal
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_title = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_userInput = new System.Windows.Forms.TextBox();
            this.textBox_passwdInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_logIn = new System.Windows.Forms.Button();
            this.button_reset = new System.Windows.Forms.Button();
            this.button_dbset = new System.Windows.Forms.Button();
            this.button_hideDisp = new System.Windows.Forms.Button();
            this.button_testConn = new System.Windows.Forms.Button();
            this.textBox_hostAddr = new System.Windows.Forms.TextBox();
            this.textBox_dbName = new System.Windows.Forms.TextBox();
            this.textBox_userPasswd = new System.Windows.Forms.TextBox();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_Auction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_title.Location = new System.Drawing.Point(134, 27);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(106, 24);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "用户登录";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户名：";
            // 
            // textBox_userInput
            // 
            this.textBox_userInput.Location = new System.Drawing.Point(100, 72);
            this.textBox_userInput.MaxLength = 24;
            this.textBox_userInput.Name = "textBox_userInput";
            this.textBox_userInput.Size = new System.Drawing.Size(198, 21);
            this.textBox_userInput.TabIndex = 2;
            this.textBox_userInput.Text = "admin";
            // 
            // textBox_passwdInput
            // 
            this.textBox_passwdInput.Location = new System.Drawing.Point(100, 105);
            this.textBox_passwdInput.MaxLength = 24;
            this.textBox_passwdInput.Name = "textBox_passwdInput";
            this.textBox_passwdInput.Size = new System.Drawing.Size(198, 21);
            this.textBox_passwdInput.TabIndex = 4;
            this.textBox_passwdInput.Text = "admin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密  码：";
            // 
            // button_logIn
            // 
            this.button_logIn.Location = new System.Drawing.Point(114, 163);
            this.button_logIn.Name = "button_logIn";
            this.button_logIn.Size = new System.Drawing.Size(54, 24);
            this.button_logIn.TabIndex = 5;
            this.button_logIn.Text = "登录";
            this.button_logIn.UseVisualStyleBackColor = true;
            this.button_logIn.Click += new System.EventHandler(this.button_logIn_Click);
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(210, 163);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(54, 24);
            this.button_reset.TabIndex = 6;
            this.button_reset.Text = "重置";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // button_dbset
            // 
            this.button_dbset.Location = new System.Drawing.Point(-1, 207);
            this.button_dbset.Name = "button_dbset";
            this.button_dbset.Size = new System.Drawing.Size(101, 21);
            this.button_dbset.TabIndex = 7;
            this.button_dbset.Text = "数据库设置";
            this.button_dbset.UseVisualStyleBackColor = true;
            this.button_dbset.Click += new System.EventHandler(this.button_dbset_Click);
            // 
            // button_hideDisp
            // 
            this.button_hideDisp.Location = new System.Drawing.Point(-2, 344);
            this.button_hideDisp.Name = "button_hideDisp";
            this.button_hideDisp.Size = new System.Drawing.Size(101, 21);
            this.button_hideDisp.TabIndex = 8;
            this.button_hideDisp.Text = "隐藏显示";
            this.button_hideDisp.UseVisualStyleBackColor = true;
            this.button_hideDisp.Click += new System.EventHandler(this.button_hideDisp_Click);
            // 
            // button_testConn
            // 
            this.button_testConn.Location = new System.Drawing.Point(131, 333);
            this.button_testConn.Name = "button_testConn";
            this.button_testConn.Size = new System.Drawing.Size(75, 23);
            this.button_testConn.TabIndex = 9;
            this.button_testConn.Text = "测试连接";
            this.button_testConn.UseVisualStyleBackColor = true;
            this.button_testConn.Click += new System.EventHandler(this.button_testConn_Click);
            // 
            // textBox_hostAddr
            // 
            this.textBox_hostAddr.Location = new System.Drawing.Point(69, 249);
            this.textBox_hostAddr.MaxLength = 30;
            this.textBox_hostAddr.Name = "textBox_hostAddr";
            this.textBox_hostAddr.Size = new System.Drawing.Size(100, 21);
            this.textBox_hostAddr.TabIndex = 10;
            this.textBox_hostAddr.Text = "127.0.0.1";
            this.textBox_hostAddr.TextChanged += new System.EventHandler(this.textBox_hostAddr_TextChanged);
            // 
            // textBox_dbName
            // 
            this.textBox_dbName.Location = new System.Drawing.Point(230, 249);
            this.textBox_dbName.MaxLength = 24;
            this.textBox_dbName.Name = "textBox_dbName";
            this.textBox_dbName.Size = new System.Drawing.Size(100, 21);
            this.textBox_dbName.TabIndex = 11;
            this.textBox_dbName.Text = "dietcal";
            this.textBox_dbName.TextChanged += new System.EventHandler(this.textBox_dbName_TextChanged);
            // 
            // textBox_userPasswd
            // 
            this.textBox_userPasswd.Location = new System.Drawing.Point(231, 288);
            this.textBox_userPasswd.MaxLength = 24;
            this.textBox_userPasswd.Name = "textBox_userPasswd";
            this.textBox_userPasswd.Size = new System.Drawing.Size(100, 21);
            this.textBox_userPasswd.TabIndex = 13;
            this.textBox_userPasswd.Text = "admin";
            this.textBox_userPasswd.TextChanged += new System.EventHandler(this.textBox_userPasswd_TextChanged);
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(67, 288);
            this.textBox_userName.MaxLength = 24;
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(100, 21);
            this.textBox_userName.TabIndex = 12;
            this.textBox_userName.Text = "admin";
            this.textBox_userName.TextChanged += new System.EventHandler(this.textBox_userName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "主机地址：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 292);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "用户名：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(175, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "数据库：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(175, 292);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "密  码：";
            // 
            // label_Auction
            // 
            this.label_Auction.AutoSize = true;
            this.label_Auction.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Auction.ForeColor = System.Drawing.Color.Red;
            this.label_Auction.Location = new System.Drawing.Point(105, 131);
            this.label_Auction.Name = "label_Auction";
            this.label_Auction.Size = new System.Drawing.Size(65, 10);
            this.label_Auction.TabIndex = 18;
            this.label_Auction.Text = "无效登录操作";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 356);
            this.Controls.Add(this.label_Auction);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_userPasswd);
            this.Controls.Add(this.textBox_userName);
            this.Controls.Add(this.textBox_dbName);
            this.Controls.Add(this.textBox_hostAddr);
            this.Controls.Add(this.button_testConn);
            this.Controls.Add(this.button_hideDisp);
            this.Controls.Add(this.button_dbset);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.button_logIn);
            this.Controls.Add(this.textBox_passwdInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_userInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Login_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_userInput;
        private System.Windows.Forms.TextBox textBox_passwdInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_logIn;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Button button_dbset;
        private System.Windows.Forms.Button button_hideDisp;
        private System.Windows.Forms.Button button_testConn;
        private System.Windows.Forms.TextBox textBox_hostAddr;
        private System.Windows.Forms.TextBox textBox_dbName;
        private System.Windows.Forms.TextBox textBox_userPasswd;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_Auction;
    }
}