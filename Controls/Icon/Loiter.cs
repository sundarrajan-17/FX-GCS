using System.Drawing;

namespace XagSurveillanceGCS.Controls.Icon
{
    public class Loiter : Icon
    {
        internal override void doPaint(Graphics g)
        {
            int w = Width;
            int h = Height;

            Rectangle rect = new Rectangle(w/4, h/4, w/2, h/2);

            g.DrawArc(LinePen, rect, 45, 300);

            PointF p1 = new PointF(rect.Right, rect.Top + rect.Height / 2);
            PointF p2 = new PointF(rect.Right - 8, rect.Top + rect.Height / 2 - 5);
            PointF p3 = new PointF(rect.Right - 8, rect.Top + rect.Height / 2 + 5);

            g.DrawLine(LinePen, p1, p2);
            g.DrawLine(LinePen, p1, p3);
        }
    }
}