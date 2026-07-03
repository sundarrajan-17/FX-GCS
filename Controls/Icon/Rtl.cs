using System.Drawing;

namespace XagSurveillanceGCS.Controls.Icon
{
    public class RTL : Icon
    {
        internal override void doPaint(Graphics g)
        {
            int w = Width;
            int h = Height;

            Rectangle rect = new Rectangle(w / 4, h / 4, w / 2, h / 2);

            g.DrawArc(LinePen, rect, 45, 270);

            // Arrow
            g.DrawLine(LinePen,
                rect.Left + 5,
                rect.Top + 10,
                rect.Left,
                rect.Top);

            g.DrawLine(LinePen,
                rect.Left + 5,
                rect.Top + 10,
                rect.Left + 12,
                rect.Top + 5);

            // Home
            PointF[] roof =
            {
                new PointF(w*0.45f,h*0.55f),
                new PointF(w*0.5f,h*0.45f),
                new PointF(w*0.55f,h*0.55f)
            };

            g.DrawPolygon(LinePen, roof);
            g.DrawRectangle(LinePen, w*0.46f, h*0.55f, w*0.08f, h*0.1f);
        }
    }
}