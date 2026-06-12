using Microsoft.VisualStudio.TestTools.UnitTesting;
using XagSurveillanceGCS.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XagSurveillanceGCS.Utilities.Tests
{
    [TestClass()]
    public class DownloadTests
    {
        [TestMethod()]
        public void getFilefromNetTest()
        {
            if (Utilities.Download.getFilefromNet("https://www.google.com/", Path.GetTempFileName()))
                return;

            Assert.Fail();
        }

        [TestMethod()]
        public void CheckHTTPFileExists()
        {
            if (Utilities.Download.CheckHTTPFileExists("https://github.com/ArduPilot/XagSurveillanceGCS/releases/download/betarelease/XagSurveillanceGCSBeta.zip"))
                return;

            Assert.Fail();
        }

        [TestMethod()]
        public void GetFileSize()
        {
            if (Utilities.Download.GetFileSize("https://github.com/ArduPilot/XagSurveillanceGCS/releases/download/betarelease/XagSurveillanceGCSBeta.zip") > 0)
                return;

            Assert.Fail();
        }
    }
}