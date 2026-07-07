using System;
using System.Threading;
using System.Windows.Forms;
using XagSurveillanceGCS.Comms;
using XagSurveillanceGCS.Utilities;

namespace XagSurveillanceGCS.Controls
{
    public partial class ConnectionOptions : Form
    {
        // Thread signal. 
        public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        public ConnectionOptions()
        {
            InitializeComponent();

            CMB_serialport.Items.AddRange(SerialPort.GetPortNames());
            CMB_serialport.Items.Add("TCP");
            CMB_serialport.Items.Add("UDP");
            CMB_serialport.Items.Add("UDPCl");
            CMB_serialport.Items.Add("WS");

            ThemeManager.ApplyThemeTo(this);

            XagSurveillanceGCS.Utilities.Tracking.AddPage(this.GetType().ToString(), this.Text);
        }

        public void BUT_connect_Click(object sender, EventArgs e)
        {
            var mav = new MAVLinkInterface();

            try
            {
                // Connect, but don't try to get params yet, the serial reader thread doesn't try to
                // read from this port until we add it to Comports, and it cannot be added to Comports
                // until the BaseStream is open.
                MainV2.instance.doConnect(mav, "UDP", "115200", getparams:false);

                MainV2.Comports.Add(mav);

                // It is now safe to fetch params
                mav.getParamList();

                MainV2._connectionControl.UpdateSysIDS();
            }
            catch (Exception)
            {
            }
        }
    }
}