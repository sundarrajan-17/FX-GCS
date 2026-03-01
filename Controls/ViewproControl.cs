using log4net;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace MissionPlanner.Controls
{
    public partial class ViewproControl : UserControl
    {
        private BaseCameraController _parentController;
        private VirtualJoystick _virtualJoystick;
        private bool zoomInActive = false;
        private bool zoomOutActive = false;
        public ViewproControl(BaseCameraController parentController)
        {
            InitializeComponent();
            this._parentController = parentController;
            this._virtualJoystick = new VirtualJoystick(this._parentController);
            this.tableLayoutPanel1.Controls.Add(this._virtualJoystick,0,0);
            this.tableLayoutPanel1.Controls.Add(this.trackZoom, 1,0);
            // this.camControlGroup.Controls.Add(this.tableLayoutPanel1);
            this.parentTableLayoutPanel.Controls.Add(this.tableLayoutPanel1,0,0);
        }

        private void TrackZoom_ValueChanged(object sender, EventArgs e)
        {
            if (trackZoom.Value > 10)
            {
                if (!zoomInActive)
                {
                    // SendZoomIn();
                    this._parentController._flightData.viewproZoomInCommand();
                    zoomInActive = true;
                    zoomOutActive = false;
                }
            }
            else if (trackZoom.Value < -10)
            {
                if (!zoomOutActive)
                {
                    this._parentController._flightData.viewproZoomOutCommand();
                    zoomOutActive = true;
                    zoomInActive = false;
                }
            }
            else
            {
                this._parentController._flightData.viewproZoomStopCommand();
                zoomInActive = false;
                zoomOutActive = false;
            }
        }
        private void TrackZoom_MouseUp(object sender, MouseEventArgs e)
        {
            // Return slider to center
            trackZoom.Value = 0;

            // Stop zoom motor
            this._parentController._flightData.viewproZoomStopCommand();

            zoomInActive = false;
            zoomOutActive = false;
        }
        private void BtnStartRecording_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.viewproStartRecordingCommand();
        }
        private void BtnStopRecording_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.viewproStopRecordingCommand();
        }
        private void BtnTakePhoto_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.viewproTakePictureCommand();
        }
        private void BtnOsdOn_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.viewproOsdOnCommand();
        }
        private void BtnOsdOff_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.viewproOsdOffCommand();
        }
        private void BtnAiOsdOn_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.viewproStartRecordingCommand();
        }
        private void BtnAiOsdOff_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.viewproStartRecordingCommand();
        }
        private void BtnDZoomPlus_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.viewproStartRecordingCommand();
        }
        private void BtnDZoomMinus_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.viewproStartRecordingCommand();
        }
        private void CMB_setEoIrMode_Click(object sender,EventArgs e)
        {
            try
            {
                // ((Control) sender).Enabled = false;
                // MainV2.comPort.setWPCurrent(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid,
                //     (ushort) CMB_setEoIrMode.SelectedIndex); // set nav to
                Console.WriteLine("Selected Index");
            }
            catch
            {
                CustomMessageBox.Show(Strings.CommandFailed, Strings.ERROR);
            }

            ((Control) sender).Enabled = true;
        }
        private void setEoIrMode_Click(object sender,EventArgs e)
        {
            Console.WriteLine("Set Eo Ir Mode {0}",CMB_setEoIrMode.SelectedIndex);
            if(CMB_setEoIrMode.SelectedIndex == 0)
            {
                this._parentController._flightData.viewproEo_Command();
            }else if(CMB_setEoIrMode.SelectedIndex == 1)
            {
                this._parentController._flightData.viewproEoIr_WhiteCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 2)
            {
                this._parentController._flightData.viewproEoIr_BlackCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 3)
            {
                this._parentController._flightData.viewproEoIr_PseudoCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 4)
            {
                this._parentController._flightData.viewproIrEo_WhiteCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 5)
            {
                this._parentController._flightData.viewproIrEo_BlackCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 6)
            {
                this._parentController._flightData.viewproIrEo_PseudoCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 7)
            {
                this._parentController._flightData.viewproIr_WhiteCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 8)
            {
                this._parentController._flightData.viewproIr_BlackCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 9)
            {
                this._parentController._flightData.viewproIr_PseudoCommand();
            }
        }
    }
}
