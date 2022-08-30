using Framework.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.IO;

namespace Tests
{
    [TestClass]
    public class BaseTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            if (Driver.Instance == null)
            {
                Driver.Initialize(Driver.BrowserType);
            }
            else
            {
                Driver.Refresh();
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
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
        }

        public TestContext TestContext { get; set; }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Driver.Dispose();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Driver.Dispose();
        }
    }
}