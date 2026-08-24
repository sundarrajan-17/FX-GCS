using log4net;
using XagSurveillanceGCS.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace XagSurveillanceGCS.Controls
{
    public partial class XagCamControl : MyUserControl
    {
        private BaseCameraController _parentController;
        private VirtualJoystick _virtualJoystick;
        private bool zoomInActive = false;
        private bool zoomOutActive = false;
        public XagCamControl(BaseCameraController parentController)
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
                    this._parentController._flightData.XagCamZoomInCommand();
                    zoomInActive = true;
                    zoomOutActive = false;
                }
            }
            else if (trackZoom.Value < -10)
            {
                if (!zoomOutActive)
                {
                    this._parentController._flightData.XagCamZoomOutCommand();
                    zoomOutActive = true;
                    zoomInActive = false;
                }
            }
            else
            {
                this._parentController._flightData.XagCamZoomStopCommand();
                zoomInActive = false;
                zoomOutActive = false;
            }
        }
        private void TrackZoom_MouseUp(object sender, MouseEventArgs e)
        {
            // Return slider to center
            trackZoom.Value = 0;

            // Stop zoom motor
            this._parentController._flightData.XagCamZoomStopCommand();

            zoomInActive = false;
            zoomOutActive = false;
        }
        private void BtnStartRecording_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.XagCamStartRecordingCommand();
        }
        private void BtnStopRecording_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.XagCamStopRecordingCommand();
        }
        private void BtnTakePhoto_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.XagCamTakePictureCommand();
        }
        private void BtnOsdOn_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.XagCamOsdOnCommand();
        }
        private void BtnOsdOff_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.XagCamOsdOffCommand();
        }
        private void BtnAiOsdOn_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.XagCamAiOsdOnCommand();
        }
        private void BtnAiOsdOff_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.XagCamAiOsdOffCommand();
        }
        private void BtnDZoomPlus_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.XagCamDZoomPlusCommand();
        }
        private void BtnDZoomMinus_Click(object sender,EventArgs e)
        {
            this._parentController._flightData.XagCamDZoomMinusCommand();
        }
        private void CMB_setEoIrMode_Click(object sender,EventArgs e)
        {
            try
            {
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
                this._parentController._flightData.XagCamEo_Command();
            }else if(CMB_setEoIrMode.SelectedIndex == 1)
            {
                this._parentController._flightData.XagCamEoIr_WhiteCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 2)
            {
                this._parentController._flightData.XagCamEoIr_BlackCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 3)
            {
                this._parentController._flightData.XagCamEoIr_PseudoCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 4)
            {
                this._parentController._flightData.XagCamIrEo_WhiteCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 5)
            {
                this._parentController._flightData.XagCamIrEo_BlackCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 6)
            {
                this._parentController._flightData.XagCamIrEo_PseudoCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 7)
            {
                this._parentController._flightData.XagCamIr_WhiteCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 8)
            {
                this._parentController._flightData.XagCamIr_BlackCommand();
            }else if(CMB_setEoIrMode.SelectedIndex == 9)
            {
                this._parentController._flightData.XagCamIr_PseudoCommand();
            }
        }
        private void BtnCalculateDooaf_Click(object sender, EventArgs e)
        {
            this._parentController._flightData.calculateDooaf();
        }
        private void BtnCalculateTargetDistance_Click(object sender, EventArgs e)
        {
            this._parentController._flightData.calculateTargetDistance();
        }
    }
}
