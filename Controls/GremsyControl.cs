using log4net;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace MissionPlanner.Controls
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

        public Dictionary<string, object> cameraSettings = new Dictionary<string, object>();
        // this.cameraSettings["EO_EV"] = 0;
        // cameraSettings["EO_WB"] = 0;

        public GremsyControl(BaseCameraController parentController)
        {
            InitializeComponent();
            this._parentController = parentController;
            this._virtualJoystick = new VirtualJoystick(this._parentController);
            this.tableLayoutPanel3.Controls.Add(this._virtualJoystick,0,0);
            this.camControlGroup.Text = "Camera Controls";
            this.camControlGroup.Controls.Add(this.tableLayoutPanel3);
            this.mainFlow.Controls.Add(this.camControlGroup,1,0);
            // this.mainFlow.Controls.Add(this.tblGremsy);
            MainV2.comPort.OnPacketReceived += LoadCameraParameters;
            this.cameraSettings["EO_EV"] = 0;
            this.cameraSettings["EO_WB"] = 0;
            this.cameraSettings["EO_HS"] = 0;
            this.cameraSettings["EO_VIDEO_QUALITY"] = 20;
            this.cameraSettings["EO_RESOLUTION"] = 0;
            this.cameraSettings["EO_BITRATE"] = 0.0f;
            this.cameraSettings["EO_ZOOM_MODE"]= 2;
            this.cameraSettings["EO_DZOOM"] = 1.0f;
            this.cameraSettings["IR_THERMOMETRY"] = 0;
            this.cameraSettings["IR_PALETTE"] = 0;
            this.cameraSettings["IR_ZOOM"] = 1.0f;
            this.cameraSettings["YAW_MODE"] = 0;
            this.cameraSettings["AI_RESOLUTION"] = 0;
            this.cameraSettings["AI_OSD"] = 0;
            this.cameraSettings["AI_SOURCE"] = "eo";
        }

        // private void BtnGremsy_Click(object sender, EventArgs e)
        // {
        //     Console.WriteLine("Gremsy Camera Settings Applied.");
        // }

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
                    Console.WriteLine("Param Ext Value" + paramextvalue.param_id + " " +paramextvalue.param_value);
                    if(this.cameraSettings.ContainsKey(Encoding.ASCII.GetString(paramextvalue.param_id)))
                    {
                        this.cameraSettings[Encoding.ASCII.GetString(paramextvalue.param_id)] = paramextvalue.param_value;
                    }   
                    break;
                default:
                    break;   
            } 
        }
        private void TrackZoom_ValueChanged(object sender, EventArgs e)
        {
            if (trackZoom.Value > 10)
            {
                if (!zoomInActive)
                {
                    // SendZoomIn();
                    this._parentController._flightData.GremsyZoomIn();
                    zoomInActive = true;
                    zoomOutActive = false;
                }
            }
            else if (trackZoom.Value < -10)
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
            this._parentController._flightData.GremsyStartRecording();
        }

        private void BtnStopRecording_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Recording Stopped.");
            this._parentController._flightData.GremsyStopRecording();
        }
        private void BtnTakePhoto_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Photo Taken.");
            this._parentController._flightData.GremsyTakePhoto();
        }
        private void BtnCameraSettings_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Settings Applied.");
            // PARAM_EXT_REQUEST_LIST
            var msg = new MAVLink.mavlink_param_ext_request_list_t
            {
                target_system = (byte)MainV2.comPort.sysidcurrent,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_GIMBAL,
            };

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_GIMBAL
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
            this._parentController._flightData.GremsyZoomOut();
        }

        private void BtnZoomStop_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Gremsy Camera Zoom Stopped.");
            this._parentController._flightData.GremsyZoomStop();
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
                btnGremsyTest.Text = "Recording Mode";
                
                // Send gimbal command → start video recording mode
                StartRecordingMode();
            }
            else
            {
                btnGremsyTest.Text = "Photo Mode";
                
                // Send gimbal command → photo mode
                SetPhotoMode();
            }
        }
        private void StartRecordingMode()
        {
            // Example: call your Gremsy / Viewpro / MAVLink command
            Console.WriteLine("Recording Mode Enabled");
            this._parentController._flightData.GremsySwitchCameraModeToVideo();
        }

        private void SetPhotoMode()
        {
            Console.WriteLine("Photo Mode Enabled");
           this._parentController._flightData.GremsySwitchCameraModeToPhoto();
        }


    }
}
