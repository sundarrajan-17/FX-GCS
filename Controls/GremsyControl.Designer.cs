namespace XagSurveillanceGCS.Controls
{
    partial class GremsyControl
    {
        private System.Windows.Forms.TableLayoutPanel mainFlow;
        private System.Windows.Forms.TableLayoutPanel cameraSettingsLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel parentTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnGremsyTest;
        private System.Windows.Forms.Button btnGremsyStartRecording;
        private System.Windows.Forms.Button btnGremsyStopRecording;
        private System.Windows.Forms.Button btnGremsyTakePhoto;
        private System.Windows.Forms.Button btnGremsyZoomIn;
        private System.Windows.Forms.Button btnGremsyZoomOut;
        private System.Windows.Forms.Button btnGremsyZoomStop;
        private System.Windows.Forms.Button btnGremsyTrackStop;
        // private System.Windows.Forms.Button btnGremsyDoUp;
        // private System.Windows.Forms.Button btnGremsyDoDown;
        // private System.Windows.Forms.Button btnGremsyDoLeft;
        // private System.Windows.Forms.Button btnGremsyDoRight;
        // private System.Windows.Forms.Button btnGremsyDoStop;
        private new System.Windows.Forms.TableLayoutPanel tblGremsy;
        private System.Windows.Forms.CheckBox chkRecordMode;
        private System.Windows.Forms.Label recordingPhotoMode;
        // private System.Windows.Forms.GroupBox groupSmartTracker;
        private XagSurveillanceGCS.Controls.VirtualJoystick virtualJoystick;    
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.Button ZoomInButton;
        private System.Windows.Forms.Button ZoomOutButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnIrZoomPlus;
        private System.Windows.Forms.Button btnIrZoomMinus;

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
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));

            // Zoomtablelayoutpanel
            // tableLayoutPanel1
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.00F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.00F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.00F));

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
            this.btnGremsyTest = new System.Windows.Forms.Button();
            this.btnGremsyStartRecording = new System.Windows.Forms.Button();
            this.btnGremsyStopRecording = new System.Windows.Forms.Button();
            this.btnGremsyTakePhoto = new System.Windows.Forms.Button();
            this.btnGremsyZoomIn = new System.Windows.Forms.Button();
            this.btnGremsyZoomOut = new System.Windows.Forms.Button();
            this.btnGremsyZoomStop = new System.Windows.Forms.Button();
            this.btnGremsyTrackStop = new System.Windows.Forms.Button();
            this.btnIrZoomPlus = new System.Windows.Forms.Button();
            this.btnIrZoomMinus = new System.Windows.Forms.Button();
            this.recordingPhotoMode = new System.Windows.Forms.Label();
            this.chkRecordMode = new System.Windows.Forms.CheckBox();

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

            //
            //Zoom in Button
            //
            this.ZoomInButton = new System.Windows.Forms.Button();
            this.ZoomInButton.Text = "+";
            this.ZoomInButton.Height = 35;
            this.ZoomInButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZoomInButton.Font = new System.Drawing.Font(
            "Segoe UI",
            16F,
            System.Drawing.FontStyle.Bold,
            System.Drawing.GraphicsUnit.Point,
            ((byte)(0)));
            // this.ZoomInButton.Click += new System.EventHandler(this.BtnZoomIn_Click);
            this.ZoomInButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnZoomIn_MouseDown);
            this.ZoomInButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnZoomIn_MouseUp);

            //
            //Zoom out Button
            //
            this.ZoomOutButton = new System.Windows.Forms.Button();
            this.ZoomOutButton.Text = "-";
            this.ZoomOutButton.Height = 35;
            this.ZoomOutButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZoomOutButton.Font = new System.Drawing.Font(
            "Segoe UI",
            16F,
            System.Drawing.FontStyle.Bold,
            System.Drawing.GraphicsUnit.Point,
            ((byte)(0)));
            // this.ZoomOutButton.Click += new System.EventHandler(this.BtnZoomOut_Click);
            this.ZoomOutButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnZoomOut_MouseDown);
            this.ZoomOutButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnZoomOut_MouseUp);


            this.btnGremsyTest.Text = "Photo Mode";
            this.btnGremsyTest.Height = 35;
            this.btnGremsyTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyTest.AutoSize = false;
            this.btnGremsyTest.Click += new System.EventHandler(this.ChkRecordMode_CheckedChanged);

            this.btnGremsyStartRecording.Text = "Start Recording";
            this.btnGremsyStartRecording.Height = 35;
            this.btnGremsyStartRecording.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyStartRecording.Click += new System.EventHandler(this.BtnStartRecording_Click);

            this.btnGremsyStopRecording.Text = "Stop Recording";
            this.btnGremsyStopRecording.Height = 35;
            this.btnGremsyStopRecording.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyStopRecording.Click += new System.EventHandler(this.BtnStopRecording_Click);

            this.btnGremsyTakePhoto.Text = "Take Photo";
            this.btnGremsyTakePhoto.Height = 35;
            this.btnGremsyTakePhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyTakePhoto.Click += new System.EventHandler(this.BtnTakePhoto_Click);

            this.btnGremsyZoomIn.Text = "Camera Settings";
            this.btnGremsyZoomIn.Height = 35;
            this.btnGremsyZoomIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyZoomIn.Click += new System.EventHandler(this.BtnCameraSettings_Click);

            this.btnGremsyZoomOut.Text = "Point Home";
            this.btnGremsyZoomOut.Height = 35;
            this.btnGremsyZoomOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyZoomOut.Click += new System.EventHandler(this.BtnZoomOut_Click);

            this.btnIrZoomPlus.Text = "IR Zoom+";
            this.btnIrZoomPlus.Height = 35;
            this.btnIrZoomPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIrZoomPlus.Click += new System.EventHandler(this.BtnIrZoomPlus_Click);

            this.btnIrZoomMinus.Text = "IR Zoom-";
            this.btnIrZoomMinus.Height = 35;
            this.btnIrZoomMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIrZoomMinus.Click += new System.EventHandler(this.BtnIrZoomMinus_Click);

            this.btnGremsyZoomStop.Text = "Point Down";
            this.btnGremsyZoomStop.Height = 35;
            this.btnGremsyZoomStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyZoomStop.Click += new System.EventHandler(this.BtnZoomStop_Click);

            this.btnGremsyTrackStop.Text = "Calculate Distance";
            this.btnGremsyTrackStop.Height = 35;
            this.btnGremsyTrackStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGremsyTrackStop.Click += new System.EventHandler(this.BtnStopTracking_Click);

            //
            // this.tableLayoutPanel2.Controls.Add(this.btnGremsyTest, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyStartRecording, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyStopRecording, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyTakePhoto, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyZoomIn, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyZoomOut, 1,1);    
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyZoomStop, 2,1);
            this.tableLayoutPanel2.Controls.Add(this.btnGremsyTrackStop, 4,0);
            this.tableLayoutPanel2.Controls.Add(this.btnIrZoomPlus,3,0);
            this.tableLayoutPanel2.Controls.Add(this.btnIrZoomMinus,3,1);

            this.parentTableLayoutPanel.Controls.Add(this.tableLayoutPanel2,0,0);
            this.Controls.Add(this.parentTableLayoutPanel);
            this.Size = new System.Drawing.Size(500, 200);
        }

    }
}