namespace XagSurveillanceGCS
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Label TXT_version;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            // this.SuspendLayout();

            this.label1 = new System.Windows.Forms.Label();
            this.TXT_version = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(470, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Tag = "custom";
            this.label1.Text = "by Software Team";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TXT_version
            // 
            this.TXT_version.BackColor = System.Drawing.Color.Transparent;
            this.TXT_version.ForeColor = System.Drawing.Color.White;
            this.TXT_version.Location = new System.Drawing.Point(425, 160);
            this.TXT_version.Name = "TXT_version";
            this.TXT_version.Size = new System.Drawing.Size(155, 25);
            this.TXT_version.TabIndex = 1;
            this.TXT_version.Text = "Version: ";
            this.TXT_version.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // txtUsername
            this.txtUsername.BackColor = System.Drawing.Color.Black;
            this.txtUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(193)))), ((int)(((byte)(78)))));;
            this.txtUsername.Location = new System.Drawing.Point(200, 80);
            this.txtUsername.Text = "XAGGCS";
            // this.txtUsername.ForeColor = Color.Gray;
            this.txtUsername.Enter += new System.EventHandler(this.txtUsername_Enter);
            this.txtUsername.Leave += new System.EventHandler(this.txtUsername_Leave);
            this.txtUsername.Size = new System.Drawing.Size(200, 23);
            this.txtUsername.TabIndex = 0;

            // txtPassword
            this.txtPassword.BackColor = System.Drawing.Color.Black;
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(193)))), ((int)(((byte)(78)))));;
            this.txtPassword.Location = new System.Drawing.Point(200, 125);
            this.txtPassword.Text = "xag@12345";
            // this.txtPassword.ForeColor = Color.Gray;
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            this.txtPassword.Size = new System.Drawing.Size(200, 23);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;

            // btnLogin
            this.btnLogin.BackColor = System.Drawing.Color.Black;
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(193)))), ((int)(((byte)(78)))));;
            // this.btnLogin. = System.Drawing.Color.Transparent;
            this.btnLogin.Location = new System.Drawing.Point(250, 160);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 30);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            this.BackgroundImage = global::XagSurveillanceGCS.Properties.Resources.splashdark;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(584, 336);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TXT_version);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 375);
            this.MinimumSize = new System.Drawing.Size(600, 375);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
