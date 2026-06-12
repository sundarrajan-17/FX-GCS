using System;
using System.Windows.Forms;
using XagSurveillanceGCS.Comms;

namespace Flasher
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new FlasherForm(args));
        }
    }
}
