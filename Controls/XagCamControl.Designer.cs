namespace XagSurveillanceGCS.Controls
{
    partial class XagCamControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel parentTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private XagSurveillanceGCS.Controls.VirtualJoystick virtualJoystick;
        private System.Windows.Forms.GroupBox camControlGroup;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.Button btnStartRecording;
        private System.Windows.Forms.Button btnStopRecording;
        private System.Windows.Forms.Button btnTakePhoto;
        private System.Windows.Forms.Button btnOsdOn;
        private System.Windows.Forms.Button btnOsdOff;
        private System.Windows.Forms.Button btnAiOsdOn;
        private System.Windows.Forms.Button btnAiOsdOff;
        private System.Windows.Forms.Button btnDZoomPlus;
        private System.Windows.Forms.Button  btnDZoomMinus;
        private System.Windows.Forms.Button  setEoIrMode;
        private System.Windows.Forms.ComboBox CMB_setEoIrMode;
        private System.Windows.Forms.Button btnCalculateDooaf;
        private System.Windows.Forms.Button btnCalculateTargetDistance;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.parentTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.parentTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parentTableLayoutPanel.RowCount = 2;
            this.parentTableLayoutPanel.ColumnCount = 2;
            this.parentTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.95876F));
            this.parentTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.04124F));
            this.parentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.00F));
            this.parentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.00F));
            // this.parentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00F));

            // tableLayoutPanel1
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.00F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.00F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.00F));

            // tableLayoutPanel2
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));

            // tablelayoutpanell3
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.00F));

            // zoom control
            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.trackZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackZoom.Minimum = -100;
            this.trackZoom.Maximum = 100;
            this.trackZoom.Value = 0;
            this.trackZoom.TickFrequency = 20;
            this.trackZoom.LargeChange = 10;
            this.trackZoom.SmallChange = 1;
            this.trackZoom.Height = 80;
            this.trackZoom.Width = 40;
            this.trackZoom.Dock = System.Windows.Forms.DockStyle.None;
            this.trackZoom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.trackZoom.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.trackZoom.ValueChanged += TrackZoom_ValueChanged;
            this.trackZoom.MouseUp += TrackZoom_MouseUp;

            // start recording button

            this.btnStartRecording = new System.Windows.Forms.Button();
            this.btnStartRecording.Text = "Start Record";
            this.btnStartRecording.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartRecording.Click += new System.EventHandler(this.BtnStartRecording_Click);
            // stop recording button

            this.btnStopRecording = new System.Windows.Forms.Button();
            this.btnStopRecording.Text = "Stop Record";
            this.btnStopRecording.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStopRecording.Click += new System.EventHandler(this.BtnStopRecording_Click);
            // photo button

            this.btnTakePhoto = new System.Windows.Forms.Button();
            this.btnTakePhoto.Text = "Photo";
            this.btnTakePhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTakePhoto.Click += new System.EventHandler(this.BtnTakePhoto_Click);
            // osd on

            this.btnOsdOn = new System.Windows.Forms.Button();
            this.btnOsdOn.Text = "Connect Gimbal";
            this.btnOsdOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOsdOn.Click += new System.EventHandler(this.BtnOsdOn_Click);
            // osd off

            this.btnOsdOff = new System.Windows.Forms.Button();
            this.btnOsdOff.Text = "Point Home";
            this.btnOsdOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOsdOff.Click += new System.EventHandler(this.BtnOsdOff_Click);

            // Ai osd on

            this.btnAiOsdOn = new System.Windows.Forms.Button();
            this.btnAiOsdOn.Text = "Point Down";
            this.btnAiOsdOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAiOsdOn.Click += new System.EventHandler(this.BtnAiOsdOn_Click);
            // Ai osd off

            this.btnAiOsdOff = new System.Windows.Forms.Button();
            this.btnAiOsdOff.Text = "Stop Track";
            this.btnAiOsdOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAiOsdOff.Click += new System.EventHandler(this.BtnAiOsdOff_Click);
            // Digital Zoom +

            this.btnDZoomPlus = new System.Windows.Forms.Button();
            this.btnDZoomPlus.Text = "DZoom +";
            this.btnDZoomPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDZoomPlus.Click += new System.EventHandler(this.BtnDZoomPlus_Click);
            // Digital Zoom -

            this.btnDZoomMinus = new System.Windows.Forms.Button();
            this.btnDZoomMinus.Text = "DZoom -";
            this.btnDZoomMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDZoomMinus.Click += new System.EventHandler(this.BtnDZoomMinus_Click);

            // eo ir selection
            this.CMB_setEoIrMode = new System.Windows.Forms.ComboBox();
            this.CMB_setEoIrMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_setEoIrMode.DropDownWidth = 150;
            this.CMB_setEoIrMode.FormattingEnabled = true;
            this.CMB_setEoIrMode.Items.AddRange(new object[] {"EO","EO+IR WhiteHot","EO+IR BlackHot","EO+IR PseudoHot","IR+EO WhiteHot","IR+EO BlackHot","IR+EO PseudoHot","IR WhiteHot","IR BlackHot","IR PseudoHot"});
            this.CMB_setEoIrMode.Name = "CMB_setEoIrMode";
            this.CMB_setEoIrMode.Click += new System.EventHandler(this.CMB_setEoIrMode_Click);

            // eo ir set
            this.setEoIrMode = new System.Windows.Forms.Button();
            this.setEoIrMode.Text = "Set Camera Mode";
            this.setEoIrMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setEoIrMode.Click += new System.EventHandler(this.setEoIrMode_Click);

            // calculate dooaf
            this.btnCalculateDooaf = new System.Windows.Forms.Button();
            this.btnCalculateDooaf.Text = "Calculate Dooaf";
            this.btnCalculateDooaf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalculateDooaf.Click += new System.EventHandler(this.BtnCalculateDooaf_Click);

            // calculate target distance
            this.btnCalculateTargetDistance = new System.Windows.Forms.Button();
            this.btnCalculateTargetDistance.Text = "Calculate Target Distance";
            this.btnCalculateTargetDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalculateTargetDistance.Click += new System.EventHandler(this.BtnCalculateTargetDistance_Click);

            // parent controls
            // this.parentTableLayoutPanel.Controls.Add(this.tableLayoutPanel1,0,0);

            // tableLayoutPanel2 Controls
            this.tableLayoutPanel2.Controls.Add(this.btnStartRecording,0,0);
            this.tableLayoutPanel2.Controls.Add(this.btnStopRecording,1,0);
            this.tableLayoutPanel2.Controls.Add(this.btnTakePhoto,2,0);
            this.tableLayoutPanel2.Controls.Add(this.btnOsdOn,0,1);
            this.tableLayoutPanel2.Controls.Add(this.btnOsdOff,1,1);
            this.tableLayoutPanel2.Controls.Add(this.btnAiOsdOn,2,1);
            // this.tableLayoutPanel2.Controls.Add(this.btnAiOsdOff,3,1);
            this.tableLayoutPanel2.Controls.Add(this.btnDZoomPlus,3,0);
            this.tableLayoutPanel2.Controls.Add(this.btnDZoomMinus,3,1);
            this.tableLayoutPanel2.Controls.Add(this.CMB_setEoIrMode,4,0);
            this.tableLayoutPanel2.Controls.Add(this.setEoIrMode,5,0);

            // tablelayoutPanel3 Controls
            this.tableLayoutPanel2.Controls.Add(this.btnCalculateDooaf,5,1);
            this.tableLayoutPanel2.Controls.Add(this.btnCalculateTargetDistance,4,1);

            // Main Controls
            this.parentTableLayoutPanel.Controls.Add(tableLayoutPanel2,1,0);
            this.Controls.Add(this.parentTableLayoutPanel);
            this.Size = new System.Drawing.Size(550,240);
        }
    }
}