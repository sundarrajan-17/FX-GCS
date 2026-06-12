// LoginForm.cs
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace XagSurveillanceGCS
{
    public partial class LoginForm : Form
    {
        public bool LoginSuccess { get; private set; } = false;

        public string SelectedLMMode {get; private set;} = "Combat Mode";

        public string SystemMACAddress = "";

        public string[] ValidMACAddress = new[] {"E89C2591620F","",""};

        public LoginForm()
        {
            InitializeComponent();

            TXT_version.Text = "Version: 1.5.10";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine($"{nic.Name} - {nic.GetPhysicalAddress()} - {nic.OperationalStatus}");
                if(nic.Name == "Ethernet" )
                {
                    Console.WriteLine("Selected MAC Address: " + nic.GetPhysicalAddress());
                    this.SystemMACAddress = nic.GetPhysicalAddress().ToString();
                }
            }

            // Console.WriteLine("MAC Address: " + mac);
            // if(Array.Exists(ValidMACAddress, mac => mac == this.SystemMACAddress))
            // {
                if (username == "XAGGCS" && password == "xag@12345")
                {
                    LoginSuccess = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid credentials!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            // } else
            // {
            //     MessageBox.Show("Unauthorized Device! No License For This PC.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // }
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
