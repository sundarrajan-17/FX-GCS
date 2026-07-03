using System.Drawing;

namespace XagSurveillanceGCS.Controls.Icon
{
    public class Waypoint : Icon
    {
        internal override void doPaint(Graphics g)
        {
            int w = Width;
            int h = Height;

            Rectangle circle = new Rectangle(w/3, h/3, w/3, h/3);

            g.DrawEllipse(LinePen, circle);

            g.FillEllipse(Brushes.Black,
                w/2-2,
                h/2-2,
                4,
                4);
        }
    }
}