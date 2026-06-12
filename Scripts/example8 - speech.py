import sys
import math
import clr
import time
clr.AddReference("XagSurveillanceGCS")
import XagSurveillanceGCS
clr.AddReference("XagSurveillanceGCS.Utilities") # includes the Utilities class

print 'Start Script'

XagSurveillanceGCS.MainV2.speechEnable = True

while True:
	print 'speech ...'
	XagSurveillanceGCS.MainV2.speechEngine.SpeakAsync("test " + cs.roll.ToString())
	time.sleep(1)



