using System;
using XagSurveillanceGCS.Comms;

namespace XagSurveillanceGCS.Radio
{
    public static class ComPort
    {
        public static ICommsSerial GetComPortForSiKRadio()
        {
            return SikRadio.Config.comPort;
        }

        public static void FinishedWithComPortForSiKRadio()
        {
        }
    }

}