using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using XagSurveillanceGCS.GCSViews;
using XagSurveillanceGCS.Utilities;
using ZedGraph;

namespace XagSurveillanceGCS.Controls
{
    public partial class ElevationGraphControl : UserControl
    {
        public enum HazardLevel
        {
            Safe,       // White (> 150m clearance)
            Caution,    // Orange (100m - 150m clearance)
            Warning,    // Coral / Light Red (0m - 100m clearance)
            Collision   // Dark Red (<= 0m clearance)
        }

        public class RadialArcSector
        {
            public int WaypointIndex { get; set; }
            public PointLatLngAlt Center { get; set; }
            public double StartAngleDeg { get; set; }
            public double EndAngleDeg { get; set; }
            public double RadiusMeters { get; set; }
            public HazardLevel Hazard { get; set; }
            public Color DisplayColor { get; set; }
        }

        public class EvaluatedWpInfo
        {
            public PointLatLngAlt Location { get; set; }
            public string Command { get; set; }
            public double CustomRadius { get; set; }

            public double Lat => Location != null ? Location.Lat : 0;
            public double Lng => Location != null ? Location.Lng : 0;
            public double Alt
            {
                get => Location != null ? Location.Alt : 0;
                set
                {
                    if (Location != null) Location.Alt = value;
                }
            }
            public string Tag
            {
                get => Location?.Tag;
                set
                {
                    if (Location != null) Location.Tag = value?.ToString();
                }
            }
        }

        private class SegmentPath
        {
            public PointPairList Points = new PointPairList();
            public Color SegmentColor = Color.Green;
        }

        private class LoiterProfileMark
        {
            public double DistX { get; set; }
            public double FlightAltMsl { get; set; }
            public double MaxTerrainMsl { get; set; }
            public double Radius { get; set; }
            public HazardLevel Hazard { get; set; }
            public string WpTag { get; set; }
        }

        private ZedGraphControl zgc;
        private List<EvaluatedWpInfo> planlocs = new List<EvaluatedWpInfo>();
        private PointPairList listPlannedAll = new PointPairList();
        private PointPairList listDEM = new PointPairList();
        private List<SegmentPath> plannedSegments = new List<SegmentPath>();
        private List<LoiterProfileMark> loiterGraphMarks = new List<LoiterProfileMark>();

        private double totalDistance = 0;
        private double homealt = 0;
        private FlightPlanner.altmode altmode = FlightPlanner.altmode.Relative;
        private double loiterRadiusDefault = 30.0;
        private double wpRadiusDefault = 5.0;

        public bool ElevationIsClear { get; private set; } = true;
        public HashSet<int> CollidingWaypointIndices { get; private set; } = new HashSet<int>();
        public HashSet<int> WarningWaypointIndices { get; private set; } = new HashSet<int>();
        public List<RadialArcSector> CircleArcSectors { get; private set; } = new List<RadialArcSector>();

        public event Action<bool, HashSet<int>, HashSet<int>, List<RadialArcSector>> OnElevationEvaluated;

        public ElevationGraphControl()
        {
            InitializeComponentLayout();
        }

        private void InitializeComponentLayout()
        {
            zgc = new ZedGraphControl
            {
                Dock = DockStyle.Fill,
                IsShowPointValues = true,
                BackColor = Color.FromArgb(38, 39, 40)
            };
            zgc.PointValueEvent += Zgc_PointValueEvent;

            Controls.Add(zgc);
            Size = new Size(600, 220);
        }

        private string Zgc_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            return $"{curve.Label.Text}\nDist: {pt.X:F1} {CurrentState.DistanceUnit}\nAlt: {pt.Y:F1} {CurrentState.AltUnit}";
        }

        public void UpdateElevation(List<EvaluatedWpInfo> locations, double homeAltitude, FlightPlanner.altmode mode, double loiterRadius, double wpRadius)
        {
            homealt = homeAltitude;
            altmode = mode;
            loiterRadiusDefault = loiterRadius > 0 ? loiterRadius : 30.0;
            wpRadiusDefault = wpRadius > 0 ? wpRadius : 5.0;

            planlocs = locations?.Where(l => l?.Location != null && (l.Location.Tag == null || !l.Location.Tag.ToString().Contains("ROI"))).ToList() 
                            ?? new List<EvaluatedWpInfo>();

            CalculateAndRender();
        }

        public void Clear()
        {
            listPlannedAll.Clear();
            listDEM.Clear();
            plannedSegments.Clear();
            CircleArcSectors.Clear();
            loiterGraphMarks.Clear();
            CollidingWaypointIndices.Clear();
            WarningWaypointIndices.Clear();
            ElevationIsClear = true;
            totalDistance = 0;

            if (zgc?.GraphPane != null)
            {
                zgc.GraphPane.CurveList.Clear();
                zgc.GraphPane.GraphObjList.Clear();
                RenderEmptyChart();
            }
        }

        private void CalculateAndRender()
        {
            Clear();

            if (planlocs == null || planlocs.Count < 2)
            {
                RenderEmptyChart();
                OnElevationEvaluated?.Invoke(true, CollidingWaypointIndices, WarningWaypointIndices, CircleArcSectors);
                return;
            }

            // 1. Linear Flight Path Evaluation
            ExecuteLinearFlightPathEvaluation();

            // 2. Command-Specific Radial Perimeter Evaluation (WP Radius vs Loiter Radius)
            ExecuteRadialPerimeterSectorEvaluation();

            RenderChart();
            OnElevationEvaluated?.Invoke(ElevationIsClear, CollidingWaypointIndices, WarningWaypointIndices, CircleArcSectors);
        }

        private void ExecuteLinearFlightPathEvaluation()
        {
            double accumDist = 0;
            PointLatLngAlt lastloc = null;

            for (int i = 0; i < planlocs.Count; i++)
            {
                var planloc = planlocs[i].Location;
                if (lastloc != null)
                    accumDist += planloc.GetDistance(lastloc);

                double wpMslAlt = 0;
                if (altmode == FlightPlanner.altmode.Absolute)
                    wpMslAlt = planloc.Alt * CurrentState.multiplieralt;
                else if (altmode == FlightPlanner.altmode.Terrain)
                    wpMslAlt = (srtm.getAltitude(planloc.Lat, planloc.Lng).alt + planloc.Alt) * CurrentState.multiplieralt;
                else // Relative
                    wpMslAlt = (homealt + planloc.Alt) * CurrentState.multiplieralt;

                listPlannedAll.Add(accumDist * CurrentState.multiplierdist, wpMslAlt, 0, planloc.Tag ?? (i == 0 ? "H" : i.ToString()));
                lastloc = planloc;
            }

            totalDistance = accumDist;

            double disttotal = 0;
            lastloc = null;
            SegmentPath currentSeg = null;

            for (int i = 0; i < planlocs.Count; i++)
            {
                var loc = planlocs[i].Location;
                string currentCmd = planlocs[i].Command ?? "WAYPOINT";

                if (lastloc == null)
                {
                    lastloc = new PointLatLngAlt(loc.Lat, loc.Lng, loc.Alt, loc.Tag);
                    continue;
                }

                double segDist = lastloc.GetDistance(loc);
                
                double startMsl = 0;
                double endMsl = 0;

                if (altmode == FlightPlanner.altmode.Absolute)
                {
                    startMsl = lastloc.Alt * CurrentState.multiplieralt;
                    endMsl = loc.Alt * CurrentState.multiplieralt;
                }
                else if (altmode == FlightPlanner.altmode.Terrain)
                {
                    startMsl = (srtm.getAltitude(lastloc.Lat, lastloc.Lng).alt + lastloc.Alt) * CurrentState.multiplieralt;
                    endMsl = (srtm.getAltitude(loc.Lat, loc.Lng).alt + loc.Alt) * CurrentState.multiplieralt;
                }
                else // Relative
                {
                    startMsl = (homealt + lastloc.Alt) * CurrentState.multiplieralt;
                    endMsl = (homealt + loc.Alt) * CurrentState.multiplieralt;
                }

                int points = (int)(segDist / 15.0) + 1;
                double steplat = (lastloc.Lat - loc.Lat) / points;
                double steplng = (lastloc.Lng - loc.Lng) / points;
                double stepalt = (startMsl - endMsl) / points;

                PointLatLngAlt lastpnt = lastloc;

                for (int a = 0; a <= points; a++)
                {
                    double lat = lastloc.Lat - (steplat * a);
                    double lng = lastloc.Lng - (steplng * a);
                    double planInterpolatedAlt = startMsl - (stepalt * a);

                    var newpoint = new PointLatLngAlt(lat, lng, srtm.getAltitude(lat, lng).alt, "");
                    disttotal += lastpnt.GetDistance(newpoint);

                    double srtmTerrainMsl = newpoint.Alt * CurrentState.multiplieralt;
                    double currentDistScaled = disttotal * CurrentState.multiplierdist;

                    listDEM.Add(currentDistScaled, srtmTerrainMsl);

                    double clearance = planInterpolatedAlt - srtmTerrainMsl;
                    Color pointColor;

                    // Exclude takeoff/landing descent transitions from false ground collisions
                    bool isTakeoffTransition = (i == 1) || (lastloc.Tag?.ToString() == "H") 
                                               || currentCmd.Contains("TAKEOFF") 
                                               || currentCmd.Contains("VTOL_TAKEOFF");
                    bool isLandingTransition = currentCmd.Contains("LAND") 
                                               || currentCmd.Contains("VTOL_LAND");

                    if (clearance <= 0 && !isTakeoffTransition && !isLandingTransition)
                    {
                        pointColor = Color.Red;
                        ElevationIsClear = false;

                        if (int.TryParse(lastloc.Tag?.ToString(), out int wpStart) && wpStart > 0) CollidingWaypointIndices.Add(wpStart);
                        if (int.TryParse(loc.Tag?.ToString(), out int wpEnd) && wpEnd > 0) CollidingWaypointIndices.Add(wpEnd);
                    }
                    else if (clearance <= (100.0 * CurrentState.multiplieralt) && !isTakeoffTransition && !isLandingTransition)
                    {
                        pointColor = Color.FromArgb(255, 102, 102);
                        if (int.TryParse(lastloc.Tag?.ToString(), out int wpStart) && wpStart > 0) WarningWaypointIndices.Add(wpStart);
                        if (int.TryParse(loc.Tag?.ToString(), out int wpEnd) && wpEnd > 0) WarningWaypointIndices.Add(wpEnd);
                    }
                    else if (clearance <= (150.0 * CurrentState.multiplieralt) && !isTakeoffTransition && !isLandingTransition)
                    {
                        pointColor = Color.Orange;
                        if (int.TryParse(lastloc.Tag?.ToString(), out int wpStart) && wpStart > 0) WarningWaypointIndices.Add(wpStart);
                        if (int.TryParse(loc.Tag?.ToString(), out int wpEnd) && wpEnd > 0) WarningWaypointIndices.Add(wpEnd);
                    }
                    else
                    {
                        pointColor = Color.LimeGreen;
                    }

                    if (currentSeg == null || currentSeg.SegmentColor != pointColor)
                    {
                        var prevPoint = currentSeg?.Points.LastOrDefault();
                        currentSeg = new SegmentPath { SegmentColor = pointColor };
                        if (prevPoint != null)
                            currentSeg.Points.Add(prevPoint.X, prevPoint.Y);
                        plannedSegments.Add(currentSeg);
                    }

                    currentSeg.Points.Add(currentDistScaled, planInterpolatedAlt);
                    lastpnt = newpoint;
                }

                lastloc = new PointLatLngAlt(loc.Lat, loc.Lng, loc.Alt, loc.Tag);
            }
        }

        private void ExecuteRadialPerimeterSectorEvaluation()
        {
            const double R = 6371000.0;
            const int angleStep = 15; // 24 segments per circle

            for (int i = 0; i < planlocs.Count; i++)
            {
                var item = planlocs[i];
                var loc = item.Location;
                if (loc == null || loc.Tag?.ToString() == "H") continue;

                int wpIndex = 0;
                if (!int.TryParse(loc.Tag?.ToString(), out wpIndex))
                    wpIndex = i + 1;

                string cmdType = item.Command ?? "WAYPOINT";
                bool isLoiterCommand = cmdType.Contains("LOITER");

                // WAYPOINT & VTOL_LAND use wpRadiusDefault; LOITER commands use loiterRadius
                double baseRadius = 0;
                if (isLoiterCommand)
                {
                    baseRadius = item.CustomRadius > 0 ? item.CustomRadius : loiterRadiusDefault;
                }
                else
                {
                    baseRadius = wpRadiusDefault;
                }

                // Compute MSL Flight Alt
                double wpAltMsl = 0;
                if (altmode == FlightPlanner.altmode.Absolute)
                    wpAltMsl = loc.Alt * CurrentState.multiplieralt;
                else if (altmode == FlightPlanner.altmode.Terrain)
                    wpAltMsl = (srtm.getAltitude(loc.Lat, loc.Lng).alt + loc.Alt) * CurrentState.multiplieralt;
                else // Relative
                    wpAltMsl = (homealt + loc.Alt) * CurrentState.multiplieralt;

                double maxLoiterTerrainSeen = 0;
                HazardLevel worstLoiterHazard = HazardLevel.Safe;

                // 1. Inner Perimeter Circle
                for (int angleDeg = 0; angleDeg < 360; angleDeg += angleStep)
                {
                    HazardLevel innerHazard = HazardLevel.Safe;
                    Color innerColor = Color.White;

                    for (double rDist = 5.0; rDist <= baseRadius; rDist += 5.0)
                    {
                        double rad = angleDeg * (Math.PI / 180.0);
                        double latRad = loc.Lat * (Math.PI / 180.0);
                        double lngRad = loc.Lng * (Math.PI / 180.0);

                        double dByR = rDist / R;
                        double radialLat = Math.Asin(Math.Sin(latRad) * Math.Cos(dByR) + Math.Cos(latRad) * Math.Sin(dByR) * Math.Cos(rad));
                        double radialLng = lngRad + Math.Atan2(Math.Sin(rad) * Math.Sin(dByR) * Math.Cos(latRad), Math.Cos(dByR) - Math.Sin(latRad) * Math.Sin(radialLat));

                        double sampleLat = radialLat * (180.0 / Math.PI);
                        double sampleLng = radialLng * (180.0 / Math.PI);

                        double terrainHeight = srtm.getAltitude(sampleLat, sampleLng).alt * CurrentState.multiplieralt;
                        double clearance = wpAltMsl - terrainHeight;

                        if (terrainHeight > maxLoiterTerrainSeen)
                            maxLoiterTerrainSeen = terrainHeight;

                        if (clearance <= 0)
                        {
                            innerHazard = HazardLevel.Collision;
                            innerColor = Color.DarkRed;
                            ElevationIsClear = false;
                            if (wpIndex > 0) CollidingWaypointIndices.Add(wpIndex);
                            break;
                        }
                        else if (clearance <= (100.0 * CurrentState.multiplieralt))
                        {
                            if (innerHazard < HazardLevel.Warning)
                            {
                                innerHazard = HazardLevel.Warning;
                                innerColor = Color.FromArgb(255, 102, 102);
                                if (wpIndex > 0) WarningWaypointIndices.Add(wpIndex);
                            }
                        }
                        else if (clearance <= (150.0 * CurrentState.multiplieralt))
                        {
                            if (innerHazard < HazardLevel.Caution)
                            {
                                innerHazard = HazardLevel.Caution;
                                innerColor = Color.Orange;
                                if (wpIndex > 0) WarningWaypointIndices.Add(wpIndex);
                            }
                        }
                    }

                    if (innerHazard > worstLoiterHazard)
                        worstLoiterHazard = innerHazard;

                    CircleArcSectors.Add(new RadialArcSector
                    {
                        WaypointIndex = wpIndex,
                        Center = loc,
                        StartAngleDeg = angleDeg - (angleStep / 2.0),
                        EndAngleDeg = angleDeg + (angleStep / 2.0),
                        RadiusMeters = baseRadius,
                        Hazard = innerHazard,
                        DisplayColor = innerColor
                    });
                }

                // 2. Outer +200m Buffer (Evaluated for Loiters; only renders warning/collision arcs)
                if (isLoiterCommand)
                {
                    double outerBufferRadius = baseRadius + 200.0;
                    for (int angleDeg = 0; angleDeg < 360; angleDeg += angleStep)
                    {
                        HazardLevel outerHazard = HazardLevel.Safe;
                        Color outerColor = Color.Transparent;

                        for (double rDist = baseRadius; rDist <= outerBufferRadius; rDist += 10.0)
                        {
                            double rad = angleDeg * (Math.PI / 180.0);
                            double latRad = loc.Lat * (Math.PI / 180.0);
                            double lngRad = loc.Lng * (Math.PI / 180.0);

                            double dByR = rDist / R;
                            double radialLat = Math.Asin(Math.Sin(latRad) * Math.Cos(dByR) + Math.Cos(latRad) * Math.Sin(dByR) * Math.Cos(rad));
                            double radialLng = lngRad + Math.Atan2(Math.Sin(rad) * Math.Sin(dByR) * Math.Cos(latRad), Math.Cos(dByR) - Math.Sin(latRad) * Math.Sin(radialLat));

                            double sampleLat = radialLat * (180.0 / Math.PI);
                            double sampleLng = radialLng * (180.0 / Math.PI);

                            double terrainHeight = srtm.getAltitude(sampleLat, sampleLng).alt * CurrentState.multiplieralt;
                            double clearance = wpAltMsl - terrainHeight;

                            if (clearance <= 0)
                            {
                                outerHazard = HazardLevel.Collision;
                                outerColor = Color.DarkRed;
                                if (wpIndex > 0) CollidingWaypointIndices.Add(wpIndex);
                                break;
                            }
                            else if (clearance <= (100.0 * CurrentState.multiplieralt))
                            {
                                if (outerHazard < HazardLevel.Warning)
                                {
                                    outerHazard = HazardLevel.Warning;
                                    outerColor = Color.FromArgb(255, 102, 102);
                                    if (wpIndex > 0) WarningWaypointIndices.Add(wpIndex);
                                }
                            }
                            else if (clearance <= (150.0 * CurrentState.multiplieralt))
                            {
                                if (outerHazard < HazardLevel.Caution)
                                {
                                    outerHazard = HazardLevel.Caution;
                                    outerColor = Color.Orange;
                                    if (wpIndex > 0) WarningWaypointIndices.Add(wpIndex);
                                }
                            }
                        }

                        if (outerHazard != HazardLevel.Safe)
                        {
                            CircleArcSectors.Add(new RadialArcSector
                            {
                                WaypointIndex = wpIndex,
                                Center = loc,
                                StartAngleDeg = angleDeg - (angleStep / 2.0),
                                EndAngleDeg = angleDeg + (angleStep / 2.0),
                                RadiusMeters = outerBufferRadius,
                                Hazard = outerHazard,
                                DisplayColor = outerColor
                            });
                        }
                    }

                    // Save mark for Elevation Profile Graph visualization
                    if (i < listPlannedAll.Count)
                    {
                        loiterGraphMarks.Add(new LoiterProfileMark
                        {
                            DistX = listPlannedAll[i].X,
                            FlightAltMsl = wpAltMsl,
                            MaxTerrainMsl = maxLoiterTerrainSeen,
                            Radius = baseRadius,
                            Hazard = worstLoiterHazard,
                            WpTag = loc.Tag?.ToString() ?? (i + 1).ToString()
                        });
                    }
                }
            }
        }

        private void RenderEmptyChart()
        {
            if (zgc == null) return;
            GraphPane myPane = zgc.GraphPane;
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();

            myPane.Title.Text = "Elevation above ground";
            myPane.Title.FontSpec.FontColor = Color.White;
            myPane.Title.FontSpec.Size = 12;

            myPane.XAxis.Title.Text = "Distance (" + CurrentState.DistanceUnit + ")";
            myPane.YAxis.Title.Text = "Elevation (" + CurrentState.AltUnit + ")";
            myPane.Chart.Fill = new Fill(Color.FromArgb(30, 30, 30));
            myPane.Fill = new Fill(Color.FromArgb(38, 39, 40));

            zgc.AxisChange();
            zgc.Invalidate();
        }

        private void RenderChart()
        {
            if (zgc == null) return;
            GraphPane myPane = zgc.GraphPane;
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();

            myPane.Title.Text = ElevationIsClear ? "Elevation above ground (Safe)" : "Elevation above ground (COLLISION DETECTED)";
            myPane.Title.FontSpec.FontColor = ElevationIsClear ? Color.LimeGreen : Color.Red;
            myPane.Title.FontSpec.Size = 12;

            myPane.XAxis.Title.Text = "Distance (" + CurrentState.DistanceUnit + ")";
            myPane.YAxis.Title.Text = "Elevation (" + CurrentState.AltUnit + ")";
            myPane.Chart.Fill = new Fill(Color.FromArgb(30, 30, 30));
            myPane.Fill = new Fill(Color.FromArgb(38, 39, 40));

            myPane.XAxis.Color = Color.LightGray;
            myPane.YAxis.Color = Color.LightGray;
            myPane.XAxis.Scale.FontSpec.FontColor = Color.White;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.White;
            myPane.XAxis.Title.FontSpec.FontColor = Color.White;
            myPane.YAxis.Title.FontSpec.FontColor = Color.White;

            string demLegend = listDEM.Count > 0 ? $"DEM (Min: {listDEM.Select(p => p.Y).Min():0} Max: {listDEM.Select(p => p.Y).Max():0})" : "DEM";
            LineItem curveDEM = myPane.AddCurve(demLegend, listDEM, Color.Blue, SymbolType.None);
            curveDEM.Line.Width = 2.0f;

            bool firstPlanCurve = true;
            foreach (var seg in plannedSegments)
            {
                LineItem segCurve = myPane.AddCurve(firstPlanCurve ? "Planned Path" : "", seg.Points, seg.SegmentColor, SymbolType.None);
                segCurve.Line.Width = 3.0f;
                segCurve.Label.IsVisible = firstPlanCurve;
                firstPlanCurve = false;
            }

            double minElevationBase = 0;
            if (listDEM.Count > 0)
            {
                double minDem = listDEM.Select(p => p.Y).Min();
                minElevationBase = Math.Max(0, minDem - 50.0); // Start lines just below the lowest terrain
            }

            // 1. Waypoint Guidelines
           for (int i = 0; i < planlocs.Count && i < listPlannedAll.Count; i++)
            {
                var planloc = planlocs[i];
                var pp = listPlannedAll[i];

                // Vertical Line: starts at minElevationBase rather than 0
                LineObj vLine = new LineObj(Color.FromArgb(90, 90, 90), pp.X, minElevationBase, pp.X, pp.Y)
                {
                    Line = { Style = DashStyle.Dot, Width = 1.0f },
                    Location = { CoordinateFrame = CoordType.AxisXYScale },
                    IsClippedToChartRect = true,
                    ZOrder = ZOrder.D_BehindAxis
                };
                myPane.GraphObjList.Add(vLine);

                // Waypoint Label: placed right at the waypoint peak and clipped to graph rect
                string wpTag = planloc.Tag?.ToString() ?? (i == 0 ? "H" : i.ToString());
                TextObj textWp = new TextObj(wpTag, pp.X, pp.Y)
                {
                    FontSpec =
                    {
                        FontColor = Color.White,
                        Size = 9,
                        Fill = { IsVisible = false },
                        Border = { IsVisible = false }
                    },
                    Location = { AlignH = AlignH.Center, AlignV = AlignV.Bottom, CoordinateFrame = CoordType.AxisXYScale },
                    IsClippedToChartRect = true
                };
                myPane.GraphObjList.Add(textWp);
            }

            // 2. Direct Loiter Radius Hazard Envelope Visuals on Elevation Profile Graph
            double maxGraphDist = totalDistance * CurrentState.multiplierdist;
            foreach (var lmark in loiterGraphMarks)
            {
                Color boxFill = lmark.Hazard == HazardLevel.Collision ? Color.FromArgb(70, 220, 20, 20) :
                               (lmark.Hazard == HazardLevel.Warning ? Color.FromArgb(60, 255, 102, 102) :
                               (lmark.Hazard == HazardLevel.Caution ? Color.FromArgb(60, 255, 165, 0) : Color.FromArgb(40, 0, 191, 255)));

                Color boxBorder = lmark.Hazard == HazardLevel.Collision ? Color.Red :
                                 (lmark.Hazard == HazardLevel.Warning ? Color.FromArgb(255, 102, 102) :
                                 (lmark.Hazard == HazardLevel.Caution ? Color.Orange : Color.FromArgb(0, 191, 255)));

                // Clamp horizontal footprint strictly within the mission start and finish
                double radiusDist = lmark.Radius * CurrentState.multiplierdist;
                double leftX = Math.Max(0, lmark.DistX - radiusDist);
                double rightX = Math.Min(maxGraphDist, lmark.DistX + radiusDist);
                double width = Math.Max(1.0, rightX - leftX);

                double topY = lmark.FlightAltMsl;
                double botY = Math.Max(0, lmark.MaxTerrainMsl);
                double height = Math.Max(5.0, topY - botY);

                // BoxObj constructor: (x, y, width, height, borderColor, fillColor)
                BoxObj loiterBox = new BoxObj(leftX, topY, width, height, boxBorder, boxFill)
                {
                    Location = { CoordinateFrame = CoordType.AxisXYScale },
                    ZOrder = ZOrder.E_BehindCurves,
                    IsClippedToChartRect = true // Prevents box from spilling outside graph view
                };
                
                // Use .Border instead of .Line
                loiterBox.Border.Width = 1.5f;
                loiterBox.Border.Style = DashStyle.Dash;
                loiterBox.Border.Color = boxBorder;
                
                myPane.GraphObjList.Add(loiterBox);

                // Label clipped cleanly inside chart
                TextObj loiterLabel = new TextObj($"Loiter R:{lmark.Radius:0}m", lmark.DistX, topY)
                {
                    FontSpec =
                    {
                        FontColor = lmark.Hazard == HazardLevel.Collision ? Color.Red : Color.FromArgb(180, 230, 255),
                        Size = 8,
                        Fill = { IsVisible = false },
                        Border = { IsVisible = false }
                    },
                    Location = { AlignH = AlignH.Center, AlignV = AlignV.Bottom, CoordinateFrame = CoordType.AxisXYScale },
                    IsClippedToChartRect = true
                };
                myPane.GraphObjList.Add(loiterLabel);
            }

            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.FromArgb(60, 60, 60);
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.Color = Color.FromArgb(60, 60, 60);

            // Fit X-Axis exactly to the mission track distance
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = Math.Max(10.0, totalDistance * CurrentState.multiplierdist);
            myPane.XAxis.Scale.MinAuto = false;
            myPane.XAxis.Scale.MaxAuto = false;

            // Recalculate Y scale dynamically to fit DEM + Planned altitudes neatly
            zgc.AxisChange();

            // Prevent Y-Axis from dropping below minElevationBase
            if (myPane.YAxis.Scale.Min < minElevationBase)
            {
                myPane.YAxis.Scale.Min = minElevationBase;
                myPane.YAxis.Scale.MinAuto = false;
            }

            zgc.Invalidate();
        }
    }
}