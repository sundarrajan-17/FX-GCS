using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace XagSurveillanceGCS.Controls
{   
    public class CameraSettingsForm : Form
    {
        private Panel scrollPanel;
        private TableLayoutPanel table;
        private GremsyControl _parentControl;

        private bool _initializing = true;

        private readonly Dictionary<string, Action<string, int>> comboHandlers =
            new Dictionary<string, Action<string, int>>();

        private readonly Dictionary<string, Action<bool>> toggleHandlers =
            new Dictionary<string, Action<bool>>();

        private readonly Dictionary<string, ComboBox> comboControls =
            new Dictionary<string, ComboBox>();

        private readonly Dictionary<string, CheckBox> toggleControls =
            new Dictionary<string, CheckBox>();

        private readonly Dictionary<string, int> rtspMap =
            new Dictionary<string, int>
            {
                { "720p", 0 },
                { "1080p", 1 }
            };

        private readonly Dictionary<string, int> zoomMap =
            new Dictionary<string, int>
            {
                { "Normal", 2 },
                { "Super Resolution", 4 }
            };

        private readonly Dictionary<string, int> videoQualityMap =
            new Dictionary<string, int>
            {
                { "Default", 20 },
                { "Medium", 30 },
                { "High", 40 }
            };
        // Dictionary<string, Control> settingsControls = new Dictionary<string, Control>();


        public CameraSettingsForm(GremsyControl parentControl)
        {
            this._parentControl = parentControl;
            Text = "Settings";
            Size = new Size(360, 600);
            BackColor = Color.FromArgb(40, 40, 40);

            scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            table = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(10),
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

            scrollPanel.Controls.Add(table);
            Controls.Add(scrollPanel);
            Console.WriteLine("Initializing Camera Settings Form...",this._parentControl.cameraSettings);

            // ---- ADD SETTINGS ----
            AddCombo("Camera", new[] { "R1", "R2", "R3" });
            AddCombo("Camera Mode", new[] {"Photo","Video"});
            AddCombo("Camera EV", new[] {"-4.5","-3", "-1.5", "0", "+1.5", "+3","4.5"});
            AddCombo("White Balance", new[] { "Auto", "Indoor", "Outdoor", "OnePushWB", "ATW", "Manual", "OutdoorAuto", "SodiumLampAuto", "SodiumLamp", "SodiumLampOutdoorAuto"});
            AddToggle("High Sensitivity", (bool)this._parentControl.cameraSettings["EO_HS"] == false ? false : true);
            AddCombo("Video Quality", new[] { "Default", "Medium", "High" });
            AddCombo("RTSP Resolution", new[] { "720p", "1080p" });
            // AddSlider("RTSP Bitrate", 0, 100, 50);
            AddCombo("Zoom Feature", new[] { "Normal", "Super Resolution" });
            AddToggle("IR Thermometry", (bool)this._parentControl.cameraSettings["IR_THERMOMETRY"] == false ? false : true);
            AddCombo("Infrared Palette", new[] { "White Hot","Sepia","Ironbow","Rainbow","Night","Aurora","Red Hot","Jungle","Medical","Black Hot","Glory Hot" });
            // AddSlider("Gimbal Speed", 0, 100, 40);
            AddCombo("Yaw Mode", new[] { "Head", "Global" });
            AddCombo("Track Algorithm", new[] { "None", "OSTrack", "Nano", "SiamRPN" });
            AddToggle("Track AutoZoom", (bool)_parentControl.cameraSettings["TRACK_AUTOZOOM"]);
            AddCombo("Smart Select",new[] {"None","Yolo11","Yolo26","Yolov8"});
            AddCombo("AI Resolution", new[] {"640X480","1280X720","1920X1080"});
            AddToggle("AI OSD", (bool)this._parentControl.cameraSettings["AI_OSD"] == false ? false : true);
            AddToggle("RangeFinder", (bool)this._parentControl.cameraSettings["TOF_EN"] == false ? false : true);
            AddCombo("Merge Display Mode",new[] {"IR in EO","EO in IR","EO","IR"});
            AddCombo("Track Mode",new[] {"Angle Control", "Velocity Control"});
            if((bool)this._parentControl.cameraSettings["AI_OSD"] == false)
            {
                AddCombo("AI Video Source", new[] { "EO", "IR" });
            }
            // AddButton("Apply Settings");
            AddButton("Reset Camera Defaults");

            RegisterHandlers();
            _initializing = false;
        }

        private void RefreshUI()
        {
            table.Controls.Clear();
            table.RowStyles.Clear();
            table.RowCount = 0;

            AddCombo("Camera", new[] { "R1", "R2", "R3" });
            AddCombo("Camera Mode", new[] {"Photo","Video"});
            AddCombo("Camera EV", new[] {"-4.5","-3", "-1.5", "0", "+1.5", "+3","4.5"});
            AddCombo("White Balance", new[] { "Auto", "Indoor", "Outdoor", "OnePushWB", "ATW", "Manual", "OutdoorAuto", "SodiumLampAuto", "SodiumLamp", "SodiumLampOutdoorAuto" });
            AddToggle("High Sensitivity", (bool)_parentControl.cameraSettings["EO_HS"]);
            AddCombo("Video Quality", new[] { "Default", "Medium", "High" });
            AddCombo("RTSP Resolution", new[] { "720p", "1080p" });
            AddCombo("Zoom Feature", new[] { "Normal", "Super Resolution" });
            AddToggle("IR Thermometry", (bool)_parentControl.cameraSettings["IR_THERMOMETRY"]);
            AddCombo("Infrared Palette", new[] { "White Hot","Sepia","Ironbow","Rainbow","Night","Aurora","Red Hot","Jungle","Medical","Black Hot","Glory Hot" });
            AddCombo("Yaw Mode", new[] { "Head", "Global" });
            AddCombo("Track Algorithm", new[] { "None", "OSTrack", "Nano", "SiamRPN" });
            AddToggle("Track AutoZoom", (bool)_parentControl.cameraSettings["TRACK_AUTOZOOM"]);
            AddCombo("Smart Select", new[] {"None","Yolo11","Yolo26","Yolov8"});
            AddCombo("AI Resolution", new[] {"640X480","1280X720","1920X1080"});
            AddToggle("AI OSD", (bool)_parentControl.cameraSettings["AI_OSD"]);
            AddToggle("RangeFinder", (bool)_parentControl.cameraSettings["TOF_EN"]);
            AddCombo("Merge Display Mode",new[] {"IR in EO","EO in IR","EO","IR"});
            AddCombo("Track Mode",new[] {"Angle Control", "Velocity Control"});
            if (!(bool)_parentControl.cameraSettings["AI_OSD"])
            {
                AddCombo("AI Video Source", new[] { "EO", "IR" });
            }

            AddButton("Reset Camera Defaults");
            RegisterHandlers();
            _initializing = false;
        }

        // private void ApplyAIVideoSource(string source)
        // {
        //     Console.WriteLine($"Applying AI Video Source: {source}");
        //     // Example: call your Gremsy / Viewpro / MAVLink command to set video source
        //     // _parentControl.SetAIVideoSource(source);
        //     if(source == "EO")
        //     {
        //         source = "eo";
        //     }
        //     else if(source == "IR")
        //     {
        //         source = "ir";
        //     }
        //     string text = "AI_SOURCE";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] paramValue = new byte[128];
        //     byte[] valueBytes = Encoding.ASCII.GetBytes(source);
        //     Array.Copy(valueBytes, paramValue, Math.Min(valueBytes.Length, paramValue.Length));

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = paramValue;
        //     msg.param_type = 11;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }
        // private void ApplyEVValue(int EvValue)
        // {
        //     Console.WriteLine($"Applying EV Value: {EvValue}");
        //     int value = 10 - EvValue;
        //     string text = "EO_EV";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes = BitConverter.GetBytes(value);

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // } 

        // private void ApplyWhiteBalance(int value)
        // {
        //     Console.WriteLine($"Applying White Balance: {value}");
            
        //     string text = "EO_WB";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes = BitConverter.GetBytes(value);

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }
        private void ApplyCameraSelection(string camera)
        {
            Console.WriteLine($"Applying Camera Selection: {camera}");
            if(camera == "R3")
            {
                var msg = new MAVLink.mavlink_param_ext_request_list_t();
                msg.target_system = (byte)MainV2.comPort.sysidcurrent;
                msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
                MainV2.comPort.sendPacket(
                    msg,
                    (byte)MainV2.comPort.sysidcurrent,
                    (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
                );
                RefreshUI();
            }
        }
        // private void ApplyVideoQuality(string quality)
        // {
        //     Console.WriteLine($"Applying Video Quality: {quality}");
        //     int value =20;
        //     if(quality == "Low")
        //     {
        //         value = 20;
        //     }
        //     else if(quality == "Medium")
        //     {
        //         value = 30;
        //     }
        //     else if(quality == "High")
        //     {
        //         value = 40;
        //     }
        //     string text = "EO_VIDEO_QUALITY";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes = BitConverter.GetBytes(value);

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }

        // private void ApplyRTSPResolution(string resolution)
        // {
        //     Console.WriteLine($"Applying RTSP Resolution: {resolution}");
        //     int value = 0;
        //     if(resolution == "720p")
        //     {
        //         value = 0;
        //     }
        //     else if(resolution == "1080p")
        //     {
        //         value = 1;
        //     }
        //     string text = "EO_RESOLUTION";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes =BitConverter.GetBytes(value);

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }
        // private void ApplyZoomFeature(string zoomMode)
        // {
        //     Console.WriteLine($"Applying Zoom Feature: {zoomMode}");
        //     int value = 2;
        //     if(zoomMode == "Normal")
        //     {
        //         value = 2;
        //     }
        //     else if(zoomMode == "Super Resolution")
        //     {
        //         value = 4;
        //     }
        //     string text = "EO_ZOOM_MODE";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes = BitConverter.GetBytes(value);

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }
        // private void ApplyInfraredPalette(int palette)
        // {
        //     Console.WriteLine($"Applying Infrared Palette: {palette}");
        //     string text = "IR_PALETTE";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes = BitConverter.GetBytes(palette);
        //     var msg = new MAVLink.mavlink_param_ext_set_t();

        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }

        // private void ApplyCheckBoxChangeCommon(string settingName, bool value)
        // {
        //     Console.WriteLine($"{settingName} toggled → {(value ? "On" : "Off")}");
        //     // Here you can add any common logic that should happen whenever a checkbox changes
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(settingName);
        //     byte[] value_bytes = BitConverter.GetBytes(value ? 1 : 0);
        //     var msg = new MAVLink.mavlink_param_ext_set_t();

        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 1;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }

        // private void ApplyTrackAlgorithm(string algorithm)
        // {
        //     Console.WriteLine($"Applying Track Algorithm: {algorithm}");
        //     string text = "TRACK_ALGORITHM";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] paramValue = new byte[128];
        //     byte[] valueBytes = Encoding.ASCII.GetBytes(algorithm);
        //     Array.Copy(valueBytes, paramValue, Math.Min(valueBytes.Length, paramValue.Length));

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = paramValue;
        //     msg.param_type = 11;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }

        // private void ApplySmartSelect(string option)
        // {
        //     Console.WriteLine($"Applying Smart Select: {option}");
        //     string text = "SMART_SELECT";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] paramValue = new byte[128];
        //     byte[] valueBytes = Encoding.ASCII.GetBytes(option);
        //     Array.Copy(valueBytes, paramValue, Math.Min(valueBytes.Length, paramValue.Length));

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = paramValue;
        //     msg.param_type = 11;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }

        // private void ApplyYawMode(int yawmode)
        // {
        //     string text = "YAW_MODE";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes = BitConverter.GetBytes(yawmode);

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }

        // private void ApplyAIResolution(int resolution)
        // {
        //     string text = "AI_RESOLUTION";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes = BitConverter.GetBytes(resolution);

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }

        // private void ApplyMergeVideo(int merge_video)
        // {
        //     string text = "MERGE_DISPLAY";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes = BitConverter.GetBytes(merge_video);

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }

        // private void ApplyTrackMode(int track_mode)
        // {
        //     string text = "TRACK_MODE";
        //     byte[] id_bytes = Encoding.ASCII.GetBytes(text);
        //     byte[] value_bytes = BitConverter.GetBytes(track_mode);

        //     var msg = new MAVLink.mavlink_param_ext_set_t();
        //     msg.target_system = (byte)MainV2.comPort.sysidcurrent;
        //     msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
        //     msg.param_id = id_bytes;
        //     msg.param_value = value_bytes;
        //     msg.param_type = 6;

        //     MainV2.comPort.sendPacket(
        //         msg,
        //         (byte)MainV2.comPort.sysidcurrent,
        //         (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
        //     );
        // }

        private void ApplyCameraMode(int camera_mode)
        {
            if(camera_mode == 0)
            {
                this._parentControl._parentController._flightData.GremsySwitchCameraModeToPhoto();
            }
            else if(camera_mode == 1)
            {
                this._parentControl._parentController._flightData.GremsySwitchCameraModeToVideo();
            }
        }

        private void SendCameraParam(string paramId, int value)
        {
            byte[] paramValue = new byte[128];
            Array.Copy(BitConverter.GetBytes(value), paramValue, 4);

            var msg = new MAVLink.mavlink_param_ext_set_t
            {
                target_system = (byte)MainV2.comPort.sysidcurrent,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA,
                param_id = Encoding.ASCII.GetBytes(paramId),
                param_value = paramValue,
                param_type = (byte)MAVLink.MAV_PARAM_EXT_TYPE.INT32
            };

            MainV2.comPort.sendPacket(msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA);

            _parentControl.cameraSettings[paramId] = value;
        }

        private void SendCameraParam(string paramId, bool value)
        {
            byte[] paramValue = new byte[128];
            paramValue[0] = (byte)(value ? 1 : 0);

            var msg = new MAVLink.mavlink_param_ext_set_t
            {
                target_system = (byte)MainV2.comPort.sysidcurrent,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA,
                param_id = Encoding.ASCII.GetBytes(paramId),
                param_value = paramValue,
                param_type = (byte)MAVLink.MAV_PARAM_EXT_TYPE.UINT8
            };

            MainV2.comPort.sendPacket(msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA);

            _parentControl.cameraSettings[paramId] = value;
        }

        private void SendCameraParam(string paramId, string value)
        {
            byte[] paramValue = new byte[128];
            byte[] valueBytes = Encoding.ASCII.GetBytes(value);
            Array.Copy(valueBytes, paramValue, Math.Min(valueBytes.Length, 128));

            var msg = new MAVLink.mavlink_param_ext_set_t
            {
                target_system = (byte)MainV2.comPort.sysidcurrent,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA,
                param_id = Encoding.ASCII.GetBytes(paramId),
                param_value = paramValue,
                param_type = (byte)MAVLink.MAV_PARAM_EXT_TYPE.CUSTOM
            };

            MainV2.comPort.sendPacket(msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA);

            _parentControl.cameraSettings[paramId] = value;
        }

        private void ApplyCheckBoxChangeCommon(string settingName, bool value)
        {
            Console.WriteLine($"{settingName} -> {(value ? "On" : "Off")}");
            SendCameraParam(settingName, value);
        }

        // ===================== REPLACE THESE Apply METHODS =====================

        private void ApplyWhiteBalance(int value) => SendCameraParam("EO_WB", value);

        private void ApplyRTSPResolution(string resolution) => SendCameraParam("EO_RESOLUTION", rtspMap[resolution]);

        private void ApplyZoomFeature(string zoomMode) => SendCameraParam("EO_ZOOM_MODE", zoomMap[zoomMode]);

        private void ApplyInfraredPalette(int palette) => SendCameraParam("IR_PALETTE", palette);

        private void ApplyYawMode(int yawmode) => SendCameraParam("YAW_MODE", yawmode);

        private void ApplyAIResolution(int resolution) => SendCameraParam("AI_RESOLUTION", resolution);

        private void ApplyMergeVideo(int merge_video) => SendCameraParam("MERGE_DISPLAY", merge_video);

        private void ApplyTrackMode(int track_mode) => SendCameraParam("TRACK_MODE", track_mode);

        private void ApplyTrackAlgorithm(string algorithm) => SendCameraParam("TRACK_ALGORITHM", algorithm);

        private void ApplySmartSelect(string option) => SendCameraParam("SMART_SELECT", option);

        private void ApplyAIVideoSource(string source) => SendCameraParam("AI_SOURCE", source == "EO" ? "eo" : "ir");

        private void ApplyVideoQuality(string quality) => SendCameraParam("EO_VIDEO_QUALITY", videoQualityMap[quality]);

        private void ApplyEVValue(int evIndex) => SendCameraParam("EO_EV", 10 - evIndex);

        private void RegisterHandlers()
        {
            comboHandlers["White Balance"] = (v, i) => ApplyWhiteBalance(i);
            comboHandlers["RTSP Resolution"] = (v, i) => ApplyRTSPResolution(v);
            comboHandlers["Zoom Feature"] = (v, i) => ApplyZoomFeature(v);
            comboHandlers["Infrared Palette"] = (v, i) => ApplyInfraredPalette(i);
            comboHandlers["Yaw Mode"] = (v, i) => ApplyYawMode(i);
            comboHandlers["AI Resolution"] = (v, i) => ApplyAIResolution(i);
            comboHandlers["Merge Display Mode"] = (v, i) => ApplyMergeVideo(i);
            comboHandlers["Track Mode"] = (v, i) => ApplyTrackMode(i + 2);
            comboHandlers["Track Algorithm"] = (v, i) => ApplyTrackAlgorithm(v);
            comboHandlers["Smart Select"] = (v, i) => ApplySmartSelect(v);
            comboHandlers["AI Video Source"] = (v, i) => ApplyAIVideoSource(v);
            comboHandlers["Camera EV"] = (v, i) => ApplyEVValue(i);
            comboHandlers["Video Quality"] = (v, i) => ApplyVideoQuality(v);
            comboHandlers["Camera"] = (v, i) => ApplyCameraSelection(v);
            comboHandlers["Camera Mode"] = (v, i) => ApplyCameraMode(i);

            toggleHandlers["High Sensitivity"] = b => ApplyCheckBoxChangeCommon("EO_HS", b);
            toggleHandlers["IR Thermometry"] = b => ApplyCheckBoxChangeCommon("IR_THERMOMETRY", b);
            toggleHandlers["AI OSD"] = b => ApplyCheckBoxChangeCommon("AI_OSD", b);
            toggleHandlers["RangeFinder"] = b => ApplyCheckBoxChangeCommon("TOF_EN", b);
            toggleHandlers["Track AutoZoom"] = b => ApplyCheckBoxChangeCommon("TRACK_AUTOZOOM", b);
        }



        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;

            string settingName = cmb.Tag.ToString();
            string value = cmb.SelectedItem.ToString();
            int index = cmb.SelectedIndex;

            Console.WriteLine($"{settingName} changed -> {value} (Index {index})");

            if (comboHandlers.TryGetValue(settingName, out var handler))
                handler(value, index);

            // Example: immediate action
            // if (settingName == "AI Video Source")
            // {
            //     ApplyAIVideoSource(value);
            // }else if(settingName == "Camera EV")
            // {
            //     ApplyEVValue(index);
            // }else if(settingName == "Camera")
            // {
            //     ApplyCameraSelection(value);
            // }else if(settingName == "White Balance")
            // {
            //     ApplyWhiteBalance(index);
            // }else if(settingName == "Video Quality")
            // {
            //     ApplyVideoQuality(value);
            // }else if(settingName == "RTSP Resolution")
            // {
            //     ApplyRTSPResolution(value);
            // }else if(settingName == "Zoom Feature")
            // {
            //     ApplyZoomFeature(value);
            // }else if(settingName == "Infrared Palette")
            // {
            //     ApplyInfraredPalette(index);
            // }else if(settingName == "Yaw Mode")
            // {
            //     ApplyYawMode(index);
            // }else if(settingName == "Track Algorithm")
            // {
            //     ApplyTrackAlgorithm(value);
            // }else if(settingName == "Smart Select")
            // {
            //     ApplySmartSelect(value);
            // }else if(settingName == "AI Resolution")
            // {
            //     ApplyAIResolution(index);
            // }else if(settingName == "Merge Display Mode")
            // {
            //     ApplyMergeVideo(index);
            // }else if(settingName == "Track Mode")
            // {
            //     ApplyTrackMode(index+=2);
            // }else if(settingName == "Camera Mode")
            // {
            //     ApplyCameraMode(index);
            // }
        }
        // ---------- Helpers ----------
        private void AddRow(Control left, Control right)
        {
            int row = table.RowCount++;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.Controls.Add(left, 0, row);
            table.Controls.Add(right, 1, row);
        }

        private Label MakeLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 6, 0, 6)
            };
        }

        public void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            string settingName = chk.Tag.ToString();
            bool value = chk.Checked;

            Console.WriteLine($"{settingName} toggled → {(value ? "On" : "Off")}");
            chk.Text = value ? "On" : "Off";

            if (toggleHandlers.TryGetValue(settingName, out var handler))
                handler(value);

            // Example: immediate action
            // if (settingName == "High Sensitivity")
            // {
            //     ApplyCheckBoxChangeCommon("EO_HS",value);
            // }else if(settingName == "IR Thermometry")
            // {
            //     ApplyCheckBoxChangeCommon("IR_THERMOMETRY",value);
            // }else if(settingName == "AI OSD")
            // {
            //     ApplyCheckBoxChangeCommon("AI_OSD",value);
            // }else if(settingName == "RangeFinder")
            // {
            //     ApplyCheckBoxChangeCommon("TOF_EN",value);
            // }else if(settingName == "Track AutoZoom")
            // {
            //     ApplyCheckBoxChangeCommon("TRACK_AUTOZOOM",value);
            // }
        }

        private void AddCombo(string label, string[] items)
        {
            ComboBox cb = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cb.Items.AddRange(items);
            if(label == "Video Quality")
            {
                cb.SelectedIndex = (int)this._parentControl.cameraSettings["EO_VIDEO_QUALITY"] == 20 ? 0 : (int)this._parentControl.cameraSettings["EO_VIDEO_QUALITY"] == 30 ? 1 : 2; // Default to Medium
            }else if(label == "RTSP Resolution")
            {
                cb.SelectedIndex = (int)this._parentControl.cameraSettings["EO_RESOLUTION"] == 0 ? 0 : 1; // Default to 1080p
            }else if(label == "Zoom Feature")
            {
                cb.SelectedIndex = (int)this._parentControl.cameraSettings["EO_ZOOM_MODE"] == 2 ? 0 : 1; // Default to Normal
            }else if(label == "Infrared Palette")
            {
                cb.SelectedIndex = (int)this._parentControl.cameraSettings["IR_PALETTE"]; // Default to Ironbow
            }else if(label == "Yaw Mode")
            {
                cb.SelectedIndex = (int)this._parentControl.cameraSettings["YAW_MODE"]; // Default to Follow
            }else if(label == "AI Video Source")
            {
                cb.SelectedIndex = (string)this._parentControl.cameraSettings["AI_SOURCE"] == "eo" ? 0 : 1; // Default to EO
            }else if(label == "Camera")
            {
                cb.SelectedIndex = 2;
            }else if(label == "Track Algorithm")
            {
                cb.SelectedIndex = (string)this._parentControl.cameraSettings["TRACK_ALGORITHM"] == "None" ? 0 : (string)this._parentControl.cameraSettings["TRACK_ALGORITHM"] == "Nano" ? 1 : 2; // Default to None
            }else if(label == "Smart Select")
            {
                cb.SelectedIndex = (string)this._parentControl.cameraSettings["SMART_SELECT"] == "None" ? 0 : (string)this._parentControl.cameraSettings["SMART_SELECT"] == "Yolov11" ? 1 : (string)this._parentControl.cameraSettings["SMART_SELECT"] == "Yolov8" ? 2 : 3; // Default to None
            }else if(label =="AI Resolution")
            {
                cb.SelectedIndex = (int)this._parentControl.cameraSettings["AI_RESOLUTION"] == 0 ? 0 : (int)this._parentControl.cameraSettings["AI_RESOLUTION"] == 1 ? 1 : 2; // Default to 640X480
            }else if(label == "Camera EV")
            {
                cb.SelectedIndex = (int)this._parentControl.cameraSettings["EO_EV"] == 10 ? 0 : (int)this._parentControl.cameraSettings["EO_EV"] == 9 ? 1 : (int)this._parentControl.cameraSettings["EO_EV"] == 8 ? 2 : (int)this._parentControl.cameraSettings["EO_EV"] == 7 ? 3 : (int)this._parentControl.cameraSettings["EO_EV"] == 6 ? 4 : (int)this._parentControl.cameraSettings["EO_EV"] == 5 ? 5 : 6; // Default to 0
            }else if(label == "Merge Display Mode")
            {
                cb.SelectedIndex = (int)this._parentControl.cameraSettings["MERGE_DISPLAY"];
            }else if(label == "Track Mode")
            {
                cb.SelectedIndex = (int)this._parentControl.cameraSettings["TRACK_MODE"] == 2 ? 0 : 1;
            }else if(label == "Camera Mode")
            {
                cb.SelectedIndex = this._parentControl.current_camera_mode;
            }else
            {
                cb.SelectedIndex = 0;
            }
            // cb.SelectedIndex = 0;
            cb.Tag = label; // Store label for identification in event handler
            comboControls[label] = cb;
            cb.SelectedIndexChanged += Combo_SelectedIndexChanged;
            AddRow(MakeLabel(label), cb);
        }

        private void AddToggle(string label, bool value)
        {
            CheckBox chk = new CheckBox
            {
                Checked = value,
                Dock = DockStyle.Left,
                ForeColor = Color.LightGreen,
                Text = value ? "On" : "Off"
            };
            chk.Tag = label;

            toggleControls[label] = chk;

            chk.CheckedChanged += checkBox_CheckedChanged;

            AddRow(MakeLabel(label), chk);
        }

        private void AddText(string label, string value)
        {
            TextBox tb = new TextBox
            {
                Text = value,
                Dock = DockStyle.Fill
            };

            AddRow(MakeLabel(label), tb);
        }

        private void btnResetSettings_Click(object sender, EventArgs e)
        {
            MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA,
                    MAVLink.MAV_CMD.RESET_CAMERA_SETTINGS, (float)1.0, 0,0 , 0, 0, 0,0,false);
        }

        private void AddButton(string text)
        {
            Button btn = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                Height = 32,
                BackColor = Color.FromArgb(120, 180, 60),
                FlatStyle = FlatStyle.Flat
            };

            btn.FlatAppearance.BorderSize = 0;

            btn.Click+=new System.EventHandler(btnResetSettings_Click);

            AddRow(new Label(), btn);
        }
    }
}