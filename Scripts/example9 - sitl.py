import os
import sys
import math
import clr
import time
clr.AddReference("XagSurveillanceGCS")
import XagSurveillanceGCS
clr.AddReference("XagSurveillanceGCS.Utilities") # includes the Utilities class
clr.AddReference("XagSurveillanceGCS.Comms")
clr.AddReference("System")
import XagSurveillanceGCS.Comms
import System

from System.Diagnostics import Process

for i in range(20):
	workdir = 'C:\Users\michael\Documents\XagSurveillanceGCS\sitl\d' + str(i)
	if not os.path.exists(workdir):
		os.makedirs(workdir)
	proc = Process()
	proc.StartInfo.WorkingDirectory = workdir
	proc.StartInfo.FileName ='C:\Users\michael\Documents\XagSurveillanceGCS\sitl\ArduCopter.exe'
	proc.StartInfo.Arguments	= ' -M+ -s1 --serial0 tcp:0 --defaults ..\default_params\copter.parm --instance ' + str(i) + ' --home -35.363261,'+ str(149.165330 + 0.000001 * i) +',584,353'
	proc.Start()

	port = XagSurveillanceGCS.Comms.TcpSerial();
	port.client = System.Net.Sockets.TcpClient("127.0.0.1", 5760 + 10 * i);

	mav = XagSurveillanceGCS.MAVLinkInterface();
	mav.BaseStream = port;
	mav.getHeartBeat()
	#XagSurveillanceGCS.MainV2.instance.doConnect(mav, "preset", "0");
	XagSurveillanceGCS.MainV2.Comports.Add(mav);
