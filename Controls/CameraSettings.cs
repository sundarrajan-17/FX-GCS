using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{   
    public class CameraSettingsForm : Form
    {
        private Panel scrollPanel;
        private TableLayoutPanel table;
        private GremsyControl _parentControl;
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
            // AddCombo("Thermal View Mode", new[] { "None","Full", "Picture-In-Picture", "Blend" });
            // AddSlider("Blend Opacity", 0, 100, 30);
            AddCombo("Camera EV", new[] {"-4.5","-3", "-1.5", "0", "+1.5", "+3","4.5"});
            AddCombo("White Balance", new[] { "Auto", "Indoor", "Outdoor" });
            AddToggle("High Sensitivity", (bool)this._parentControl.cameraSettings["EO_HS"] == false ? false : true);
            AddCombo("Video Quality", new[] { "Default", "Medium", "High" });
            AddCombo("RTSP Resolution", new[] { "720p", "1080p" });
            // AddSlider("RTSP Bitrate", 0, 100, 50);
            AddCombo("Zoom Feature", new[] { "Normal", "Super Resolution" });
            AddToggle("IR Thermometry", (bool)this._parentControl.cameraSettings["IR_THERMOMETRY"] == false ? false : true);
            AddCombo("Infrared Palette", new[] { "White Hot","Sepia","Ironbow","Rainbow","Night","Aurora","Red Hot","Jungle","Medical","Black Hot","Glory Hot" });
            // AddSlider("Gimbal Speed", 0, 100, 40);
            AddCombo("Yaw Mode", new[] { "Head", "Global" });
            AddCombo("Track Algorithm", new[] { "None", "Nano", "SiamRpn" });
            AddCombo("Smart Select",new[] {"None","Yolov11","Yolov8","Yolov5"});
            AddCombo("AI Resolution", new[] {"640X480","1280X720","1920X1080"});
            AddToggle("AI OSD", (bool)this._parentControl.cameraSettings["AI_OSD"] == false ? false : true);
            AddToggle("RangeFinder", (bool)this._parentControl.cameraSettings["TOF_EN"] == false ? false : true);
            if((bool)this._parentControl.cameraSettings["AI_OSD"] == false)
            {
                AddCombo("AI Video Source", new[] { "EO", "IR" });
            }
            // AddButton("Apply Settings");
            AddButton("Reset Camera Defaults");
        }

        private void RefreshUI()
        {
            table.Controls.Clear();
            table.RowStyles.Clear();
            table.RowCount = 0;

            AddCombo("Camera", new[] { "R1", "R2", "R3" });
            AddCombo("Camera EV", new[] {"-4.5","-3", "-1.5", "0", "+1.5", "+3","4.5"});
            AddCombo("White Balance", new[] { "Auto", "Indoor", "Outdoor" });
            AddToggle("High Sensitivity", (bool)_parentControl.cameraSettings["EO_HS"]);
            AddCombo("Video Quality", new[] { "Default", "Medium", "High" });
            AddCombo("RTSP Resolution", new[] { "720p", "1080p" });
            AddCombo("Zoom Feature", new[] { "Normal", "Super Resolution" });
            AddToggle("IR Thermometry", (bool)_parentControl.cameraSettings["IR_THERMOMETRY"]);
            AddCombo("Infrared Palette", new[] { "White Hot","Sepia","Ironbow","Rainbow","Night","Aurora","Red Hot","Jungle","Medical","Black Hot","Glory Hot" });
            AddCombo("Yaw Mode", new[] { "Head", "Global" });
            AddCombo("Track Algorithm", new[] { "None", "Nano", "SiamRpn" });
            AddCombo("Smart Select", new[] {"None","Yolov11","Yolov8","Yolov5"});
            AddCombo("AI Resolution", new[] {"640X480","1280X720","1920X1080"});
            AddToggle("AI OSD", (bool)_parentControl.cameraSettings["AI_OSD"]);
            AddToggle("RangeFinder", (bool)_parentControl.cameraSettings["TOF_EN"]);

            if (!(bool)_parentControl.cameraSettings["AI_OSD"])
            {
                AddCombo("AI Video Source", new[] { "EO", "IR" });
            }

            AddButton("Reset Camera Defaults");
        }

        private void ApplyAIVideoSource(string source)
        {
            Console.WriteLine($"Applying AI Video Source: {source}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set video source
            // _parentControl.SetAIVideoSource(source);
            if(source == "EO")
            {
                source = "eo";
            }
            else if(source == "IR")
            {
                source = "ir";
            }
            string text = "AI_SOURCE";
            byte[] id_bytes = new byte[16];
            byte[] text_bytes = Encoding.ASCII.GetBytes(text);
            // Array.Copy(text_bytes, 0, id_bytes, 0, Math.Min(text_bytes.Length, id_bytes.Length));
            // byte[] value_bytes = new byte[128];
            byte[] source_bytes = Encoding.ASCII.GetBytes(source);
            // Array.Copy(source_bytes, 0, value_bytes, 0, Math.Min(source_bytes.Length, value_bytes.Length));

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = source_bytes;
            msg.param_type = 6;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }
        private void ApplyEVValue(int EvValue)
        {
            Console.WriteLine($"Applying EV Value: {EvValue}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set EV value
            // _parentControl.SetEVValue(EvValue);
            int value = 10 - EvValue;
            string text = "EO_EV";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = BitConverter.GetBytes(value);

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 6;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        } 

        private void ApplyWhiteBalance(string mode)
        {
            Console.WriteLine($"Applying White Balance: {mode}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set white balance
            // _parentControl.SetWhiteBalance(mode);
            int value = 0;
            if(mode == "Auto")
            {
                value = 0;
            }else if(mode == "Indoor")
            {
                value = 1;
            }else if(mode == "Outdoor")
            {
                value = 2;
            }
            string text = "EO_WB";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = BitConverter.GetBytes(value);

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 6;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }
        private void ApplyCameraSelection(string camera)
        {
            Console.WriteLine($"Applying Camera Selection: {camera}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set camera
            // _parentControl.SetCameraSelection(camera);
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
        private void ApplyVideoQuality(string quality)
        {
            Console.WriteLine($"Applying Video Quality: {quality}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set video quality
            // _parentControl.SetVideoQuality(quality);
            int value =20;
            if(quality == "Low")
            {
                value = 20;
            }
            else if(quality == "Medium")
            {
                value = 30;
            }
            else if(quality == "High")
            {
                value = 40;
            }
            string text = "EO_VIDEO_QUALITY";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = BitConverter.GetBytes(value);

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 6;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }

        private void ApplyRTSPResolution(string resolution)
        {
            Console.WriteLine($"Applying RTSP Resolution: {resolution}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set RTSP resolution
            // _parentControl.SetRTSPResolution(resolution);
            int value = 0;
            if(resolution == "720p")
            {
                value = 0;
            }
            else if(resolution == "1080p")
            {
                value = 1;
            }
            string text = "EO_RESOLUTION";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes =BitConverter.GetBytes(value);

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 6;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }
        private void ApplyZoomFeature(string zoomMode)
        {
            Console.WriteLine($"Applying Zoom Feature: {zoomMode}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set zoom feature
            // _parentControl.SetZoomFeature(zoomMode);
            int value = 2;
            if(zoomMode == "Normal")
            {
                value = 2;
            }
            else if(zoomMode == "Super Resolution")
            {
                value = 4;
            }
            string text = "EO_ZOOM_MODE";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = BitConverter.GetBytes(value);

            // MAVLink.MAV_PARAM_TYPE param_type = MAVLink.MAV_PARAM_TYPE.MAV_PARAM_TYPE_UINT8;
            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 6;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }
        private void ApplyInfraredPalette(int palette)
        {
            Console.WriteLine($"Applying Infrared Palette: {palette}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set infrared palette
            // _parentControl.SetInfraredPalette(palette);
            string text = "IR_PALETTE";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = BitConverter.GetBytes(palette);
            // var msg = new MAVLink.mavlink_param_ext_set_t
            // {
            //     target_system = (byte)MainV2.comPort.sysidcurrent,
            //     target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA,
            //     param_id = id_bytes,
            //     param_value = value_bytes, 
            //     param_type = unchecked((byte)-1)
            // };

            var msg = new MAVLink.mavlink_param_ext_set_t();

            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 6;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }

        private void ApplyCheckBoxChangeCommon(string settingName, bool value)
        {
            Console.WriteLine($"{settingName} toggled → {(value ? "On" : "Off")}");
            // Here you can add any common logic that should happen whenever a checkbox changes
            byte[] id_bytes = Encoding.ASCII.GetBytes(settingName);
            byte[] value_bytes = BitConverter.GetBytes(value ? 1 : 0);
            var msg = new MAVLink.mavlink_param_ext_set_t();

            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 1;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }

        private void ApplyTrackAlgorithm(string algorithm)
        {
            Console.WriteLine($"Applying Track Algorithm: {algorithm}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set tracking algorithm
            // _parentControl.SetTrackAlgorithm(algorithm);
            string text = "TRACK_ALGORITHM";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = Encoding.ASCII.GetBytes(algorithm);

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 11;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }

        private void ApplySmartSelect(string option)
        {
            Console.WriteLine($"Applying Smart Select: {option}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set smart select option
            // _parentControl.SetSmartSelect(option);
            string text = "SMART_SELECT";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = Encoding.ASCII.GetBytes(option);

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 11;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }

        private void ApplyYawMode(int yawmode)
        {
            string text = "YAW_MODE";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = BitConverter.GetBytes(yawmode);

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 6;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }

        private void ApplyAIResolution(int resolution)
        {
            string text = "AI_RESOLUTION";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = BitConverter.GetBytes(resolution);

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 6;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA
            );
        }

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;

            string settingName = cmb.Tag.ToString();
            string value = cmb.SelectedItem.ToString();
            int index = cmb.SelectedIndex;

            Console.WriteLine($"{settingName} changed → {value} (Index {index})");

            // Example: immediate action
            if (settingName == "AI Video Source")
            {
                ApplyAIVideoSource(value);
            }else if(settingName == "CAMERA EV")
            {
                ApplyEVValue(index);
            }else if(settingName == "Camera")
            {
                ApplyCameraSelection(value);
            }else if(settingName == "White Balance")
            {
                ApplyWhiteBalance(value);
            }else if(settingName == "Video Quality")
            {
                ApplyVideoQuality(value);
            }else if(settingName == "RTSP Resolution")
            {
                ApplyRTSPResolution(value);
            }else if(settingName == "Zoom Feature")
            {
                ApplyZoomFeature(value);
            }else if(settingName == "Infrared Palette")
            {
                ApplyInfraredPalette(index);
            }else if(settingName == "Yaw Mode")
            {
                ApplyYawMode(index);
            }else if(settingName == "Track Algorithm")
            {
                ApplyTrackAlgorithm(value);
            }else if(settingName == "Smart Select")
            {
                ApplySmartSelect(value);
            }else if(settingName == "AI Resolution")
            {
                ApplyAIResolution(index);
            }
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

            // Example: immediate action
            if (settingName == "High Sensitivity")
            {
                ApplyCheckBoxChangeCommon("EO_HS",value);
            }else if(settingName == "IR Thermometry")
            {
                ApplyCheckBoxChangeCommon("IR_THERMOMETRY",value);
            }else if(settingName == "AI OSD")
            {
                ApplyCheckBoxChangeCommon("AI_OSD",value);
            }else if(settingName == "RangeFinder")
            {
                ApplyCheckBoxChangeCommon("TOF_EN",value);
            }
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
            }else
            {
                cb.SelectedIndex = 0;
            }
            // cb.SelectedIndex = 0;
            cb.Tag = label; // Store label for identification in event handler
            cb.SelectedIndexChanged += Combo_SelectedIndexChanged;
            AddRow(MakeLabel(label), cb);
        }

        private void AddSlider(string label, int min, int max, int value)
        {
            TrackBar tb = new TrackBar
            {
                Minimum = min,
                Maximum = max,
                Value = value,
                Dock = DockStyle.Fill
            };

            AddRow(MakeLabel(label), tb);
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

            AddRow(new Label(), btn);
        }
    }
}