using log4net;
using XagSurveillanceGCS.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace XagSurveillanceGCS.Controls
{
    public partial class GremsyControl : UserControl
    {
        public BaseCameraController _parentController;
        private VirtualJoystick _virtualJoystick;
        private float camera_Storage = 0.0f;
        private bool zoomInActive = false;
        private bool zoomOutActive = false;
        // private int EO_EV = 0;
        // private int EO_WB = 0;
        // private int EO_HS = 0;
        // private int EO_VIDEO_QUALITY = 20;
        // private int EO_RESOLUTION = 0;
        // private float EO_BITRATE = 0.0f;
        // private int EO_ZOOM_MODE = 2;
        // private float EO_DZOOM = 1.0f;
        // private int IR_PALETTE = 0;
        // private float IR_ZOOM = 1.0f;
        // private int AI_RESOLUTION = 0;
        // private int AI_OSD = 0;
        // private string AI_SOURCE = "eo";

        // public Dictionary<string, object> cameraSettings = new Dictionary<string, object>();
        public Dictionary<string, object> cameraSettings = new Dictionary<string, object>()
        {
            // ===== EO (Electro Optical) =====
            { "EO_EV", 7 },                         // int32
            { "EO_WB", 0 },                         // int32
            // { "EO_SPOTAE", (uint)0 },               // uint32
            { "EO_HS", false },                     // bool
            { "EO_VIDEO_QUALITY", 20 },             // int32
            { "EO_RESOLUTION", 0 },                 // int32
            { "EO_BITRATE", 2.5f },                 // float
            { "EO_ZOOM_MODE", 4 },                  // int32
            // { "EO_DZOOM", 1.0f },                   // float
            // { "EO_COMMAND", "" },                   // string (writeonly)

            // ===== IR (Infrared) =====
            { "IR_PALETTE", 0 },                    // int32
            { "IR_ZOOM", 1.0f },                    // float
            { "IR_GAIN", true },                    // bool
            { "IR_THERMOMETRY", false },            // bool
            // { "IR_TEMP_POINT", "" },                // custom
            // { "IR_TEMP_LINE", "" },                 // custom
            // { "IR_TEMP_RECT", "" },                 // custom
            // { "IR_TEMP_DATA", "" },                 // readonly custom

            // ===== Gimbal =====
            { "GB_SPEED", 50 },                     // int32
            { "YAW_SMOOTH", 50 },                   // int32
            { "YAW_MODE", 0 },                      // int32

            // ===== Landing =====
            // { "LANDING_MODE", false },              // bool
            // { "LANDMODE_SWITCH", 0 },               // int32

            // ===== Tracking =====
            { "TRACK_GAIN", 50 },                   // int32
            { "TRACK_ALGORITHM", "None" },          // string
            { "SMART_SELECT", "None" },             // string
            // { "TRACK_PLUGINS", "" },                // readonly string
            // { "DETECT_PLUGINS", "" },               // readonly string

            // ===== AI / NV =====
            { "AI_RESOLUTION", 1 },                 // int32
            { "AI_OSD", false },                    // bool
            { "AI_SOURCE", "eo" },                  // string

            { "NV_POWER_MODE", 1 },                 // int32
            // { "NV_STATUS", "" },                    // readonly custom
            // { "NV_DEBUG", false },                  // bool

            // ===== System =====
            // { "TIME_ZONE", "" },                    // string
            { "UUID", "" },                         // readonly string
            // { "FACTORY_CALI", 0 },                  // int32
            // { "FACTORY_DATA", "" },                 // readonly custom
            // { "CALIBRATE_FLAGS", 0 },               // readonly int32
            // { "JSON_TR_REQ", "" },                  // readonly string
            { "CAM_FLIP", false },                  // bool
            { "TOF_EN", false },                    // bool
            {"TRACK_MODE",0},
            {"MERGE_DISPLAY",0}

            // ===== Detection =====
            // { "DETECT_OBJECTS", "" },               // readonly custom
            // { "DETECT_STATS", "" }                  // readonly custom
        };
        // this.cameraSettings["EO_EV"] = 0;
        // cameraSettings["EO_WB"] = 0;

        public GremsyControl(BaseCameraController parentController)
        {
            InitializeComponent();
            this._parentController = parentController;
            this._virtualJoystick = new VirtualJoystick(this._parentController);
            this.tableLayoutPanel1.Controls.Add(this._virtualJoystick,0,0);
            this.tableLayoutPanel1.Controls.Add(this.trackZoom,1,0);
            this.parentTableLayoutPanel.Controls.Add(this.tableLayoutPanel1,1,0);
            MainV2.comPort.OnPacketReceived += LoadCameraParameters;
            // this._parentController._flightData.GremsySwitchCameraModeToPhoto();
        }

        public void LoadCameraParameters(object sender,MAVLink.MAVLinkMessage packet)
        {
            switch (packet.msgid)
            {
                case (uint)MAVLink.MAVLINK_MSG_ID.STORAGE_INFORMATION:
                    var storagestatus = packet.ToStructure<MAVLink.mavlink_storage_information_t>();
                    Console.WriteLine("Storage Status" + storagestatus.available_capacity + " " +storagestatus.total_capacity);
                    this.camera_Storage = storagestatus.available_capacity;
                    Console.WriteLine("Camera Storage "+this.camera_Storage);
                    break; 
                case (uint)MAVLink.MAVLINK_MSG_ID.CAMERA_SETTINGS:
                    var camerasettings = packet.ToStructure<MAVLink.mavlink_camera_settings_t>();
                    Console.WriteLine("Camera Settings" + camerasettings.mode_id + " " +camerasettings.zoomLevel);
                    break;
                case (uint)MAVLink.MAVLINK_MSG_ID.PARAM_EXT_VALUE:

                    var paramextvalue = packet.ToStructure<MAVLink.mavlink_param_ext_value_t>();

                    // 1️⃣ Clean parameter name
                    string paramName = Encoding.ASCII.GetString(paramextvalue.param_id).Trim('\0').Trim();

                    if (!this.cameraSettings.ContainsKey(paramName))
                    {
                        Console.WriteLine($"Unknown parameter received: {paramName}");
                        break;
                    }

                    byte[] rawValue = paramextvalue.param_value;
                    object currentTypeValue = this.cameraSettings[paramName];
                    object decodedValue = null;

                    // 2️⃣ Decode based on initialized type
                    switch (currentTypeValue)
                    {
                        case int _:
                            decodedValue = BitConverter.ToInt32(rawValue, 0);
                            break;

                        case uint _:
                            decodedValue = BitConverter.ToUInt32(rawValue, 0);
                            break;

                        case float _:
                            decodedValue = BitConverter.ToSingle(rawValue, 0);
                            break;

                        case bool _:
                            decodedValue = BitConverter.ToInt32(rawValue, 0) == 1;
                            break;

                        case string _:
                            decodedValue = Encoding.ASCII.GetString(rawValue).Trim('\0');
                            break;

                        default:
                            decodedValue = rawValue; // fallback
                            break;
                    }

                    // 3️⃣ Update dictionary
                    this.cameraSettings[paramName] = decodedValue;

                    if(paramName == "IR_ZOOM")
                    {
                        foreach (var item in cameraSettings)
                        {
                            Console.WriteLine($"{item.Key} : {item.Value}");
                        }
                    }
                    // 4️⃣ Print clean output
                    Console.WriteLine($"Updated {paramName} → {decodedValue}");
                    break;
                case (uint)MAVLink.MAVLINK_MSG_ID.CAMERA_TRACKING_GEO_STATUS:
                    var camGeoStatus = packet.ToStructure<MAVLink.mavlink_camera_tracking_geo_status_t>();
                    Console.WriteLine("Camera Geo Status {0} {1} {2}",camGeoStatus.tracking_status,camGeoStatus.lat,camGeoStatus.dist);
                    break; 
                default:
                    break;   
            } 
        }
        private void TrackZoom_ValueChanged(object sender, EventArgs e)
        {
            if (trackZoom.Value > 25)
            {
                if (!zoomInActive)
                {
                    // SendZoomIn();
                    this._parentController._flightData.GremsyZoomIn();
                    zoomInActive = true;
                    zoomOutActive = false;
                }
            }
            else if (trackZoom.Value < -25)
            {
                if (!zoomOutActive)
                {
                    this._parentController._flightData.GremsyZoomOut();
                    zoomOutActive = true;
                    zoomInActive = false;
                }
            }
            else
            {
                this._parentController._flightData.GremsyZoomStop();
                zoomInActive = false;
                zoomOutActive = false;
            }
        }
        private void TrackZoom_MouseUp(object sender, MouseEventArgs e)
        {
            // Return slider to center
            trackZoom.Value = 0;

            // Stop zoom motor
            this._parentController._flightData.GremsyZoomStop();

            zoomInActive = false;
            zoomOutActive = false;
        }
        private void BtnStartRecording_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Recording Started.");
            bool result =this._parentController._flightData.GremsyStartRecording();
            if (result){
                Console.WriteLine("Start Recording Command Acknowledged.");
            }
            else
            {
                CustomMessageBox.Show(
                    "Failed to Send Start Recording Check Camera Mode and Try Again",
                    "Recording Status",
                    MessageBoxButtons.OK,
                    CustomMessageBox.MessageBoxIcon.Error
                );
            }
        }

        private void BtnStopRecording_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Recording Stopped.");
            bool result = this._parentController._flightData.GremsyStopRecording();
            if (result)            {
                Console.WriteLine("Stop Recording Command Acknowledged.");
            }
            else
            {
                CustomMessageBox.Show(
                    "Failed to Send Stop Recording Check Camera Mode and Try Again",
                    "Recording Status",
                    MessageBoxButtons.OK,
                    CustomMessageBox.MessageBoxIcon.Error
                );
            }
        }
        private void BtnTakePhoto_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Photo Taken.");
            bool result = this._parentController._flightData.GremsyTakePhoto();
            if (result)            {
                Console.WriteLine("Take Photo Command Acknowledged.");
            }
            else
            {
                CustomMessageBox.Show(
                    "Failed to Send Take Photo Check Camera Mode and Try Again",
                    "Photo Status",
                    MessageBoxButtons.OK,
                    CustomMessageBox.MessageBoxIcon.Error
                );
            }
        }
        private void BtnCameraSettings_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Settings Applied.");
            // PARAM_EXT_REQUEST_LIST
            var msg = new MAVLink.mavlink_param_ext_request_list_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            Console.WriteLine("Requesting Camera Parameters... {0}",msg);
            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
            using (CameraSettingsForm settings = new CameraSettingsForm(this))
            {
                if (settings.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine("Camera Settings Saved.");
                    MessageBox.Show("Settings updated");
                }
            }
        }

        private void BtnZoomIn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Zooming In.");
            this._parentController._flightData.GremsyZoomIn();
        }
        private void BtnZoomOut_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Zooming Out.");
            this._parentController._flightData.GremsyHomeCommand();
        }

        private void BtnZoomStop_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Zoom Stopped.");
            this._parentController._flightData.GremsyPointDownCommand();
        }
        private void BtnStopTracking_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Stopped Tracking.");
            this._parentController._flightData.GremsyStopTracking();
        }
        // private void BtnUp_Click(object sender, EventArgs e)
        // {
        //     Console.WriteLine("Gremsy Camera Moving Up.");
        //     this._parentController._flightData.GremsyPitchYawControl(1,0);
        // }   
        // private void BtnDown_Click(object sender, EventArgs e)
        // {
        //     Console.WriteLine("Gremsy Camera Moving Down.");
        //     this._parentController._flightData.GremsyPitchYawControl(-1,0);
        // }
        // private void BtnLeft_Click(object sender, EventArgs e)
        // {
        //     Console.WriteLine("Gremsy Camera Moving Left.");
        //     this._parentController._flightData.GremsyPitchYawControl(0,-1);
        // }
        // private void BtnRight_Click(object sender, EventArgs e)
        // {
        //     Console.WriteLine("Gremsy Camera Moving Right.");
        //     this._parentController._flightData.GremsyPitchYawControl(0,1);
        // }
        // private void BtnHome_Click(object sender, EventArgs e)
        // {
        //     Console.WriteLine("Gremsy Camera Movement Stopped.");
        //     this._parentController._flightData.GremsyHomeCommand();
        // }
        private void ChkRecordMode_CheckedChanged(object sender, EventArgs e)
        {
            if (btnGremsyTest.Text == "Photo Mode")
            {
                bool result = StartRecordingMode();
                if (result)
                {
                    btnGremsyTest.Text = "Recording Mode";
                }
                else
                {
                    CustomMessageBox.Show(
                    "Failed to Switch to Video Mode Try Again",
                    "Camera Mode Status",
                    MessageBoxButtons.OK,
                    CustomMessageBox.MessageBoxIcon.Error
                    );
                }
                
                // Send gimbal command → start video recording mode
            }
            else
            {   
                // Send gimbal command → photo mode
                bool result = SetPhotoMode();
                if (result)
                {
                    btnGremsyTest.Text = "Photo Mode";
                }
                else
                {
                    CustomMessageBox.Show(
                    "Failed to Switch to Photo Mode Try Again",
                    "Camera Mode Status",
                    MessageBoxButtons.OK,
                    CustomMessageBox.MessageBoxIcon.Error
                    );
                }
            }
        }
        private bool StartRecordingMode()
        {
            // Example: call your Gremsy / Viewpro / MAVLink command
            Console.WriteLine("Recording Mode Enabled");
            bool result = this._parentController._flightData.GremsySwitchCameraModeToVideo();
            return result;
        }

        private bool SetPhotoMode()
        {
            Console.WriteLine("Photo Mode Enabled");
            bool result = this._parentController._flightData.GremsySwitchCameraModeToPhoto();
            return result;
        }


    }
}
