import sys
import math
import clr
import time
clr.AddReference("XagSurveillanceGCS")
import XagSurveillanceGCS
clr.AddReference("XagSurveillanceGCS.Utilities") # includes the Utilities class

print 'Start Script'

XagSurveillanceGCS.MainV2.instance.FlightPlanner.BUT_read_Click(XagSurveillanceGCS.MainV2.instance.FlightPlanner,null)

