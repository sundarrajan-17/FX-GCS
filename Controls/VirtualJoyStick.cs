using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using XagSurveillanceGCS.Controls;

namespace XagSurveillanceGCS.Controls
{
    public partial class VirtualJoystick : UserControl
    {
        private Point center;
        private Point knobPosition;
        private int baseRadius;
        private int knobRadius = 15;
        private bool dragging = false;

        // Output values (-1 to +1)
        public float XValue { get; private set; }
        public float YValue { get; private set; }

        public event Action<float, float> ValueChanged;

        private BaseCameraController _parentControl;   

        public VirtualJoystick(BaseCameraController parent)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this._parentControl = parent;   
            this.Load += (s, e) => InitializeJoystick();
            this.Resize += (s, e) => InitializeJoystick();
        }

        private void InitializeJoystick()
        {
            if (pnlJoystick.Width == 0 || pnlJoystick.Height == 0)
                return;

            center = new Point(pnlJoystick.Width / 2, pnlJoystick.Height / 2);
            baseRadius = Math.Min(pnlJoystick.Width, pnlJoystick.Height) / 2 - 8;
            knobPosition = center;

            XValue = 0;
            YValue = 0;

            pnlJoystick.Invalidate();
        }

        private void pnlJoystick_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Base circle
            using (var baseBrush = new SolidBrush(Color.FromArgb(60, 60, 60)))
                g.FillEllipse(baseBrush,
                    center.X - baseRadius,
                    center.Y - baseRadius,
                    baseRadius * 2,
                    baseRadius * 2);

            using (var basePen = new Pen(Color.LimeGreen, 2))
                g.DrawEllipse(basePen,
                    center.X - baseRadius,
                    center.Y - baseRadius,
                    baseRadius * 2,
                    baseRadius * 2);
            
            using (var linePen = new Pen(Color.LimeGreen, 1))
            {
                // Horizontal line (left ↔ right)
                g.DrawLine(linePen,
                    center.X - baseRadius, center.Y,
                    center.X + baseRadius, center.Y);

                // Vertical line (top ↕ bottom)
                g.DrawLine(linePen,
                    center.X, center.Y - baseRadius,
                    center.X, center.Y + baseRadius);
            }

            using (var arrowPen = new Pen(Color.LimeGreen, 2))
            {
                arrowPen.CustomEndCap = new AdjustableArrowCap(6, 6);

                // ➡ Right arrow
                g.DrawLine(arrowPen,
                    center.X + baseRadius - 15, center.Y,
                    center.X + baseRadius, center.Y);

                // ⬅ Left arrow
                g.DrawLine(arrowPen,
                    center.X - baseRadius + 15, center.Y,
                    center.X - baseRadius, center.Y);

                // ⬇ Bottom arrow
                g.DrawLine(arrowPen,
                    center.X, center.Y + baseRadius - 15,
                    center.X, center.Y + baseRadius);

                // ⬆ Top arrow
                g.DrawLine(arrowPen,
                    center.X, center.Y - baseRadius + 15,
                    center.X, center.Y - baseRadius);
            }

            // Knob
            using (var knobBrush = new SolidBrush(Color.LimeGreen))
                g.FillEllipse(knobBrush,
                    knobPosition.X - knobRadius,
                    knobPosition.Y - knobRadius,
                    knobRadius * 2,
                    knobRadius * 2);
        }

        private void pnlJoystick_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            UpdateKnob(e.Location);
        }

        private void pnlJoystick_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            UpdateKnob(e.Location);
        }

        private void pnlJoystick_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            knobPosition = center;
            XValue = 0;
            YValue = 0;
            ValueChanged?.Invoke(XValue, YValue);
            if(this._parentControl.SelectedCamera == "XAGCAM2")
            {     
                this._parentControl._flightData.GremsyControlPitchYaw(XValue, YValue);
            }
            else
            {
                this._parentControl._flightData.XagCamPitchYawCommand(XValue,YValue);
            }
            pnlJoystick.Invalidate();
        }

        private void UpdateKnob(Point mouse)
        {
            int dx = mouse.X - center.X;
            int dy = mouse.Y - center.Y;

            double dist = Math.Sqrt(dx * dx + dy * dy);
            if (dist > baseRadius)
            {
                dx = (int)(dx * baseRadius / dist);
                dy = (int)(dy * baseRadius / dist);
            }

            knobPosition = new Point(center.X + dx, center.Y + dy);

            XValue = (float)dx / baseRadius;
            YValue = -(float)dy / baseRadius; // inverted Y

            ValueChanged?.Invoke(XValue, YValue);
            pnlJoystick.Invalidate();
            Console.WriteLine($"Joystick Updated: X={XValue:F2}, Y={YValue:F2}");
            if(this._parentControl.SelectedCamera == "XAGCAM2")
            {     
                this._parentControl._flightData.GremsyControlPitchYaw(XValue*0.5, YValue*0.5);
            }
            else
            {
                this._parentControl._flightData.XagCamPitchYawCommand(XValue,YValue);
            }
        }
    }
}
