namespace XagSurveillanceGCS.Controls
{
    partial class GremsyVioControl
    {
        private System.Windows.Forms.TableLayoutPanel mainFlow;
        private System.Windows.Forms.TableLayoutPanel cameraSettingsLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel parentTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnGremsyVioTest;
        private System.Windows.Forms.Button btnGremsyVioStartRecording;
        private System.Windows.Forms.Button btnGremsyVioStopRecording;
        private System.Windows.Forms.Button btnGremsyVioTakePhoto;
        private System.Windows.Forms.Button btnGremsyVioZoomIn;
        private System.Windows.Forms.Button btnGremsyVioZoomOut;
        private System.Windows.Forms.Button btnGremsyVioZoomStop;
        private System.Windows.Forms.Button btnGremsyVioTrackStop;
        // private System.Windows.Forms.Button btnGremsyVioDoUp;
        // private System.Windows.Forms.Button btnGremsyVioDoDown;
        // private System.Windows.Forms.Button btnGremsyVioDoLeft;
        // private System.Windows.Forms.Button btnGremsyVioDoRight;
        // private System.Windows.Forms.Button btnGremsyVioDoStop;
        private new System.Windows.Forms.TableLayoutPanel tblGremsyVio;
        private System.Windows.Forms.CheckBox chkRecordMode;
        private System.Windows.Forms.Label recordingPhotoMode;
        // private System.Windows.Forms.GroupBox groupSmartTracker;
        private XagSurveillanceGCS.Controls.VirtualJoystick virtualJoystick;    
        private System.Windows.Forms.GroupBox camControlGroup;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.ComboBox cmbCameraSelect;

         /// <summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.parentTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.parentTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parentTableLayoutPanel.ColumnCount = 2; 
            this.parentTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.00F));
            this.parentTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.00F));
            this.parentTableLayoutPanel.RowCount = 2;
            this.parentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.00F));
            this.parentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.00F));

            // tableLayoutPanel1
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.00F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.00F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.00F));

            // tableLayoutPanel2
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));

            // trackzoom

            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.trackZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackZoom.Minimum = -100;
            this.trackZoom.Maximum = 100;
            this.trackZoom.Value = 0;
            this.trackZoom.TickFrequency = 20;
            this.trackZoom.LargeChange = 10;
            this.trackZoom.SmallChange = 1;
            this.trackZoom.Dock = System.Windows.Forms.DockStyle.None;
            this.trackZoom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.trackZoom.Height = 80;
            this.trackZoom.Width = 30;
            this.trackZoom.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.trackZoom.ValueChanged += TrackZoom_ValueChanged;
            this.trackZoom.MouseUp += TrackZoom_MouseUp;

            //
            

            // Create buttons
            this.mainFlow = new System.Windows.Forms.TableLayoutPanel();
            this.mainFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainFlow.Name = "mainFlow";
            this.mainFlow.ColumnCount = 2;
            this.mainFlow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00F));
            this.mainFlow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00F));
            // this.mainFlow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            // this.mainFlow.Controls.Add(this.cameraSettingsLayoutPanel, 0, 0);
            // this.mainFlow.Controls.Add(this.tableLayoutPanel2, 0, 1);
            // this.mainFlow.Controls.Add(this.groupBox1, 1, 0);
            // this.mainFlow.Controls.Add(this.groupBox2, 2, 0);
            this.mainFlow.Location = new System.Drawing.Point(0, 0);
            // this.mainFlow.Name = "mainFlow";
            this.mainFlow.RowCount = 2;
            this.mainFlow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.00F));
            this.mainFlow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.00F));
            this.mainFlow.Size = new System.Drawing.Size(562, 250);
            this.mainFlow.TabIndex = 0;
            // this.mainFlow.Size = new System.Drawing.Size(1200, 800);

            this.cameraSettingsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.cameraSettingsLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraSettingsLayoutPanel.ColumnCount = 4;
            this.cameraSettingsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.cameraSettingsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.cameraSettingsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.cameraSettingsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.cameraSettingsLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.cameraSettingsLayoutPanel.Name = "cameraSettingsLayoutPanel";
            this.cameraSettingsLayoutPanel.RowCount = 3;
            this.cameraSettingsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.cameraSettingsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.cameraSettingsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.cameraSettingsLayoutPanel.Size = new System.Drawing.Size(400, 250);
            this.cameraSettingsLayoutPanel.TabIndex = 0;


            this.btnGremsyVioTest = new System.Windows.Forms.Button();
            this.btnGremsyVioStartRecording = new System.Windows.Forms.Button();
            this.btnGremsyVioStopRecording = new System.Windows.Forms.Button();
            this.btnGremsyVioTakePhoto = new System.Windows.Forms.Button();
            this.btnGremsyVioZoomIn = new System.Windows.Forms.Button();
            this.btnGremsyVioZoomOut = new System.Windows.Forms.Button();
            this.btnGremsyVioZoomStop = new System.Windows.Forms.Button();
            this.btnGremsyVioTrackStop = new System.Windows.Forms.Button();
            this.recordingPhotoMode = new System.Windows.Forms.Label();
            this.chkRecordMode = new System.Windows.Forms.CheckBox();
            this.camControlGroup = new System.Windows.Forms.GroupBox();
            this.cmbCameraSelect = new System.Windows.Forms.ComboBox();
            this.camControlGroup.SuspendLayout();
            // this.virtualJoystick = new DumsLogisticsGCS.Controls.VirtualJoystick();

            // Camera Control Group
            this.camControlGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.camControlGroup.Name = "camControlGroup";
            this.camControlGroup.TabIndex = 3;
            this.camControlGroup.TabStop = false;
            this.camControlGroup.ForeColor = System.Drawing.Color.White;

            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.TabIndex = 0;

            this.trackZoom = new System.Windows.Forms.TrackBar();

            this.trackZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackZoom.Minimum = -100;
            this.trackZoom.Maximum = 100;
            this.trackZoom.Value = 0;
            this.trackZoom.TickFrequency = 20;
            this.trackZoom.LargeChange = 10;
            this.trackZoom.SmallChange = 1;

            // this.trackZoom.Height = 70;
            // this.trackZoom.Width = 30;

            this.trackZoom.Dock = System.Windows.Forms.DockStyle.None;
            this.trackZoom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.trackZoom.Height = 80;
            this.trackZoom.Width = 40;
            this.trackZoom.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);

            // EVENTS
            this.trackZoom.ValueChanged += TrackZoom_ValueChanged;
            this.trackZoom.MouseUp += TrackZoom_MouseUp;


            this.btnGremsyVioTest.Text = "Stop Capture";
            this.btnGremsyVioTest.Height = 35;
            this.btnGremsyVioTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyVioTest.AutoSize = false;
            this.btnGremsyVioTest.Click += new System.EventHandler(this.ChkRecordMode_CheckedChanged);

            this.btnGremsyVioStartRecording.Text = "Start Recording";
            this.btnGremsyVioStartRecording.Height = 35;
            this.btnGremsyVioStartRecording.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyVioStartRecording.Click += new System.EventHandler(this.BtnStartRecording_Click);

            this.btnGremsyVioStopRecording.Text = "Stop Recording";
            this.btnGremsyVioStopRecording.Height = 35;
            this.btnGremsyVioStopRecording.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyVioStopRecording.Click += new System.EventHandler(this.BtnStopRecording_Click);

            this.btnGremsyVioTakePhoto.Text = "Start Capture";
            this.btnGremsyVioTakePhoto.Height = 35;
            this.btnGremsyVioTakePhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyVioTakePhoto.Click += new System.EventHandler(this.BtnTakePhoto_Click);

            this.btnGremsyVioZoomIn.Text = "Camera Settings";
            this.btnGremsyVioZoomIn.Height = 35;
            this.btnGremsyVioZoomIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyVioZoomIn.Click += new System.EventHandler(this.BtnCameraSettings_Click);

            this.btnGremsyVioZoomOut.Text = "IR Zoom +";
            this.btnGremsyVioZoomOut.Height = 35;
            this.btnGremsyVioZoomOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyVioZoomOut.Click += new System.EventHandler(this.BtnZoomOut_Click);

            this.btnGremsyVioZoomStop.Text = "Point Home";
            this.btnGremsyVioZoomStop.Height = 35;
            this.btnGremsyVioZoomStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyVioZoomStop.Click += new System.EventHandler(this.BtnZoomStop_Click);

            this.btnGremsyVioTrackStop.Text = "Point Down";
            this.btnGremsyVioTrackStop.Height = 35;
            this.btnGremsyVioTrackStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyVioTrackStop.Click += new System.EventHandler(this.BtnStopTracking_Click);

            this.cmbCameraSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameraSelect.Height = 35;
            this.cmbCameraSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCameraSelect.Items.AddRange(new string[] {"1X","2X","3X","4X","5X","6X","7X","8X"});
            this.cmbCameraSelect.SelectedIndexChanged += new System.EventHandler(this.handleIrzoom_Click);

            this.cameraSettingsLayoutPanel.Controls.Add(this.btnGremsyVioTest, 0, 0);
            this.cameraSettingsLayoutPanel.Controls.Add(this.btnGremsyVioStartRecording, 1, 0);
            this.cameraSettingsLayoutPanel.Controls.Add(this.btnGremsyVioStopRecording, 2, 0);
            this.cameraSettingsLayoutPanel.Controls.Add(this.btnGremsyVioTakePhoto, 3, 0);
            this.cameraSettingsLayoutPanel.Controls.Add(this.btnGremsyVioZoomIn, 0, 1);
            this.cameraSettingsLayoutPanel.Controls.Add(this.btnGremsyVioZoomOut, 1,1);    
            this.cameraSettingsLayoutPanel.Controls.Add(this.btnGremsyVioZoomStop, 2,1);
            this.cameraSettingsLayoutPanel.Controls.Add(this.btnGremsyVioTrackStop, 3,1);

            //
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyVioTest, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyVioStartRecording, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyVioStopRecording, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyVioTakePhoto, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyVioZoomIn, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbCameraSelect, 1,1);    
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyVioZoomStop, 2,1);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyVioTrackStop, 3,1);

            // this.groupSmartTracker = new System.Windows.Forms.GroupBox();
            // this.groupSmartTracker.Text = "Gimbal Control";
            // this.groupSmartTracker.ForeColor = System.Drawing.Color.White;
            // this.groupSmartTracker.Size = new System.Drawing.Size(180, 150);

            // this.groupSmartTracker.Controls.AddRange(new System.Windows.Forms.Control[] {
            //     this.btnGremsyVioDoUp, this.btnGremsyVioDoDown, this.btnGremsyVioDoLeft, this.btnGremsyVioDoRight, this.btnGremsyVioDoStop
            // });
            // ---- Add TableLayoutPanel to Control ----
            // this.mainFlow.Controls.Add(this.groupSmartTracker);
            // this.mainFlow.Controls.Add(this.virtualJoystick);
            // this.mainFlow.Controls.Add(this.cameraSettingsLayoutPanel, 0, 0);
            // this.tableLayoutPanel3.Controls.Add(this.trackZoom, 1, 0);
            this.parentTableLayoutPanel.Controls.Add(this.tableLayoutPanel2,0,0);
            this.Controls.Add(this.parentTableLayoutPanel);
            // // ---- GremsyVioControl ----
            // this.Name = "GremsyVioControl";
            this.Size = new System.Drawing.Size(650, 800);
        }

        // Helper method for adding rows
        private void AddRow(System.Windows.Forms.Control c1, System.Windows.Forms.Control c2,System.Windows.Forms.Control c3,System.Windows.Forms.Control c4)
        {
            int row = this.tblGremsyVio.RowCount++;
            this.tblGremsyVio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblGremsyVio.Controls.Add(c1, 0, row);
            this.tblGremsyVio.Controls.Add(c2, 1, row);
            this.tblGremsyVio.Controls.Add(c3, 2, row);
            this.tblGremsyVio.Controls.Add(c4, 3, row);
        }
    }
}