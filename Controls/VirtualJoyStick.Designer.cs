namespace MissionPlanner.Controls
{
    partial class VirtualJoystick
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlJoystick;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlJoystick = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            // pnlJoystick
            this.pnlJoystick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlJoystick.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.pnlJoystick.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlJoystick_Paint);
            this.pnlJoystick.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlJoystick_MouseDown);
            this.pnlJoystick.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlJoystick_MouseMove);
            this.pnlJoystick.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlJoystick_MouseUp);

            // VirtualJoystick
            this.Controls.Add(this.pnlJoystick);
            this.Size = new System.Drawing.Size(150, 140);
            this.ResumeLayout(false);
        }
    }
}
