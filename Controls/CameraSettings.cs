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

            // ---- ADD SETTINGS ----
            AddCombo("Camera", new[] { "R1", "R2", "R3" });
            // AddCombo("Thermal View Mode", new[] { "None","Full", "Picture-In-Picture", "Blend" });
            // AddSlider("Blend Opacity", 0, 100, 30);
            AddCombo("Camera EV", new[] { "-2", "-1", "0", "+1", "+2" });
            AddCombo("White Balance", new[] { "Auto", "Day", "Cloudy" });
            AddToggle("High Sensitivity", (int)_parentControl.cameraSettings["EO_HS"] == 0 ? false : true);
            AddCombo("Video Quality", new[] { "Low", "Medium", "High" });
            AddCombo("RTSP Resolution", new[] { "720p", "1080p" });
            // AddSlider("RTSP Bitrate", 0, 100, 50);
            AddCombo("Zoom Feature", new[] { "Normal", "Super Resolution" });
            AddToggle("IR Thermometry", (int)_parentControl.cameraSettings["IR_THERMOMETRY"] == 0 ? false : true);
            AddCombo("Infrared Palette", new[] { "Ironbow", "White Hot" });
            // AddSlider("Gimbal Speed", 0, 100, 40);
            AddCombo("Yaw Mode", new[] { "Follow", "Lock" });
            AddToggle("AI OSD", (int)_parentControl.cameraSettings["AI_OSD"] == 0 ? false : true);
            AddCombo("AI Video Source", new[] { "EO", "IR" });
            // AddButton("Apply Settings");
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
            Array.Copy(text_bytes, 0, id_bytes, 0, Math.Min(text_bytes.Length, id_bytes.Length));
            byte[] value_bytes = new byte[128];
            byte[] source_bytes = Encoding.ASCII.GetBytes(source);
            Array.Copy(source_bytes, 0, value_bytes, 0, Math.Min(source_bytes.Length, value_bytes.Length));

            // MAVLink.MAV_PARAM_TYPE param_type = MAVLink.MAV_PARAM_TYPE.MAV_PARAM_TYPE_UINT8;
            var msg = new MAVLink.mavlink_param_ext_set_t
            {
                target_system = (byte)MainV2.comPort.sysidcurrent,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_GIMBAL,
                param_id = id_bytes,
                param_value = value_bytes, 
                param_type = unchecked((byte)-1)
            };

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_GIMBAL
            );
        }
        private void ApplyThermalMode(string mode)
        {
            Console.WriteLine($"Applying Thermal View Mode: {mode}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set thermal mode
            // _parentControl.SetThermalViewMode(mode);
        }   
        private void ApplyCameraSelection(string camera)
        {
            Console.WriteLine($"Applying Camera Selection: {camera}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set camera
            // _parentControl.SetCameraSelection(camera);
            if(camera == "R3")
            {
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
            }
        }
        private void ApplyVideoQuality(string quality)
        {
            Console.WriteLine($"Applying Video Quality: {quality}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set video quality
            // _parentControl.SetVideoQuality(quality);
            if(quality == "Low")
            {
                quality = "20";
            }
            else if(quality == "Medium")
            {
                quality = "30";
            }
            else if(quality == "High")
            {
                quality = "40";
            }
            int.TryParse(quality, out int value);
            string text = "EO_VIDEO_QUALITY";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = Encoding.ASCII.GetBytes(value.ToString());

            // MAVLink.MAV_PARAM_TYPE param_type = MAVLink.MAV_PARAM_TYPE.MAV_PARAM_TYPE_UINT8;
            var msg = new MAVLink.mavlink_param_ext_set_t
            {
                target_system = (byte)MainV2.comPort.sysidcurrent,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_GIMBAL,
                param_id = id_bytes,
                param_value = value_bytes, 
                param_type = unchecked((byte)-1)
            };

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_GIMBAL
            );
        }

        private void ApplyRTSPResolution(string resolution)
        {
            Console.WriteLine($"Applying RTSP Resolution: {resolution}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set RTSP resolution
            // _parentControl.SetRTSPResolution(resolution);
            if(resolution == "720p")
            {
                resolution = "0";
            }
            else if(resolution == "1080p")
            {
                resolution = "1";
            }
            int.TryParse(resolution, out int value);
            string text = "EO_RESOLUTION";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = Encoding.ASCII.GetBytes(value.ToString());

            // MAVLink.MAV_PARAM_TYPE param_type = MAVLink.MAV_PARAM_TYPE.MAV_PARAM_TYPE_UINT8;
            var msg = new MAVLink.mavlink_param_ext_set_t
            {
                target_system = (byte)MainV2.comPort.sysidcurrent,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA,
                param_id = id_bytes,
                param_value = value_bytes, 
                param_type = unchecked((byte)-1)
            };

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_GIMBAL
            );
        }
        private void ApplyZoomFeature(string zoomMode)
        {
            Console.WriteLine($"Applying Zoom Feature: {zoomMode}");
            // Example: call your Gremsy / Viewpro / MAVLink command to set zoom feature
            // _parentControl.SetZoomFeature(zoomMode);
            if(zoomMode == "Normal")
            {
                zoomMode = "2";
            }
            else if(zoomMode == "Super Resolution")
            {
                zoomMode = "4";
            }
            int.TryParse(zoomMode, out int value);
            string text = "EO_ZOOM_MODE";
            byte[] id_bytes = Encoding.ASCII.GetBytes(text);
            byte[] value_bytes = Encoding.ASCII.GetBytes(value.ToString());

            // MAVLink.MAV_PARAM_TYPE param_type = MAVLink.MAV_PARAM_TYPE.MAV_PARAM_TYPE_UINT8;
            var msg = new MAVLink.mavlink_param_ext_set_t
            {
                target_system = (byte)MainV2.comPort.sysidcurrent,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_GIMBAL,
                param_id = id_bytes,
                param_value = value_bytes, 
                param_type = unchecked((byte)-1)
            };

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_GIMBAL
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
                ApplyThermalMode(value);
            }else if(settingName == "Camera")
            {
                ApplyCameraSelection(value);
            }else if(settingName == "White Balance")
            {
                // ApplyWhiteBalance(value);
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
                // ApplyInfraredPalette(value);
            }else if(settingName == "Yaw Mode")
            {
                // ApplyYawMode(value);
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
                cb.SelectedIndex = (int)_parentControl.cameraSettings["EO_VIDEO_QUALITY"] == 20 ? 0 : (int)_parentControl.cameraSettings["EO_VIDEO_QUALITY"] == 30 ? 1 : 2; // Default to Medium
            }else if(label == "RTSP Resolution")
            {
                cb.SelectedIndex = (int)_parentControl.cameraSettings["EO_RESOLUTION"] == 0 ? 0 : 1; // Default to 1080p
            }else if(label == "Zoom Feature")
            {
                cb.SelectedIndex = (int)_parentControl.cameraSettings["EO_ZOOM_MODE"] == 2 ? 0 : 1; // Default to Normal
            }else if(label == "Infrared Palette")
            {
                cb.SelectedIndex = (int)_parentControl.cameraSettings["IR_PALETTE"]; // Default to Ironbow
            }else if(label == "Yaw Mode")
            {
                cb.SelectedIndex = (int)_parentControl.cameraSettings["YAW_MODE"]; // Default to Follow
            }else if(label == "AI Video Source")
            {
                cb.SelectedIndex = (string)_parentControl.cameraSettings["AI_SOURCE"] == "eo" ? 0 : 1; // Default to EO
            }else if(label == "Camera")
            {
                cb.SelectedIndex = 2;
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

            chk.CheckedChanged += (s, e) =>
            {
                chk.Text = chk.Checked ? "On" : "Off";
            };

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