using System.Drawing;

namespace XagSurveillanceGCS.Controls.Icon
{
    public class Takeoff : Icon
    {
        internal override void doPaint(Graphics g)
        {
            int w = Width;
            int h = Height;

            // Ground
            g.DrawLine(LinePen, w * 0.2f, h * 0.8f, w * 0.8f, h * 0.8f);

            // Vertical line
            g.DrawLine(LinePen, w * 0.5f, h * 0.75f, w * 0.5f, h * 0.25f);

            // Arrow
            g.DrawLine(LinePen, w * 0.5f, h * 0.25f, w * 0.4f, h * 0.35f);
            g.DrawLine(LinePen, w * 0.5f, h * 0.25f, w * 0.6f, h * 0.35f);
        }
    }
}