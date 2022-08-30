using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;

namespace Framework.Selenium
{
    public class Driver
    {
        public static IWebDriver Instance { get; internal set; }

        public static string BaseAddress
        {
            get { return "https://www.saucedemo.com/"; }
        }

        public static string BrowserType
        {
            get { return Environment.GetEnvironmentVariable("Browser"); }
        }

        public static void Initialize(string BrowserType, bool isMaximized = true)
        {
            var DriverPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            switch (BrowserType)
            {
                case "Chrome":

                    ChromeOptions ChromeOptions = new ChromeOptions();

                    ChromeOptions.AddArguments(
                        "--ignore-ssl-errors=yes",
                        "--ignore-certificate-errors"
                        );

                    if (isMaximized)
                        ChromeOptions.AddArgument("start-maximized");

                    Instance = new ChromeDriver(DriverPath, ChromeOptions);

                    break;

                case "ChromeHeadless":

                    ChromeOptions ChromeHeadlessOptions = new ChromeOptions();

                    ChromeHeadlessOptions.AddArguments(
                        "no-proxy-server",
                        "headless",
                        "--ignore-ssl-errors=yes",
                        "--ignore-certificate-errors",
                        "--window-size=1920,1080",
                        "aggressive-cache-discard");
                    Instance = new ChromeDriver(DriverPath, ChromeHeadlessOptions);
                    break;

                case "Edge":

                    EdgeOptions edgeOptions = new EdgeOptions();
                    edgeOptions.AddArguments(
                        "no-proxy-server",
                        "incognito",
                        "--window-size=1920,1080",
                        "aggressive-cache-discard");

                    if (isMaximized)
                        edgeOptions.AddArgument("start-maximized");

                    Instance = new EdgeDriver(edgeOptions);
                    break;

                default:

                    ChromeOptions ChromeOptionsDefault = new ChromeOptions();

                    ChromeOptionsDefault.AddArguments(
                        "--ignore-ssl-errors=yes",
                        "--ignore-certificate-errors"
                        );

                    Instance = new ChromeDriver(DriverPath, ChromeOptionsDefault);

                    break;
            }
        }

        public static void Close()
        {
            if (Instance != null)
                Instance.Close();
            else
                throw new Exception("When using the driver class you must call initialize function!");
        }

        public static void Quit()
        {
            if (Instance != null)
                Instance.Quit();
            else
                throw new Exception("When using the driver class you must call initialize function!");
        }

        public static void Dispose()
        {
            if (Instance != null)
                Instance.Dispose();
            else
                throw new Exception("When using the driver class you must call initialize function!");
        }

        public static void CleanupDriver()
        {
            Instance.Quit();
            Instance.Dispose();
        }

        public static void Refresh()
        {
            Instance.Navigate().Refresh();
        }

        public static void WaitUntilExist(By locator, int timeout = 90)
        {
            new WebDriverWait(Instance, TimeSpan.FromSeconds(timeout)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
        }

        public static void WaitElementToBeClickable(By locator)
        {
            new WebDriverWait(Instance, TimeSpan.FromSeconds(90)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        public static void WaitUntilIsVisible(By locator)
        {
            new WebDriverWait(Instance, TimeSpan.FromSeconds(90)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        public static void IgnoreExceptionTypes(params Type[] exceptionTypes)
        {
            new WebDriverWait(Instance, TimeSpan.FromSeconds(90)).IgnoreExceptionTypes(exceptionTypes);
        }

        public static void DeleteAllCookies()
        {
            Instance.Manage().Cookies.DeleteAllCookies();
        }
    }
}