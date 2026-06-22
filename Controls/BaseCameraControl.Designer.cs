namespace XagSurveillanceGCS.Controls
{
    partial class BaseCameraController
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox cmbCameraSelect;
        private System.Windows.Forms.Button btnApplyCamera;
        private System.Windows.Forms.Panel pnlCameraHost;
        private System.Windows.Forms.Label lblCamera;
        public System.Windows.Forms.Label TargetDistance;
        public System.Windows.Forms.Label DooafX;
        public System.Windows.Forms.Label DooafY;
        public System.Windows.Forms.Label GnssModeStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbCameraSelect = new System.Windows.Forms.ComboBox();
            this.btnApplyCamera = new System.Windows.Forms.Button();
            this.pnlCameraHost = new System.Windows.Forms.Panel();
            this.lblCamera = new System.Windows.Forms.Label();
            this.TargetDistance = new System.Windows.Forms.Label();
            this.DooafX = new System.Windows.Forms.Label();
            this.DooafY = new System.Windows.Forms.Label();
            this.GnssModeStatus = new System.Windows.Forms.Label();

            // Label
            this.lblCamera.Text = "Camera Type:";
            this.lblCamera.Location = new System.Drawing.Point(10, 12);
            this.lblCamera.AutoSize = true;

            // ComboBox
            this.cmbCameraSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameraSelect.Location = new System.Drawing.Point(100, 8);
            this.cmbCameraSelect.Size = new System.Drawing.Size(180, 21);

            // Apply Button
            this.btnApplyCamera.Text = "Apply";
            this.btnApplyCamera.Location = new System.Drawing.Point(290, 7);
            this.btnApplyCamera.Size = new System.Drawing.Size(75, 23);

            // TargetDistance
            this.TargetDistance.Name = "Distance";
            this.TargetDistance.Text = "Distance: 0.00 m";
            this.TargetDistance.ForeColor = System.Drawing.Color.White;
            this.TargetDistance.Location = new System.Drawing.Point(10,12);
            this.TargetDistance.Size = new System.Drawing.Size(110, 20);

            // DooafX
            this.DooafX.Name = "DOOAFX";
            this.DooafX.Text = "DOOAFX: 0.00 m";
            this.DooafX.ForeColor = System.Drawing.Color.White;
            this.DooafX.Location = new System.Drawing.Point(120,12);
            // this.DooafX.Size = new System.Drawing.Size(250, 20);

            // DooafY
            this.DooafY.Name = "DOOAFY";
            this.DooafY.Text = "DOOAFY: 0.00 m";
            this.DooafY.ForeColor = System.Drawing.Color.White;
            this.DooafY.Location = new System.Drawing.Point(240,12);
            // this.DooafY.Size = new System.Drawing.Size(250, 20);

            // GnssModeStatus
            this.GnssModeStatus.Name = "GnssModeStatus";
            this.GnssModeStatus.Text = "GNSS Mode: Unknown";
            this.GnssModeStatus.ForeColor = System.Drawing.Color.White;
            this.GnssModeStatus.Location = new System.Drawing.Point(360, 12);
            this.GnssModeStatus.Size = new System.Drawing.Size(200, 20);

            // Camera Host Panel
            this.pnlCameraHost.Location = new System.Drawing.Point(10, 40);
            this.pnlCameraHost.Size = new System.Drawing.Size(600, 280);
            this.pnlCameraHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // BaseCameraController
            this.Controls.Add(this.DooafX);
            this.Controls.Add(this.DooafY);
            this.Controls.Add(this.TargetDistance);
            this.Controls.Add(this.GnssModeStatus);
            this.Controls.Add(this.pnlCameraHost);

            this.Size = new System.Drawing.Size(600, 320);
        }
    }
}
