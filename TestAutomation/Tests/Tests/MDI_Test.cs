using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.SMA;
using Framework;

namespace Tests.SmaUnitTests
{
    [TestClass]
    public class MDI_Test : SmaTest
    {

        [TestMethod]
        [TestCategory("SMA - Animation test")]
        public void MDITest()
        {
            Page.GoTo("MDI_Test");
            string[] ColorValue = ScreenPage.RoundedRectangle.Bargraph.GetColorValue("20");
            Assert.AreEqual("rgb(179,202,226)", ColorValue[0], "The color of the beginning do not match with the expected color.");

        }
    }
}
