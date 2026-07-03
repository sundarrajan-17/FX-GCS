#if !LIB
// XXX: We need both the System.Drawing.Bitmap from System.Drawing and XagSurveillanceGCS.Drawing
extern alias Drawing;
using MPBitmap = Drawing::System.Drawing.Bitmap;
#else
using MPBitmap = System.Drawing.Bitmap;
#endif

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using XagSurveillanceGCS.Utilities;
using SkiaSharp;
using log4net;
using System.Collections.Generic;
using static MAVLink;
using XagSurveillanceGCS.GCSViews;
using System.Threading.Tasks;
using XagSurveillanceGCS.ArduPilot.Mavlink;
using GMap.NET.WindowsForms;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace XagSurveillanceGCS.Controls
{
    public partial class GimbalVideoControl : UserControl, IMessageFilter
    {
        // logger
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private GimbalControlSettings preferences = new GimbalControlSettings();

        private readonly GStreamer _stream = new GStreamer();

        private HashSet<Keys> heldKeys = new HashSet<Keys>();
        private HashSet<Keys> boundHoldKeys = new HashSet<Keys>();
        private HashSet<Keys> boundPressKeys = new HashSet<Keys>();

        private Dictionary<Keys, Keys> mod2key = new Dictionary<Keys, Keys>()
        {
            { Keys.Shift, Keys.ShiftKey },
            { Keys.Control, Keys.ControlKey },
            { Keys.Alt, Keys.Menu }
        };

        private float previousPitchRate = 0;
        private float previousYawRate = 0;
        private float previousZoomRate = 0;
        private bool yaw_lock = false;
        public int counter = 0;

        private CameraProtocol _selectedCamera;
        private CameraProtocol selectedCamera
        {
            get
            {
                return _selectedCamera ?? MainV2.comPort?.MAV?.Camera;
            }
            set
            {
                _selectedCamera = value;
            }
        }

        private bool isRecording
        {
            // TODO: ArduPilot hard-codes this to 0 presently, so we can't check it
            /*get
            {
                return selectedCamera?.CameraCaptureStatus.video_status > 0;
            }*/

            // So for now, we will manually track it
            // TODO: Remove this once ArduPilot is fixed
            get;
            set;
        }

        private GimbalManagerProtocol _selectedGimbalManager;
        private GimbalManagerProtocol selectedGimbalManager
        {
            get
            {
                return _selectedGimbalManager ?? MainV2.comPort?.MAV?.GimbalManager;
            }
            set
            {
                _selectedGimbalManager = value;
            }
        }

        // The selected gimbal ID for the currently-selected gimbal manager
        // (0 means all gimbals)
        private byte selectedGimbalID = 0;

        private GMapOverlay mouseMapMarker;

        private readonly System.Timers.Timer AutoConnectTimer;

        private FlightData flightData;

        private BaseCameraController selectedCameraController;

        private String selectedCamera1 = "";

        private bool isVideoPanelVisible = true;
        public GimbalVideoControl(FlightData flightdata)
        {
            InitializeComponent();

            this.flightData = flightdata;
            loadPreferences();


            yaw_lock = preferences.DefaultLockedMode;

            // Register the global key handler
            Application.AddMessageFilter(this);

            mouseMapMarker = flightdata.mouseMapMarker1;

            selectedCameraController = flightData._baseCameraController;


            this.selectedCamera1 = selectedCameraController.SelectedCamera;

            if (!initializeGStreamer())
            {
                // No point in doing anything else if GStreamer isn't available
                return;
            }

            _stream.OnNewImage += RenderFrame;

            videoStreamToolStripMenuItem_Click(null,null);
        }

        private bool initializeGStreamer()
        {
            GStreamer.GstLaunch = GStreamer.LookForGstreamer();

            if (!GStreamer.GstLaunchExists)
            {
                var result = CustomMessageBox.Show(
                    "This feature requires GStreamer. Would you like to download and install it now?",
                    "GStreamer not found",
                    MessageBoxButtons.YesNo,
                    CustomMessageBox.MessageBoxIcon.Question
                );
                if (result != (int)DialogResult.Yes)
                {
                    return false;
                }
                GStreamerUI.DownloadGStreamer();
                // Check success
                if (!GStreamer.GstLaunchExists)
                {
                    var message = "GStreamer was not found after installation. Please install it manually.";
                    if (GStreamer.NativeMethods.Backend == GStreamer.NativeMethods.BackendEnum.Windows)
                    {
                        message += "\n\nFor Windows, install the MinGW version from https://gstreamer.freedesktop.org/download/#windows";
                    }
                    CustomMessageBox.Show(
                        message,
                        "GStreamer not found",
                        MessageBoxButtons.OK,
                        CustomMessageBox.MessageBoxIcon.Error
                    );
                    return false;
                }
            }
            return true;
        }

        private void loadPreferences()
        {
            preferences = new GimbalControlSettings();
            var json = Settings.Instance["GimbalControlPreferences", ""];
            if (json != "")
            {
                try
                {
                    preferences = Newtonsoft.Json.JsonConvert.DeserializeObject<GimbalControlSettings>(json);
                }
                catch (Exception ex)
                {
                    log.Error("Invalid GimbalControlPreferences, reverting to default", ex);
                }
            }

            // Populate the list of keys that are expected to be pressed
            boundPressKeys.Clear();
            boundPressKeys.Add(preferences.TakePicture);
            boundPressKeys.Add(preferences.ToggleRecording);
            boundPressKeys.Add(preferences.StartRecording);
            boundPressKeys.Add(preferences.StopRecording);
            boundPressKeys.Add(preferences.ToggleLockFollow);
            boundPressKeys.Add(preferences.SetLock);
            boundPressKeys.Add(preferences.SetFollow);
            boundPressKeys.Add(preferences.Retract);
            boundPressKeys.Add(preferences.Neutral);
            boundPressKeys.Add(preferences.PointDown);
            boundPressKeys.Add(preferences.Home);

            // Populate the list of keys that are expected to be held down
            boundHoldKeys.Clear();
            boundHoldKeys.Add(preferences.SlewLeft);
            boundHoldKeys.Add(preferences.SlewRight);
            boundHoldKeys.Add(preferences.SlewUp);
            boundHoldKeys.Add(preferences.SlewDown);
            boundHoldKeys.Add(preferences.ZoomIn);
            boundHoldKeys.Add(preferences.ZoomOut);
            // Add relevant modifier keys
            boundHoldKeys.Add(mod2key[preferences.SlewFastModifier]);
            boundHoldKeys.Add(mod2key[preferences.SlewSlowModifier]);
        }

        private void RenderFrame(object sender, MPBitmap image)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => RenderFrame(sender, image)));
                return;
            }
            try
            {
                if (image == null || image.Width <= 0 || image.Height <= 0)
                {
                    VideoBox.Image?.Dispose();
                    VideoBox.Image = null;
                    VideoBox.Image = global::XagSurveillanceGCS.Properties.Resources.no_video;
                    return;
                }

                var old = VideoBox.Image;
                VideoBox.Image = new Bitmap(
                    image.Width, image.Height, 4 * image.Width,
                    PixelFormat.Format32bppPArgb,
                    image.LockBits(Rectangle.Empty, null, SKColorType.Bgra8888).Scan0
                );
                
                // Overlay tracking info
                var tracking_status = selectedCamera?.CameraTrackingImageStatus ?? new mavlink_camera_tracking_image_status_t();
                if (dragStartPoint.HasValue && dragEndPoint.HasValue)
                {
                    var start = dragStartPoint.Value;
                    var end = dragEndPoint.Value;
                    using (var g = Graphics.FromImage(VideoBox.Image))
                    {
                        var x = (float)(Math.Min(start.x, end.x) + 1) * VideoBox.Image.Width / 2;
                        var y = (float)(Math.Min(start.y, end.y) + 1) * VideoBox.Image.Height / 2;
                        var w = (float)Math.Abs(start.x - end.x) * VideoBox.Image.Width / 2;
                        var h = (float)Math.Abs(start.y - end.y) * VideoBox.Image.Height / 2;
                        g.DrawRectangle(Pens.Red, x, y, w, h);
                    }
                }
                else if (tracking_status.tracking_status == (byte)MAVLink.CAMERA_TRACKING_STATUS_FLAGS.ACTIVE &&
                    (tracking_status.target_data & (byte)MAVLink.CAMERA_TRACKING_TARGET_DATA.RENDERED) == 0 && // Don't render if the target is already rendered
                    (tracking_status.target_data & (byte)MAVLink.CAMERA_TRACKING_TARGET_DATA.IN_STATUS) != 0) // Only render if this status message contains the target data
                {
                    if (tracking_status.tracking_mode == (byte)MAVLink.CAMERA_TRACKING_MODE.POINT &&
                        !float.IsNaN(tracking_status.point_x) &&
                        !float.IsNaN(tracking_status.point_y))
                    {
                        var x = tracking_status.point_x * VideoBox.Image.Width;
                        var y = tracking_status.point_y * VideoBox.Image.Height;
                        var size = float.IsNaN(tracking_status.radius) ? 10 : tracking_status.radius;
                        using (var g = Graphics.FromImage(VideoBox.Image))
                        {
                            g.DrawEllipse(Pens.Red, (int)x - size / 2, (int)y - size / 2, size, size);
                        }
                    }
                    else if (tracking_status.tracking_mode == (byte)MAVLink.CAMERA_TRACKING_MODE.RECTANGLE &&
                        !float.IsNaN(tracking_status.rec_top_x) &&
                        !float.IsNaN(tracking_status.rec_top_y) &&
                        !float.IsNaN(tracking_status.rec_bottom_x) &&
                        !float.IsNaN(tracking_status.rec_bottom_y))
                    {
                        var x = (float)(Math.Min(tracking_status.rec_top_x, tracking_status.rec_bottom_x)) * VideoBox.Image.Width;
                        var y = (float)(Math.Min(tracking_status.rec_top_y, tracking_status.rec_bottom_y)) * VideoBox.Image.Height;
                        var w = (float)Math.Abs(tracking_status.rec_top_x - tracking_status.rec_bottom_x) * VideoBox.Image.Width;
                        var h = (float)Math.Abs(tracking_status.rec_top_y - tracking_status.rec_bottom_y) * VideoBox.Image.Height;
                        using (var g = Graphics.FromImage(VideoBox.Image))
                        {
                            g.DrawRectangle(Pens.Red, x, y, w, h);
                        }
                    }
                }


                old?.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Error rendering frame", ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stream.StopVideo = false;
                Stop();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public void Stop()
        {
            try
            {
                _stream.Stop();
            }
            catch (Exception ex)
            {
                // log.Debug(ex);
                CustomMessageBox.Show("Unable To Stop Stream Try Again", "Video Stream Status");
            }
        }

        private void videoStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                GStreamer.GstLaunch = GStreamer.LookForGstreamer();

                if (!GStreamer.GstLaunchExists)
                {
                    GStreamerUI.DownloadGStreamer();

                    if (!GStreamer.GstLaunchExists)
                    {
                        return;
                    }
                }
                if (this.flightData._baseCameraController.SelectedCamera == "XAGCAM2")
                {  
                    _stream.Start("rtspsrc location=rtsp://192.168.199.119:8554/merge latency=0 dulation=-1 ! decodebin ! videoconvert ! video/x-raw,format=BGRA ! appsink name=outsink");
                }else if(this.flightData._baseCameraController.SelectedCamera == "XAGCAM3")
                {
                    _stream.Start("rtspsrc location=rtsp://192.168.199.240:8554/payload latency=0 dulation=-1 ! decodebin ! videoconvert ! video/x-raw,format=BGRA ! appsink name=outsink");
                }else
                {
                    _stream.Start("rtspsrc location=rtsp://192.168.199.119:554/ latency=0 dulation=-1 ! decodebin ! videoconvert ! video/x-raw,format=BGRA ! appsink name=outsink");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exceptionnnnnnnnnnn {0}",ex);
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            // Don't hog the keyboard when this control doesn't have focus
            if (!(Parent?.ContainsFocus ?? false))
            {
                if(heldKeys.Count > 0)
                {
                    heldKeys.Clear();
                    HandleHeldKeys();
                }
                return false;
            }

            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;
            const int WM_SYSKEYDOWN = 0x0104;
            const int WM_SYSKEYUP = 0x0105;

            if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
            {
                // Don't handle repeated keydown events from holding down a key
                if ((m.LParam.ToInt32() & 0x40000000) != 0)
                {
                    return false;
                }
                return HandleKeyDown((Keys)m.WParam);
            }
            else if (m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP)
            {
                return HandleKeyUp((Keys)m.WParam);
            }

            return false; // Allow the message to continue to the next filter
        }

        private bool HandleKeyDown(Keys key)
        {
            if (boundHoldKeys.Contains(key))
            {
                heldKeys.Add(key);
                HandleHeldKeys();
                return true;
            }
            else if (boundPressKeys.Contains(key | Control.ModifierKeys))
            {
                HandleKeyPress(key | Control.ModifierKeys);
                return true;
            }
            return false;
        }

        private bool HandleKeyUp(Keys key)
        {
            // Always try to remove the key from the list of pressed keys, even if not bound, just in case
            heldKeys.Remove(key);
            if (boundHoldKeys.Contains(key))
            {
                HandleHeldKeys();
            }
            return boundHoldKeys.Contains(key);
        }

        private void HandleHeldKeys()
        {
            float pitch = 0;
            float yaw = 0;
            if (heldKeys.Contains(preferences.SlewDown))
            {
                flightData.GremsyPitchYawControl(-1,0);
                pitch -= 1;
            }
            if (heldKeys.Contains(preferences.SlewUp))
            {
                flightData.GremsyPitchYawControl(1,0);
                pitch += 1;
            }
            if (heldKeys.Contains(preferences.SlewLeft))
            {
                flightData.GremsyPitchYawControl(0,-1);
                yaw -= 1;
            }
            if (heldKeys.Contains(preferences.SlewRight))
            {
                flightData.GremsyPitchYawControl(0,1);
                yaw += 1;
            }

            float speed = (float)preferences.SlewSpeedNormal;
            if (Control.ModifierKeys == preferences.SlewFastModifier)
            {
                flightData.GremsyZoomStop();
                speed = (float)preferences.SlewSpeedFast;
            }
            else if (Control.ModifierKeys == preferences.SlewSlowModifier)
            {
                speed = (float)preferences.SlewSpeedSlow;
            }

            pitch *= speed;
            yaw *= speed;

            if (pitch != previousPitchRate || yaw != previousYawRate)
            {
                previousPitchRate = pitch;
                previousYawRate = yaw;
                selectedGimbalManager?.SetRatesCommandAsync(pitch, yaw, yaw_lock, selectedGimbalID);
                Console.WriteLine($"Pitch: {pitch}, Yaw: {yaw}");
            }

            float zoom = 0;
            if (heldKeys.Contains(preferences.ZoomIn))
            {
                flightData.GremsyZoomIn();
                zoom += 1;
            }
            if (heldKeys.Contains(preferences.ZoomOut))
            {
                flightData.GremsyZoomOut();
                zoom -= 1;
            }

            zoom *= (float)preferences.ZoomSpeed;

            if (zoom != previousZoomRate)
            {
                previousZoomRate = zoom;
                selectedCamera?.SetZoomAsync(zoom, CAMERA_ZOOM_TYPE.ZOOM_TYPE_CONTINUOUS);
                // Console.WriteLine($"Zoom: {zoom}");
            }
        }

        private void HandleKeyPress(Keys key)
        {
            if (key == preferences.TakePicture)
            {
                TakePicture();
            }
            if (key == preferences.ToggleRecording)
            {
                SetRecording(!isRecording);
            }
            if (key == preferences.StartRecording)
            {
                SetRecording(true);
            }
            if (key == preferences.StopRecording)
            {
                SetRecording(false);
            }
            if (key == preferences.ToggleLockFollow)
            {
                SetYawLock(!yaw_lock);
            }
            if (key == preferences.SetLock)
            {
                SetYawLock(true);
            }
            if (key == preferences.SetFollow)
            {
                SetYawLock(false);
            }
            if (key == preferences.Retract)
            {
                Retract();
            }
            if (key == preferences.Neutral)
            {
                Neutral();
            }
            if (key == preferences.PointDown)
            {
                PointDown();
            }
            if (key == preferences.Home)
            {
                Home();
            }
        }

        private void TakePicture()
        {
            selectedCamera?.TakeSinglePictureAsync();
        }

        private void SetRecording(bool start)
        {
            isRecording = start;
            if(start)
            {
                selectedCamera?.StartRecordingAsync();
            }
            else
            {
                selectedCamera?.StopRecordingAsync();
            }
        }

        private void SetYawLock(bool locked)
        {
            string message = locked ? "lock" : "follow";
            yaw_lock = locked;
            yawLockToolStripMenuItem.Checked = locked;
            if(this.flightData._baseCameraController.SelectedCamera == "XAGCAM2")
            {
                // flightData.GremsyHomeCommand();
            }
            else
            {
                flightData.XagCamSetLockFollowCommand(message);
            }
        }

        private void Retract()
        {
            flightData.XagCamStopTrackCommand();
        }

        private void Neutral()
        {
            selectedGimbalManager?.NeutralAsync();
        }

        private void PointDown()
        {
            if(this.flightData._baseCameraController.SelectedCamera == "XAGCAM2")
            {
                // flightData.GremsyPointDownCommand();
            }
            else
            {
                flightData.XagCamPointDownCommand();
            }
        }

        private void Home()
        {
            if(this.flightData._baseCameraController.SelectedCamera == "XAGCAM2")
            {
                flightData.GremsyHomeCommand();
            }
            else
            {
                flightData.XagCamHomeCommand();
            }

        }

    
        private DateTime lastMouseMove = DateTime.MinValue;
        private (double x, double y)? dragStartPoint = null;
        private (double x, double y)? dragEndPoint = null;
        private void VideoBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(DateTime.Now > lastMouseMove.AddMilliseconds(100))
            {
                lastMouseMove = DateTime.Now;
                mouseMapMarker.Clear();


                var point = getMousePosition(e.X, e.Y);
                if(!point.HasValue) { return; }
                var latlon = selectedCamera?.CalculateImagePointLocation(point.Value.x, point.Value.y);
                if (latlon != null)
                {
                    mouseMapMarker.Markers.Add(
                        new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                            new GMap.NET.PointLatLng(latlon.Lat, latlon.Lng),
                            GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_small
                        )
                    );
                }
            }

            if ((Control.ModifierKeys, e.Button) == preferences.TrackObjectUnderMouse)
            {
                if(dragStartPoint == null)
                {
                    dragStartPoint = getMousePosition(e.X, e.Y);
                }
                dragEndPoint = getMousePosition(e.X, e.Y);
                Console.WriteLine($"Drag start: {dragStartPoint?.x}, {dragStartPoint?.y} - Drag end: {dragEndPoint?.x}, {dragEndPoint?.y}");
                float x01 = ((float)dragStartPoint?.x + 1f) / 2f;
                float y01 = ((float)dragStartPoint?.y + 1f) / 2f;
                float x02 = ((float)dragEndPoint?.x + 1f) / 2f;
                float y02 = ((float)dragEndPoint?.y + 1f) / 2f;
                // flightData.GremsyRectangleTracking(x01, y01, x02, y02);
            }
            else
            {
                if(dragStartPoint != null && dragEndPoint != null)
                {   
                    float x01 = ((float)dragStartPoint?.x + 1f) / 2f;
                    float y01 = ((float)dragStartPoint?.y + 1f) / 2f;
                    float x02 = ((float)dragEndPoint?.x + 1f) / 2f;
                    float y02 = ((float)dragEndPoint?.y + 1f) / 2f;
                    flightData.GremsyRectangleTracking(x01, y01, x02, y02);
                }
                dragStartPoint = null;
                dragEndPoint = null;
            }
        }

        private void VideoBox_MouseLeave(object sender, EventArgs e)
        {
            if(dragStartPoint != null && dragEndPoint != null)
            {   
                float x01 = ((float)dragStartPoint?.x + 1f) / 2f;
                float y01 = ((float)dragStartPoint?.y + 1f) / 2f;
                float x02 = ((float)dragEndPoint?.x + 1f) / 2f;
                float y02 = ((float)dragEndPoint?.y + 1f) / 2f;
                flightData.GremsyRectangleTracking(x01, y01, x02, y02);
            }
            mouseMapMarker.Markers.Clear();
            dragStartPoint = null;
            dragEndPoint = null;
        }

         public void SendNormalizedTrackingPoint(double xNorm, double yNorm, int width, int height)
        {
            int x = (int)(xNorm * (width));
            int y = (int)(yNorm * (height));

            if(this.flightData._baseCameraController.SelectedCamera == "XAGCAM2")
            {
                float x01 = ((float)xNorm + 1f) / 2f;
                float y01 = ((float)yNorm + 1f) / 2f;

                Console.WriteLine("Converted X Y Point {0} {1}", x01, y01);
                flightData.GremsyStartTracking(x01, y01);
            }else
            {
                int x01 = (int)(xNorm * (width / 2));
                int y01 = (int)(yNorm * (height / 2));
                flightData.BuildTrackingCommand(x01, y01);
            }
        }

        private void VideoBox_Click(object sender, EventArgs e)
        {
            // Focus the control when clicked
            VideoBox.Focus();

            MouseEventArgs me = (MouseEventArgs)e;
            var point = getMousePosition(me.X, me.Y);
            if (!point.HasValue)
            {
                return;
            }

            // Check the key/button combination to determine the action
            if ((Control.ModifierKeys, me.Button) == preferences.MoveCameraToMouseLocation)
            {
                var imageWidth = VideoBox.Image.Width;
                var imageHeight = VideoBox.Image.Height;
                SendNormalizedTrackingPoint(point.Value.x, point.Value.y, 1920, 1080);
               
            }
            else if ((Control.ModifierKeys, me.Button) == preferences.MoveCameraPOIToMouseLocation)
            {
                var loc = selectedCamera?.CalculateImagePointLocation(point.Value.x, point.Value.y);
                if (loc == null)
                {
                    return;
                }
                selectedGimbalManager?.SetROILocationAsync(loc.Lat, loc.Lng, loc.Alt, frame: MAV_FRAME.GLOBAL);
            }
            else if ((Control.ModifierKeys, me.Button) == preferences.TrackObjectUnderMouse)
            {
                selectedCamera?.RequestTrackingMessageInterval(5);
                var x = (float)point.Value.x;
                var y = (float)point.Value.y;
                if (dragStartPoint.HasValue)
                {
                    var start_x = (float)dragStartPoint.Value.x;
                    var start_y = (float)dragStartPoint.Value.y;
                    selectedCamera?.SetTrackingRectangleAsync(
                        start_x, start_y, x, y
                    );
                }
                else
                {
                    selectedCamera?.SetTrackingPointAsync(x, y);
                }
            }
        }

        private (double x, double y)? getMousePosition(int x, int y)
        {
            if (VideoBox.Image == null)
            {
                return null;
            }

            // Find the point within the image inside VideoBox, not just the VideoBox
            var imageWidth = Math.Min(VideoBox.Width, VideoBox.Height * VideoBox.Image.Width / VideoBox.Image.Height);
            var imageHeight = Math.Min(VideoBox.Height, VideoBox.Width * VideoBox.Image.Height / VideoBox.Image.Width);
            if (imageWidth < VideoBox.Width)
            {
                x -= (VideoBox.Width - imageWidth) / 2;
                x *= VideoBox.Width / imageWidth;
                x = Math.Max(0, Math.Min(imageWidth, x));
            }
            if (imageHeight < VideoBox.Height)
            {
                y -= (VideoBox.Height - imageHeight) / 2;
                y *= VideoBox.Height / imageHeight;
                y = Math.Max(0, Math.Min(imageHeight, y));
            }

            return (2 * x / (double)imageWidth - 1, 2 * y / (double)imageHeight - 1);
        }

        /// <summary>
        /// Update the camera controls based on the current camera capabilities
        /// </summary>
        private void UITimer_Tick(object sender, EventArgs e)
        {
            takePictureToolStripMenuItem.Enabled = selectedCamera?.CanCaptureImage ?? false;
            startRecordingToolStripMenuItem.Enabled = selectedCamera?.CanCaptureVideo ?? false;
            stopRecordingToolStripMenuItem.Enabled = selectedCamera?.CanCaptureVideo ?? false;

            if (selectedCamera != null)
            {
                selectedCamera.HFOV = (float)preferences.CameraHFOV;
                selectedCamera.VFOV = (float)preferences.CameraVFOV;
                selectedCamera.UseFOVStatus = preferences.UseFOVReportedByCamera;
            }
        }

        private void yawLockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item == null) { return; }
            SetYawLock(!item.Checked);
        }

        private void retractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flightData.GimbalConnectStartThread();
        }

        private void neutralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Neutral();
        }

        private void pointDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointDown();
        }

        private void pointHomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Home();
        }

        private void takePictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TakePicture();
        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetRecording(true);
        }

        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetRecording(false);
        }

        private void VideoBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.flightData.GremsyStopTracking();
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new GimbalControlSettingsForm(preferences);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Settings.Instance["GimbalControlPreferences"] = Newtonsoft.Json.JsonConvert.SerializeObject(form.preferences);
                loadPreferences();
            }
        }

        private void closeStreamToolStripMenuItem_Click(object sender,EventArgs e)
        {
            Stop();
        }

        private void AutoConnectTimerCallback(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (CameraProtocol.VideoStreams.Count < 1)
            {
                Console.Write("Requesting camera information...");
                // We must not have any reported video streams. Try to request them
                selectedCamera?.RequestCameraInformationAsync().Wait();
                // Come back later and see if any streams have been reported
                AutoConnectTimer.Start();
                return;
            }


            string previous_stream = Settings.Instance["gimbal_video_stream", ""];
            // See if any of the streams are the last one used
            foreach (var stream in CameraProtocol.VideoStreams.Values)
            {
                if (System.Text.Encoding.UTF8.GetString(stream.uri).Split('\0')[0] == previous_stream)
                {
                    _stream.Start(CameraProtocol.GStreamerPipeline(stream));
                    return;
                }
            }

            // If not, just use the first one
            var first_stream = CameraProtocol.VideoStreams.First().Value;
            Settings.Instance["gimbal_video_stream"] = System.Text.Encoding.UTF8.GetString(first_stream.uri).Split('\0')[0];
            _stream.Start(CameraProtocol.GStreamerPipeline(first_stream));
            return;
        }
    }
}
