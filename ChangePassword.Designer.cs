namespace XagSurveillanceGCS
{
    partial class ChangePassword
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCurrentPassword = new System.Windows.Forms.Label();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.lblConfirmPassword = new System.Windows.Forms.Label();

            this.txtCurrentPassword = new System.Windows.Forms.TextBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();

            this.chkShowPassword = new System.Windows.Forms.CheckBox();

            this.btnChangePassword = new XagSurveillanceGCS.Controls.MyButton();
            this.btnCancel = new XagSurveillanceGCS.Controls.MyButton();

            this.SuspendLayout();

            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(35, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(205, 30);
            this.lblTitle.Text = "Change Password";

            //
            // lblCurrentPassword
            //
            this.lblCurrentPassword.AutoSize = true;
            this.lblCurrentPassword.Location = new System.Drawing.Point(40, 80);
            this.lblCurrentPassword.Name = "lblCurrentPassword";
            this.lblCurrentPassword.Size = new System.Drawing.Size(102, 15);
            this.lblCurrentPassword.Text = "Current Password";

            //
            // txtCurrentPassword
            //
            this.txtCurrentPassword.Location = new System.Drawing.Point(40, 100);
            this.txtCurrentPassword.Name = "txtCurrentPassword";
            this.txtCurrentPassword.Size = new System.Drawing.Size(320, 23);
            this.txtCurrentPassword.TabIndex = 0;

            //
            // lblNewPassword
            //
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Location = new System.Drawing.Point(40, 140);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(90, 15);
            this.lblNewPassword.Text = "New Password";

            //
            // txtNewPassword
            //
            this.txtNewPassword.Location = new System.Drawing.Point(40, 160);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(320, 23);
            this.txtNewPassword.TabIndex = 1;

            //
            // lblConfirmPassword
            //
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Location = new System.Drawing.Point(40, 200);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(113, 15);
            this.lblConfirmPassword.Text = "Confirm Password";

            //
            // txtConfirmPassword
            //
            this.txtConfirmPassword.Location = new System.Drawing.Point(40, 220);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Size = new System.Drawing.Size(320, 23);
            this.txtConfirmPassword.TabIndex = 2;

            //
            // chkShowPassword
            //
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Location = new System.Drawing.Point(40, 255);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(113, 19);
            this.chkShowPassword.TabIndex = 3;
            this.chkShowPassword.Text = "Show Password";
            this.chkShowPassword.UseVisualStyleBackColor = true;

            //
            // btnChangePassword
            //
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Text = "Change Password";
            this.btnChangePassword.Location = new System.Drawing.Point(40, 300);
            this.btnChangePassword.Size = new System.Drawing.Size(150, 38);
            this.btnChangePassword.TabIndex = 4;

            this.btnChangePassword.BGGradTop =
                System.Drawing.Color.FromArgb(148, 193, 31);
            this.btnChangePassword.BGGradBot =
                System.Drawing.Color.FromArgb(205, 226, 150);

            //
            // btnCancel
            //
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(210, 300);
            this.btnCancel.Size = new System.Drawing.Size(150, 38);
            this.btnCancel.TabIndex = 5;

            this.btnCancel.BGGradTop =
                System.Drawing.Color.FromArgb(255, 54, 54);
            this.btnCancel.BGGradBot =
                System.Drawing.Color.FromArgb(255, 68, 68);

            //
            // ChangePassword
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 370);

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblCurrentPassword);
            this.Controls.Add(this.txtCurrentPassword);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.btnCancel);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Password";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCurrentPassword;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.Label lblConfirmPassword;

        private System.Windows.Forms.TextBox txtCurrentPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;

        private System.Windows.Forms.CheckBox chkShowPassword;

        private Controls.MyButton btnChangePassword;
        private Controls.MyButton btnCancel;
    }
}