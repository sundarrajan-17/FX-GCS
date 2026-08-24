using System;
using System.Windows.Forms;
using XagSurveillanceGCS.Controls;
using XagSurveillanceGCS.GCSViews.ConfigurationView;

namespace XagSurveillanceGCS.GCSViews
{
    public partial class PreFlightConfig : MyUserControl
    {
        public MotorTestPanel motorTestPanel;
        private ControlSurface configFailSafe;
        public PreFlightConfig()
        {
            InitializeComponent();
            motorTestPanel = new MotorTestPanel();
            motorTestPanel.Activate();
            configFailSafe = new ControlSurface();
            this.tableLayoutPanel1.Controls.Add(motorTestPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(configFailSafe, 1, 0);
            // configFailSafe.Deactivate();
            // configFailSafe.Activate();
        }

        private void btnPreFlight_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Pre-Flight Check Started",
                "Pre-Flight",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}