// LoginForm.cs
using System;
using System.Management;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Linq;
using System.Net.NetworkInformation;
using XagSurveillanceGCS.Utilities;

namespace XagSurveillanceGCS
{
    public partial class LoginForm : Form
    {
        public bool LoginSuccess { get; private set; } = false;

        public string SelectedLMMode {get; private set;} = "Combat Mode";

        public string SystemMACAddress = "";

        public string[] validUUids = new[] {"804DB1F8-BDAF-4426-A410-461AE527BB68","BF065C93-9FAC-3143-90FA-C5DC2182D911"};
        public string[] validIdentifyingNumbers = new[] {"RS603S3052","RANRCX006007408"};

        public LoginForm()
        {
            InitializeComponent();

            TXT_version.Text = "Version: 1.5.10";
        }

        public static (string UUID, string IdentifyingNumber) GetSystemInfo()
        {
            string uuid = "";
            string identifyingNumber = "";

            using (ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT UUID, IdentifyingNumber FROM Win32_ComputerSystemProduct"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    uuid = obj["UUID"]?.ToString() ?? "";
                    identifyingNumber = obj["IdentifyingNumber"]?.ToString() ?? "";
                    break; // only one system product entry is expected
                }
            }

            return (uuid, identifyingNumber);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            var (uuid, identifyingNumber) = GetSystemInfo();

            Console.WriteLine($"System UUID: {uuid}");
            Console.WriteLine($"System Identifying Number: {identifyingNumber}");

            string decodedPassword =  System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Settings.Instance["login_password"]));
            
            
            // if (validUUids.Contains(uuid) && validIdentifyingNumbers.Contains(identifyingNumber))
            // {
                if(username == "XAGGCS" && password == decodedPassword)
                {
                    LoginSuccess = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid credentials!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            // }
            // else
            // {
            //     MessageBox.Show("This system is not authorized to run the application.", "Unauthorized System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // }   
        }
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            using (ChangePassword frm = new ChangePassword())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Password changed successfully.");
                }
            }
        }
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Username")
            {
                txtUsername.Text = "";
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Username";
                // txtUsername.ForeColor = Color.Gray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
                // txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.Text = "Password";
                // txtPassword.ForeColor = Color.Gray;
            }
        }
        // private void CMB_setLMMode_Click(object sender, EventArgs e)
        // {
        //     try
        //     {
        //         Console.WriteLine("The Selected Mode issssss {0}", CMB_setLMMode.SelectedIndex);
        //         // ((Control)sender).Enabled = false;
        //     }
        //     catch
        //     {
        //         CustomMessageBox.Show(Strings.CommandFailed, Strings.ERROR);
        //     }

        //     // ((Control)sender).Enabled = true;
        // }

    }
}
