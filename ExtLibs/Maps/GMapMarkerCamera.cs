using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Collections.Generic;      // For List<>

using XagSurveillanceGCS.Utilities;
using XagSurveillanceGCS;

namespace XagSurveillanceGCS.Maps
{
    public class GMapMarkerCamera : GMapMarker
    {
        private static readonly int BlueDotRadius = 8;
        // private static readonly int HeadingLineLength = 500; // Long line like plane icon

        public double YawDegrees { get; private set; }

        public GMapMarkerCamera(PointLatLng vehiclePos, double yawDegrees)
            : base(vehiclePos)
        {
            YawDegrees = yawDegrees;
            Size = new Size(1, 1); // Not used directly
            Offset = new Point(0, 0); // Marker is centered by default
        }

        public void UpdateYaw(double yawDegrees)
        {
            YawDegrees = yawDegrees;
        }

        public void UpdateCamPos(PointLatLng newPos)
        {
            this.Position = newPos;
        }

        public override void OnRender(IGraphics g)
        {
            if (Overlay?.Control == null)
                return;

            if (!Overlay.Control.ViewArea.Contains(Position))
                return;

            var mapControl = Overlay.Control;
            var state = g.Save(); // Save current graphics state

            // Move to vehicle screen position
            g.TranslateTransform(LocalPosition.X, LocalPosition.Y);
            // Rotate with map bearing to keep drawing consistent with map
            g.RotateTransform(-mapControl.Bearing);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw blue dot at center
            using (SolidBrush blueBrush = new SolidBrush(Color.Blue))
            using (Pen blackPen = new Pen(Color.Black, 2))
            {
                g.FillEllipse(blueBrush, -BlueDotRadius, -BlueDotRadius, BlueDotRadius * 2, BlueDotRadius * 2);
                g.DrawEllipse(blackPen, -BlueDotRadius, -BlueDotRadius, BlueDotRadius * 2, BlueDotRadius * 2);
            }

            // Calculate length of yaw line to reach edge of screen
            float yawLength = Math.Max(mapControl.Width, mapControl.Height);



            // Draw long yaw/heading line (rotated correctly)
            double yawRad = (YawDegrees - 90) * MathHelper.deg2rad; // adjust so 0 is up
            PointF lineEnd = new PointF(
                (float)Math.Cos(yawRad) * yawLength,
                (float)Math.Sin(yawRad) * yawLength
            );

            using (Pen yawPen = new Pen(Color.DeepPink, 2))
            {
                g.DrawLine(yawPen, PointF.Empty, lineEnd);
            }

            g.Restore(state); // Restore original transform
        }
    }
}

namespace XagSurveillanceGCS.Maps
{
    public class GMapMarkerCamTarget : GMapMarker
    {
        private static int RedDotRadius = 7;

        // Camera parameters (example values)
        public double Altitude = 100;         // meters
        public double hfovDeg = 60f;         // degrees
        public double vfovDeg = 40f;         // degrees
        public double distance = 100f;       // meters from target to camera
        public double targetLat = 0f;        // target latitude
        public double targetLon = 0f;

        // Camera orientation
        public double Yaw { get; private set; }

        public GMapMarkerCamTarget(PointLatLng targetPos)
            : base(targetPos)
        {
            // Marker center (offset ensures it's centered)
            Yaw = 0f;
            Size = new Size(1, 1);
            Offset = new Point(-Size.Width / 2, -Size.Height / 2);
        }

        public void updatePolygonData(double hfov, double vfov, double distance, double targetLat, double targetLon,double yaw)
        {
            this.hfovDeg = hfov;
            this.vfovDeg = vfov;
            this.Yaw = yaw;
            this.targetLat = targetLat;
            this.targetLon = targetLon;
            this.distance = distance;
        }

        public override void OnRender(IGraphics g)
        {
            if (Overlay?.Control == null)
                return;

            if (!Overlay.Control.ViewArea.Contains(Position))
                return;

            var map = Overlay.Control;

            // Calculate Field of View angles in radians
            // double fovX = 2 * Math.Atan(SensorWidth / (2 * FocalLength));
            // double fovY = 2 * Math.Atan(SensorHeight / (2 * FocalLength));

            // // Ground coverage in meters
            // double groundWidth = 2 * Altitude * Math.Tan(fovX / 2);
            // double groundHeight = 2 * Altitude * Math.Tan(fovY / 2);

            // // Approximate conversion from meters to degrees
            // double halfLatDeg = groundHeight / 2 / 111320.0;
            // double halfLngDeg = groundWidth / 2 / (111320.0 * Math.Cos(Position.Lat * Math.PI / 180));

            // // Four corners of the field of view (axis-aligned)
            // var corners = new List<PointLatLng>
            // {
            //     new PointLatLng(Position.Lat + halfLatDeg, Position.Lng - halfLngDeg), // Top-Left
            //     new PointLatLng(Position.Lat + halfLatDeg, Position.Lng + halfLngDeg), // Top-Right
            //     new PointLatLng(Position.Lat - halfLatDeg, Position.Lng + halfLngDeg), // Bottom-Right
            //     new PointLatLng(Position.Lat - halfLatDeg, Position.Lng - halfLngDeg)  // Bottom-Left
            // };

            List<PointLatLng> corners = new List<PointLatLng>();

            double hfov = hfovDeg * Math.PI / 180.0;
            double vfov = vfovDeg * Math.PI / 180.0;
            double yaw = -Yaw * Math.PI / 180.0;

            double width = 2 * distance * Math.Tan(hfov / 2);
            double height = 2 * distance * Math.Tan(vfov / 2);

            double halfW = width / 2;
            double halfH = height / 2;

            var corners1 = new List<(double x, double y)>
            {
                (-halfW,  halfH),
                ( halfW,  halfH),
                ( halfW, -halfH),
                (-halfW, -halfH)
            };

            foreach (var c in corners1)
            {
                // rotate
                // double xr = c.x * Math.Cos(yaw) - c.y * Math.Sin(yaw);
                // double yr = c.x * Math.Sin(yaw) + c.y * Math.Cos(yaw);

                double latOffset = c.y / 111320.0;
                double lonOffset = c.x / (111320.0 * Math.Cos(targetLat * Math.PI / 180));

                // Console.WriteLine($"Corner offset (lat, lon): ({latOffset}, {lonOffset})");

                corners.Add(new PointLatLng(
                    targetLat + latOffset,
                    targetLon + lonOffset));
            }

            List<PointLatLng> rotated = new List<PointLatLng>();

            double angleRad = -Yaw * Math.PI / 180.0;

            foreach (var p in corners)
            {
                double x = p.Lng;
                double y = p.Lat;

                double cx = targetLon;
                double cy = targetLat;

                double xr = cx + (x - cx) * Math.Cos(angleRad) - (y - cy) * Math.Sin(angleRad);
                double yr = cy + (x - cx) * Math.Sin(angleRad) + (y - cy) * Math.Cos(angleRad);

                rotated.Add(new PointLatLng(yr, xr));
            }

            // Convert map coordinates to screen space
            GPoint centerGP = map.FromLatLngToLocal(Position);
            PointF center = new PointF(LocalPosition.X, LocalPosition.Y);

            double yawRad = Yaw * Math.PI / 180.0;

            List<PointF> screenPoints = new List<PointF>();
            foreach (var corner in rotated)
            {
                GPoint pt = map.FromLatLngToLocal(corner);
                float dx = (float)(pt.X - centerGP.X);
                float dy = (float)(pt.Y - centerGP.Y);

                // Rotate point by yaw angle
                // float rotatedX = (float)(dx * Math.Cos(yawRad) - dy * Math.Sin(yawRad));
                // float rotatedY = (float)(dx * Math.Sin(yawRad) + dy * Math.Cos(yawRad));

                screenPoints.Add(new PointF(center.X + dx, center.Y + dy));
            }

            // Draw semi-transparent light blue polygon (camera FOV)
            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(100, Color.Blue)))
            using (Pen borderPen = new Pen(Color.Blue, 2))
            {
                g.FillPolygon(fillBrush, screenPoints.ToArray());
                g.DrawPolygon(borderPen, screenPoints.ToArray());
            }

            // Draw red dot and white plus at camera center (target)
            using (SolidBrush redBrush = new SolidBrush(Color.Red))
            using (Pen whitePen = new Pen(Color.White, 2))
            {
                g.FillEllipse(redBrush, center.X - RedDotRadius, center.Y - RedDotRadius,
                    RedDotRadius * 2, RedDotRadius * 2);

                float plusSize = RedDotRadius * 0.6f;
                g.DrawLine(whitePen, center.X - plusSize, center.Y, center.X + plusSize, center.Y);
                g.DrawLine(whitePen, center.X, center.Y - plusSize, center.X, center.Y + plusSize);
            }
        }
    }
}
