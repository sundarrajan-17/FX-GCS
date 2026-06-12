using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XagSurveillanceGCS.Utilities;
using XagSurveillanceGCS.Controls;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using XagSurveillanceGCS;
using System.Drawing;
using GMap.NET.WindowsForms;
using XagSurveillanceGCS.GCSViews;
using XagSurveillanceGCS.Maps;
using XagSurveillanceGCS.Comms;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace menucleanup
{
    public class menus : XagSurveillanceGCS.Plugin.Plugin
    {
        public override string Name
        {
            get { return "menucleanup"; }
        }

        public override string Version
        {
            get { return "0.10"; }
        }

        public override string Author
        {
            get { return "Example"; }
        }

        public override bool Init()
        {
            return false;
        }

        public override bool Loaded()
        {
            loopratehz = 5;
            return true;
        }


        public override bool Loop()
        {
            Host.FDGMapControl.BeginInvokeIfRequired(() =>
            {
                var items = FlightData.instance.contextMenuStripMap.Items.GetEnumerator().ToEnumerable<ToolStripItem>()
                    .ToArray();

                string[] allowlist = new[] { "goHereToolStripMenuItem", "flyToHereAltToolStripMenuItem", "flyToCoordsToolStripMenuItem", "triggerCameraToolStripMenuItem" };

                foreach (var item in items)
                {
                    if(allowlist.Any(a=>a.Equals(item.Name)))
                        continue;

                    FlightData.instance.contextMenuStripMap.Items.Remove(item);
                }
            });

            return true;
        }

        public override bool Exit()
        {
            return true;
        }
    }
}