using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Miku.Client.Models.Recorders;
using System.IO;

namespace Miku.Client.Test
{
    [TestClass]
    public class SampleUnitTest
    {
        [TestMethod]
        public void SampleTestMethod()
        {
            KBActionRecorderLinq recorder = new KBActionRecorderLinq();
            recorder.SaveRecordedFileAs("SaveAsFile");

            Assert.AreEqual<bool>(File.Exists("SaveAsFile"), true);

            File.Delete("SaveAsFile");
        }
    }
}
