using Framework.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.IO;

namespace Tests
{
    [TestClass]
    public class PreDefinedProperties
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Driver.Initialize(Driver.BrowserType);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
            {
                var screenshotPath = $"{Directory.GetCurrentDirectory()}\\{TestContext.TestName}{DateTime.Now:yyyy-MM-dd_HH-mm-ss.fffff}.png";
                Driver.Instance.TakeScreenshot().SaveAsFile(screenshotPath);
                TestContext.AddResultFile(screenshotPath);
            }

            Driver.Dispose();
        }

        public TestContext TestContext { get; set; }
    }
}