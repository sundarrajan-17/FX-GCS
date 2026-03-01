namespace MissionPlanner.Controls
{
    partial class BaseCameraController
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox cmbCameraSelect;
        private System.Windows.Forms.Button btnApplyCamera;
        private System.Windows.Forms.Panel pnlCameraHost;
        private System.Windows.Forms.Label lblCamera;

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

            // Camera Host Panel
            this.pnlCameraHost.Location = new System.Drawing.Point(10, 40);
            this.pnlCameraHost.Size = new System.Drawing.Size(500, 300);
            this.pnlCameraHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // BaseCameraController
            this.Controls.Add(this.lblCamera);
            this.Controls.Add(this.cmbCameraSelect);
            this.Controls.Add(this.btnApplyCamera);
            this.Controls.Add(this.pnlCameraHost);

            this.Size = new System.Drawing.Size(600, 800);
        }
    }
}
