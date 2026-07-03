using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XagSurveillanceGCS.Controls
{   
    public class CameraVioSettingsForm : Form
    {
        private Panel scrollPanel;
        private TableLayoutPanel table;
        private GremsyVioControl _parentControl;

        public CameraVioSettingsForm(GremsyVioControl parentControl)
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
            AddCombo("Camera Mode", new[] { "Photo", "Video" });
            AddCombo("Camera Record Source", new[] { "Both","EO", "IR","OSD"});
            AddCombo("Camera Stream Source", new[] { "EO/IR","EO", "IR","IR/EO","Sync" });
            AddCombo("Zoom Mode", new[] { "Combine", "Super Resolution" });
            AddCombo("White Balance", new[] { "Auto", "Indoor", "Outdoor","One Push","ATW","Manual" });
            AddCombo("Infrared Palette", new[] { "White Hot","Fulgurite","IronRed","HotIron","Medical","Arctic","Rainbow1","Rainbow2","Tint","Black Hot" });
            AddCombo("Gimbal Mode", new[] {"OFF","Lock","Follow","Mapping","Reset" });
            AddCombo("Object Detection",new[] {"Disabled","Enabled"});
            // AddCombo("Tracking",new[] {"Disabled","Enabled"});
            AddCombo("Combine Zoom Level",new[] {"1X","10X","20X","40X","80X","120X","240X"});
            AddCombo("SuperRes Zoom Level",new[] {"1X", "2X","4X","6X","8X","10X","12X","14X","16X","18X","20X","22X","24X","26X","28X","30X"});
            AddCombo("RC Mode",new[] {"Gremsy","Standard"});
            AddCombo("LRF Mode",new[] {"1Hz","4Hz","10Hz","OFF"});
            AddCombo("OSD Mode",new[] {"Disable","Debug","Status"});
        }

        private void ApplyCameraSettings(string name,string value)
        {
            Console.WriteLine("Setting Name {0}, Param Value {1}",name,value);
            byte[] id_bytes = new byte[16];
            id_bytes = Encoding.ASCII.GetBytes(name);
            // Array.Copy(text_bytes, 0, id_bytes, 0, Math.Min(text_bytes.Length, id_bytes.Length));
            // byte[] value_bytes = new byte[128];
            byte[] value_bytes = new byte[128];
            value_bytes = BitConverter.GetBytes(int.Parse(value));
            // char[] id_chars = new char[16];
            // id_chars = name.ToCharArray();
            // char[] value_chars = new char[128];
            // value_chars = value.ToCharArray();
            // Array.Copy(source_bytes, 0, value_bytes, 0, Math.Min(source_bytes.Length, value_bytes.Length));

            var msg = new MAVLink.mavlink_param_ext_set_t();
            msg.target_system = (byte)MainV2.comPort.sysidcurrent;
            msg.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA2;
            msg.param_id = id_bytes;
            msg.param_value = value_bytes;
            msg.param_type = 5;

            MainV2.comPort.sendPacket(
                msg,
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA2
            );
        }

        private void ApplyCameraMode(int index)
        {
            // Example: Camera Mode might require multiple parameters to be set
            if(index == 0) // Photo
            {
                MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA2,MAVLink.MAV_CMD.SET_CAMERA_MODE,0,0,0,0,0,0,0);
            }
            else if(index == 1) // Video
            {
               MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_CAMERA2,MAVLink.MAV_CMD.SET_CAMERA_MODE,0,1,0,0,0,0,0); 
            }
        }

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;

            string settingName = cmb.Tag.ToString();
            string value = cmb.SelectedItem.ToString();
            int index = cmb.SelectedIndex;

            Console.WriteLine($"{settingName} changed → {value} (Index {index})");

            string paramid = "";
            string paramvalue = "";

            // Example: immediate action
            if (settingName == "Camera Record Source")
            {
                paramid = "C_V_REC";
                if(index == 3)
                {
                    paramvalue = "5";
                }
                else
                {
                    paramvalue = index.ToString();    
                }
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "Zoom Mode")
            {
                paramid = "C_V_ZM_MODE";
                paramvalue = index == 0 ? "0" : "2";
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "Infrared Palette")
            {
                paramid = "C_T_PALETTE";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "White Balance")
            {
                paramid = "C_V_WB";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "Gimbal Mode")
            {
                paramid = "GB_MODE";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "Camera Stream Source")
            {
                paramid = "C_SOURCE";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "Camera Mode")
            {
                paramid = "CAM_MODE";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);
                ApplyCameraMode(index);
            }else if(settingName == "Combine Zoom Level")
            {
                paramid = "C_V_ZM_CB_LV";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "SuperRes Zoom Level")
            {
                paramid = "C_V_ZM_SR_LV";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "Object Detection")
            {
                paramid = "TRACK_MODE";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "RC Mode")
            {
                paramid = "RC_MODE";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);
            }else if(settingName == "LRF Mode")
            {
                paramid = "LRF_MODE";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);                
            }else if(settingName == "OSD Mode")
            {
                paramid = "OSD_MODE";
                paramvalue = index.ToString();
                ApplyCameraSettings(paramid, paramvalue);                
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
        }

        private void AddCombo(string label, string[] items)
        {
            ComboBox cb = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cb.Items.AddRange(items);
            cb.SelectedIndex = 0;
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