using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MissionPlanner.GCSViews;

namespace MissionPlanner.Controls
{
    public partial class BaseCameraController : UserControl
    {
        private Dictionary<string, Func<UserControl>> _cameraFactories;
        private UserControl _activeCameraControl;

        public FlightData _flightData;

        public BaseCameraController(FlightData flightData)
        {
            InitializeComponent();
            InitializeCameraList();
            this._flightData = flightData;
            btnApplyCamera.Click += BtnApplyCamera_Click;
        }

        private void InitializeCameraList()
        {
            _cameraFactories = new Dictionary<string, Func<UserControl>>
            {
                { "XAGCAM1", () => new ViewproControl(this) },
                { "XAGCAM2", () => new GremsyControl(this) },
                // { "SIYI", () => new SiyiControl() },
                // { "Custom", () => new CustomControl() },
                // { "Simulation", () => new SimControl() }
            };

            cmbCameraSelect.Items.AddRange(new object[]
            {
                "XAGCAM1",
                "XAGCAM2",
                // "SIYI",
                // "Custom",
                // "Simulation"
            });

            cmbCameraSelect.SelectedIndex = 0;
        }

        public String SelectedCamera
        {
            get { return cmbCameraSelect.SelectedItem?.ToString(); }
        }

        private void BtnApplyCamera_Click(object sender, EventArgs e)
        {
            string selectedCamera = cmbCameraSelect.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCamera))
                return;

            LoadCameraControl(selectedCamera);
        }

        private void LoadCameraControl(string cameraName)
        {
            // Remove existing camera UI
            if (_activeCameraControl != null)
            {
                pnlCameraHost.Controls.Remove(_activeCameraControl);
                _activeCameraControl.Dispose();
                _activeCameraControl = null;
            }

            // Create new camera UI
            if (_cameraFactories.TryGetValue(cameraName, out var factory))
            {
                _activeCameraControl = factory.Invoke();
                _activeCameraControl.Dock = DockStyle.Fill;
                pnlCameraHost.Controls.Add(_activeCameraControl);
            }
        }
    }
}
