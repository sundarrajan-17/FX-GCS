using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace XagSurveillanceGCS.Controls
{
    public class ControlSurface : MyUserControl
    {
        // ============================================================
        // SERVO OUTPUT CONFIGURATION
        // ============================================================

        // Change these according to your flight controller output
        private const int ROLL_RIGHT_SERVO = 1;
        private const int ROLL_LEFT_SERVO  = 2;
        private const int PITCH_UP_SERVO   = 3;
        private const int PITCH_DOWN_SERVO = 4;
        private const int YAW_RIGHT_SERVO  = 5;
        private const int YAW_LEFT_SERVO   = 6;

        // Change these PWM values according to your requirement
        private const int ROLL_RIGHT_PWM = 1900;
        private const int ROLL_LEFT_PWM  = 1100;

        private const int PITCH_UP_PWM   = 1900;
        private const int PITCH_DOWN_PWM = 1100;

        private const int YAW_RIGHT_PWM  = 1900;
        private const int YAW_LEFT_PWM   = 1100;


        // ============================================================
        // CONTROLS
        // ============================================================

        private TableLayoutPanel tableLayoutPanel;
        
        private Button btnRollRight;
        private Button btnRollLeft;
        private Button btnPitchUp;
        private Button btnPitchDown;
        private Button btnYawRight;
        private Button btnYawLeft;
        private Button btnSequence;
        private GroupBox groupBox1;


        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public ControlSurface()
        {
            InitializeComponent();
        }


        // ============================================================
        // INITIALIZE UI
        // ============================================================

        private void InitializeComponent()
        {
            this.tableLayoutPanel = new TableLayoutPanel();

            this.btnRollRight = new Button();
            this.btnRollLeft = new Button();

            this.btnPitchUp = new Button();
            this.btnPitchDown = new Button();

            this.btnYawRight = new Button();
            this.btnYawLeft = new Button();

            this.btnSequence = new Button();

            this.groupBox1 = new GroupBox();

            this.SuspendLayout();

            // ========================================================
            // TABLE LAYOUT
            // ========================================================

            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.RowCount = 3;

            this.tableLayoutPanel.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.33F));

            this.tableLayoutPanel.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.33F));

            this.tableLayoutPanel.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.34F));

            this.tableLayoutPanel.RowStyles.Add(
                new RowStyle(SizeType.Percent, 33.33F));

            this.tableLayoutPanel.RowStyles.Add(
                new RowStyle(SizeType.Percent, 33.33F));

            this.tableLayoutPanel.RowStyles.Add(
                new RowStyle(SizeType.Percent, 33.34F));

            // this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Size = new Size(380, 300);
            this.tableLayoutPanel.Margin = new Padding(0);
            this.tableLayoutPanel.Padding = new Padding(20);

            // ========================================================
            // ROLL RIGHT
            // ========================================================

            ConfigureButton(
                btnRollRight,
                "Roll Right",
                Color.LightGreen);

            btnRollRight.Click += BtnRollRight_Click;

            // ========================================================
            // ROLL LEFT
            // ========================================================

            ConfigureButton(
                btnRollLeft,
                "Roll Left",
                Color.LightGreen);

            btnRollLeft.Click += BtnRollLeft_Click;

            // ========================================================
            // PITCH UP
            // ========================================================

            ConfigureButton(
                btnPitchUp,
                "Pitch Up",
                Color.LightBlue);

            btnPitchUp.Click += BtnPitchUp_Click;

            // ========================================================
            // PITCH DOWN
            // ========================================================

            ConfigureButton(
                btnPitchDown,
                "Pitch Down",
                Color.LightBlue);

            btnPitchDown.Click += BtnPitchDown_Click;

            // ========================================================
            // YAW RIGHT
            // ========================================================

            ConfigureButton(
                btnYawRight,
                "Yaw Right",
                Color.LightYellow);

            btnYawRight.Click += BtnYawRight_Click;

            // ========================================================
            // YAW LEFT
            // ========================================================

            ConfigureButton(
                btnYawLeft,
                "Yaw Left",
                Color.LightYellow);

            btnYawLeft.Click += BtnYawLeft_Click;

            ConfigureButton(
                btnSequence,
                "Sequence",
                Color.LightCoral);
            
            btnSequence.Click += btnSequence_Click;


            // ========================================================
            // ADD CONTROLS
            // ========================================================

            // Row 0
            this.tableLayoutPanel.Controls.Add(
                btnRollLeft, 0, 0);

            this.tableLayoutPanel.Controls.Add(
                btnPitchUp, 0, 1);

            this.tableLayoutPanel.Controls.Add(
                btnRollRight, 1, 0);


            // Row 1
            this.tableLayoutPanel.Controls.Add(
                btnYawLeft, 0, 2);


            this.tableLayoutPanel.Controls.Add(
                btnYawRight, 1, 2);


            // Row 2

            this.tableLayoutPanel.Controls.Add(
                btnPitchDown, 1, 1);

            // Row 2

            this.tableLayoutPanel.Controls.Add(
                btnSequence, 0, 3);

            this.tableLayoutPanel.Location = new System.Drawing.Point(20, 12);

            // ========================================================
            // CONTROL SURFACE
            // ========================================================

            this.groupBox1.Text = "Control Surface";
            // this.groupBox1.Dock = DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "Control Surface";
            this.groupBox1.Size = new System.Drawing.Size(420, 316);
            this.groupBox1.Controls.Add(this.tableLayoutPanel);

            this.Controls.Add(this.groupBox1);

            this.Dock = DockStyle.Fill;
            this.Name = "ControlSurface";

            this.ResumeLayout(false);
        }


        // ============================================================
        // BUTTON CONFIGURATION
        // ============================================================

        private void ConfigureButton(
            Button button,
            string text,
            Color backColor)
        {
            button.Text = text;

            // button.Dock = DockStyle.Fill;

            // button.Margin = new Padding(10);

            button.Font = new Font(
                "Arial",
                12,
                FontStyle.Bold);

            button.BackColor = backColor;

            button.ForeColor = Color.Black;

            button.FlatStyle = FlatStyle.Flat;

            button.UseVisualStyleBackColor = false;
            button.Size = new Size(100, 50);
        }


        // ============================================================
        // BUTTON EVENTS
        // ============================================================

        private void BtnRollRight_Click(object sender, EventArgs e)
        {
            SendRCOverride(1900, UInt16.MaxValue, UInt16.MaxValue);
            Thread.Sleep(1000);
            SendRCOverride(1500, 1500, 1500);
        }

        private void BtnRollLeft_Click(object sender, EventArgs e)
        {
            SendRCOverride(1100, UInt16.MaxValue, UInt16.MaxValue);
            Thread.Sleep(1000);
            SendRCOverride(1500, 1500, 1500);
        }

        private void BtnPitchUp_Click(object sender, EventArgs e)
        {
            SendRCOverride(UInt16.MaxValue, 1900, UInt16.MaxValue);
            Thread.Sleep(1000);
            SendRCOverride(1500, 1500, 1500);
        }

        private void BtnPitchDown_Click(object sender, EventArgs e)
        {
            SendRCOverride(UInt16.MaxValue, 1100, UInt16.MaxValue);
            Thread.Sleep(1000);
            SendRCOverride(1500, 1500, 1500);
        }

        private void BtnYawRight_Click(object sender, EventArgs e)
        {
            SendRCOverride(UInt16.MaxValue, UInt16.MaxValue, 1900);
            Thread.Sleep(1000);
            SendRCOverride(1500, 1500, 1500);
        }

        private void BtnYawLeft_Click(object sender, EventArgs e)
        {
            SendRCOverride(UInt16.MaxValue, UInt16.MaxValue, 1100);
            Thread.Sleep(1000);
            SendRCOverride(1500, 1500, 1500);
        }

        private void btnSequence_Click(object sender, EventArgs e)
        {
            SendRCOverride(1900, UInt16.MaxValue, UInt16.MaxValue);
            Thread.Sleep(1000);
            SendRCOverride(1100, UInt16.MaxValue, UInt16.MaxValue);
            Thread.Sleep(1000);
            SendRCOverride(UInt16.MaxValue, 1900, UInt16.MaxValue);
            Thread.Sleep(1000);
            SendRCOverride(UInt16.MaxValue, 1100, UInt16.MaxValue);
            Thread.Sleep(1000);
            SendRCOverride(UInt16.MaxValue, UInt16.MaxValue, 1900);
            Thread.Sleep(1000);
            SendRCOverride(UInt16.MaxValue, UInt16.MaxValue, 1100);
            Thread.Sleep(1000);
            SendRCOverride(1500, 1500, 1500);
        }

        // ============================================================
        // SEND PWM TO FLIGHT CONTROLLER
        // ============================================================

        private void SendRCOverride(
            ushort roll,
            ushort pitch,
            ushort yaw)
        {
            try
            {
                MAVLink.mavlink_rc_channels_override_t packet =
                    new MAVLink.mavlink_rc_channels_override_t();

                packet.target_system = MainV2.comPort.MAV.sysid;
                packet.target_component = MainV2.comPort.MAV.compid;

                // CH1 - Roll
                packet.chan1_raw = roll;

                // CH2 - Pitch
                packet.chan2_raw = pitch;

                // CH3 - Do not override
                packet.chan3_raw = UInt16.MaxValue;

                // CH4 - Yaw
                packet.chan4_raw = yaw;

                // CH5-CH8 - Do not override
                packet.chan5_raw = UInt16.MaxValue;
                packet.chan6_raw = UInt16.MaxValue;
                packet.chan7_raw = UInt16.MaxValue;
                packet.chan8_raw = UInt16.MaxValue;

                MainV2.comPort.sendPacket(
                    packet,
                    MainV2.comPort.MAV.sysid,
                    MainV2.comPort.MAV.compid);
            }
            catch (Exception ex)
            {
                // log.Error("RC Override failed", ex);
            }
        }
    }
}