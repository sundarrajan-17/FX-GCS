using System.Drawing;

namespace XagSurveillanceGCS.Controls.Icon
{
    public class Land : Icon
    {
        internal override void doPaint(Graphics g)
        {
            int w = Width;
            int h = Height;

            g.DrawLine(LinePen, w * 0.2f, h * 0.8f, w * 0.8f, h * 0.8f);

            g.DrawLine(LinePen, w * 0.5f, h * 0.2f, w * 0.5f, h * 0.75f);

            g.DrawLine(LinePen, w * 0.5f, h * 0.75f, w * 0.4f, h * 0.65f);
            g.DrawLine(LinePen, w * 0.5f, h * 0.75f, w * 0.6f, h * 0.65f);
        }
    }
}