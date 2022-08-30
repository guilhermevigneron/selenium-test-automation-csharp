using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework;
using Framework.Selenium;
using System;
using System.Linq;

namespace Tests.Tests
{
    [TestClass]
    public class CEF : PreDefinedProperties
    {
        [TestMethod]
        [TestCategory("Windows"), TestCategory("local"), TestCategory("CEF")]
        public void CEFImageListCustomWidget_WhenRunningTheApplication_MustRenderTheExpectedImages()
        {
            //Arrange
            var url = Environment.GetEnvironmentVariable("CEFURL") ?? "http://localhost:8088";
            Page.GoToUrl(url, Common.CEFWaitPageLoad);

            var indexOfSearch = "<img id=\"container\" src=\"";
            void wait() => Common.WaitBodyContentContains(indexOfSearch, new TimeSpan(0, 0, 10));

            Common.GotoHyperlink(1, wait);

            var body = Common.GetBodyContent();
            var startIdx = body.IndexOf(indexOfSearch);
            var startText = body.Substring(startIdx + indexOfSearch.Length);
            var actualImgSrc = startText.Substring(0, startText.IndexOf('"'));
            //Act

            //Assert
            var expectedImgSrcs = new string[]
            {
                @"..\Resources\imagelist\abelskievers.jpg",
                @"..\Resources\imagelist\copenhagen-1-10.jpg",
                @"..\Resources\imagelist\DSC00860-2h7tuqw.jpg"
            };

            AssertHelper.MultiAssert.Aggregate
                (
                () => Assert.IsTrue(expectedImgSrcs.Contains(actualImgSrc), $"image src {actualImgSrc} are not in the expected imgs src")
                );
        }
    }
}