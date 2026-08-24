using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XagSurveillanceGCS.Utilities;

namespace XagSurveillanceGCS
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();

            txtCurrentPassword.UseSystemPasswordChar = true;
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;

            btnChangePassword.Click += btnChangePassword_Click;
            btnCancel.Click += btnCancel_Click;
        }

        #region Events

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool hide = !chkShowPassword.Checked;

            txtCurrentPassword.UseSystemPasswordChar = hide;
            txtNewPassword.UseSystemPasswordChar = hide;
            txtConfirmPassword.UseSystemPasswordChar = hide;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            // ----------------------------------------------------------------
            // TODO:
            // Replace this with your own password verification logic.
            //
            // Example:
            // if (!VerifyCurrentPassword(txtCurrentPassword.Text))
            // {
            //     MessageBox.Show("Current password is incorrect.");
            //     return;
            // }
            //
            // SavePassword(txtNewPassword.Text);
            // ----------------------------------------------------------------

            // MessageBox.Show(
            //     "Password changed successfully.",
            //     "Success",
            //     MessageBoxButtons.OK,
            //     MessageBoxIcon.Information);

            Settings.Instance["login_password"] = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(txtNewPassword.Text));
            Console.WriteLine("New Password Saveddddddd {0}",Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(txtNewPassword.Text)));
            Settings.Instance.Save();
            ClearFields();

            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion

        #region Validation

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
            {
                MessageBox.Show("Please enter the current password.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCurrentPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Please enter a new password.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNewPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("Please confirm the new password.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtConfirmPassword.Focus();
                return false;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("New Password and Confirm Password do not match.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtConfirmPassword.Focus();
                txtConfirmPassword.SelectAll();

                return false;
            }

            if (!IsStrongPassword(txtNewPassword.Text))
            {
                MessageBox.Show(
                    "Password must contain:\n\n" +
                    "• Minimum 8 characters\n" +
                    "• One uppercase letter\n" +
                    "• One lowercase letter\n" +
                    "• One number\n" +
                    "• One special character",
                    "Weak Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNewPassword.Focus();
                txtNewPassword.SelectAll();

                return false;
            }

            if (txtCurrentPassword.Text == txtNewPassword.Text)
            {
                MessageBox.Show(
                    "New password cannot be the same as the current password.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNewPassword.Focus();
                txtNewPassword.SelectAll();

                return false;
            }

            return true;
        }

        private bool IsStrongPassword(string password)
        {
            return Regex.IsMatch(
                password,
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$");
        }

        #endregion

        #region Helpers

        private void ClearFields()
        {
            txtCurrentPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();

            chkShowPassword.Checked = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Override this method or replace its contents
        /// with your own password verification.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        protected virtual bool VerifyCurrentPassword(string password)
        {
            // TODO:
            // Compare against your encrypted/hashed password.

            return true;
        }

        /// <summary>
        /// Override this method or replace its contents
        /// with your own password saving logic.
        /// </summary>
        /// <param name="newPassword"></param>
        protected virtual void SavePassword(string newPassword)
        {
            // TODO:
            // Save encrypted password to XML / DB / Config file.
        }

        #endregion
    }
}