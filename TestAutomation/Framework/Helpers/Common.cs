using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Framework.Selenium
{
    public static class Common
    {
        public static void EnterText(this IWebElement element, string value, bool append)
        {
            if (!append)
                element.Clear();

            element.SendKeys(value);
        }

        public static void EnterText(this IWebDriver driver, By locator, string value, bool append)
        {
            try
            {
                driver.WaitForElementToBeVisible(locator);
                IWebElement element = driver.FindElement(locator);

                if (!append)
                    element.Clear();

                element.SendKeys(value);
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("A error occurred while trying to Enter text on the {0} element. Error message: {1}", locator, e.Message));
            }
        }

        public static void SingleClick(this IWebElement element)
        {
            element.Click();
        }

        public static void SingleClick(this IWebDriver driver, By locator)
        {
            try
            {
                driver.WaitForElementToBeVisible(locator);
                IWebElement element = driver.FindElement(locator);

                //scroll to the element
                driver.ScrollToView(locator);
                element.Click();
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("A error occurred while trying to perform a single click on the {0} element. Error message: {1}", locator, e.Message));
            }
        }

        public static void ScrollTo(IWebDriver driver, int xPosition = 0, int yPosition = 0)
        {
            var js = String.Format("window.scrollTo({0}, {1})", xPosition, yPosition);
            ExecuteJavascript(driver, js);
        }

        public static IWebElement ScrollToView(this IWebDriver driver, By selector)
        {
            var element = driver.FindElement(selector);

            if (element.Location.Y > 200)
            {
                ScrollTo(driver, 0, element.Location.Y - 100); // Make sure element is in the view but below the top navigation pane
            }
            return element;
        }

        public static void ScrollToView(this IWebDriver driver, IWebElement element)
        {
            if (element.Location.Y > 200)
            {
                ScrollTo(driver, 0, element.Location.Y - 100); // Make sure element is in the view but below the top navigation pane
            }
        }

        public static void DoubleClick(this IWebElement element)
        {
            element.Click();
            element.Click();
        }

        public static IWebElement GetParent(this IWebElement element)
        {
            return element.FindElement(By.XPath(".."));
        }

        //Getting value from Dropdownlist
        public static string GetTextFromComboBox(IWebElement element)
        {
            return new SelectElement(element).AllSelectedOptions.SingleOrDefault().Text;
        }

        public static void SelectTextFromComboBox(IWebElement element, int index)
        {
            new SelectElement(element).SelectByIndex(index);
        }

        public static void SelectTextFromComboBox(IWebElement element, string text)
        {
            new SelectElement(element).SelectByText(text);
        }

        public static bool Exist(this IWebElement element)
        {
            try
            {
                return element.Displayed;
            }
            catch (NoSuchElementException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void WaitForElementToBeVisible(this IWebDriver driver, By element)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(60)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(element));
        }

        public static void WaitForElementToBeVisible(this IWebDriver driver, By element, int timeout)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(element));
        }

        /// <summary>
        /// Waits for the element to be invisible. If it is not located on the DOM, it will throw an exception
        /// </summary>
        /// <param name="driver">The selenium WebDriver used to find and navigate</param>
        /// <param name="locator">The By object used to locate the object</param>
        ///         /// <param name="timeoutInSeconds">How many it will wait for the element condition</param>
        public static void WaitForElementToBeNotVisible(this IWebDriver driver, By locator, int timeoutInSeconds = 10)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(x => !x.FindElement(locator).Displayed);
        }

        /// <summary>
        /// Waits for the element to be invisible or not located on the DOM
        /// </summary>
        /// <param name="driver">The selenium WebDriver used to find and navigate</param>
        /// <param name="locator">The By object used to locate the object</param>
        /// <param name="timeoutInSeconds">How many it will wait for the element condition</param>
        public static void WaitUntilDoNotExist(this IWebDriver driver, By locator, int timeoutInSeconds = 10)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        public static void WaitForElementToBeClickable(this IWebDriver driver, By element)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(60)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }

        public static void WaitForElementToBeClickable(this IWebDriver driver, By element, int timeout)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }

        public static void WaitForElementToBeClickable(IWebElement element)
        {
            new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }

        public static void ImplicitWait(this IWebDriver driver, int timeout = 20)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);
        }

        public static void ExplicitWait(this IWebDriver driver, By element)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(element));
        }

        public static void FluentWait(this IWebDriver driver, int timeout = 20)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(timeout);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.Message = "Element to be searched not found";
        }

        public static object ExecuteJavascript(this IWebDriver driver, string script)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            if (js == null)
            {
                throw new System.Exception("Impossible to execute double click.");
            }

            return js.ExecuteScript(script);
        }

        public static object ExecuteJavascript(this IWebDriver driver, string script, IWebElement element)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            return js.ExecuteScript(script, element);
        }

        public static void DoubleClick(string alarmRow, string contextId)
        {
            string script = $"var node = document.getElementById(\"{alarmRow}\");" +
                "if (!node){" +
                    $"var context = document.getElementById('{contextId}');" +
                    $"node = document.evaluate(\"{Common.NormalizeXPath(alarmRow)}\", context.contentDocument, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;" +
                    "}" +
                    "var evt = new MouseEvent('dblclick', { 'view': window, 'bubbles': true, 'cancelable': true });" +
                    "node.dispatchEvent(evt);";

            _ = Common.ExecuteJavascript(Driver.Instance, script);
        }

        public static string NormalizeXPath(string xPath)
        {
            if (xPath.Length <= 0 || xPath.Contains("/") == false)
            {
                return null;
            }

            StringBuilder toReturn = new StringBuilder(4096);
            string[] chunks = xPath.Split('/');
            string element = "";
            string index = "";

            foreach (string chunk in chunks)
            {
                if (chunk.Length <= 0)
                {
                    continue;
                }

                if (chunk.Contains("[") && chunk.Contains("]"))
                {
                    element = chunk.Substring(0, chunk.IndexOf("["));
                    index = chunk.Substring(chunk.IndexOf("["), chunk.IndexOf("]") - chunk.IndexOf("[") + 1);
                }
                else
                {
                    element = chunk;
                }

                toReturn.Append($"/*[local-name() = '{element}']{index}");
                index = "";
            }

            return toReturn.ToString();
        }

        public static void SetIframeFocus(string iframeId)
        {
            var iframeObject = Driver.Instance.FindElement(By.Id(iframeId));
            Driver.Instance.SwitchTo().Frame(iframeObject);
        }

        public static string GetStylePropertyValue(string style, string property_name)
        {
            string[] properties = style.Split(';');

            string[] current_property;
            foreach (var property in properties)
            {
                current_property = property.Split(':');
                if (current_property.Length == 2)
                {
                    if (property_name == current_property[0].Trim())
                        return current_property[1].Trim();
                }
            }
            return "";
        }

        public static void WaitForAttributeValue(IWebElement element, string attribute, string value, int timeoutInSeconds = 10, bool contains = false)
        {
            if (contains)
                new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds)).Until((x) => element.GetAttribute(attribute).Contains(value));
            else
                new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds)).Until((x) => element.GetAttribute(attribute) == value);
        }

        public static void WaitForAttributeValue(By elementLocator, string attribute, string value, int timeoutInSeconds = 10, bool contains = false)
        {
            if (contains)
                new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds)).Until((x) => Driver.Instance.FindElement(elementLocator).GetAttribute(attribute).Contains(value));
            else
                new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds)).Until((x) => Driver.Instance.FindElement(elementLocator).GetAttribute(attribute) == value);
        }

        /// <summary>
        /// Waits a specified time an attribute value change
        /// </summary>
        /// <param name="element">The web element to be monitored</param>
        /// <param name="attribute">The attrbiute to be monitored</param>
        /// <param name="timeout"> The specified time to wait for the change, the time must be in miliseconds</param>
        /// <param name="avoidNoValue">Set it to true if you don't want to consider blank value as a chanrge.</param>
        /// <returns></returns>
        public static bool WaitForAttributeValueChange(IWebElement element, string attribute, int timeout = 5000, bool avoidNoValue = false)
        {
            string flagStart = element.GetAttribute(attribute);
            int timeoutCount = 0;

            if (avoidNoValue)
            {
                while (element.GetAttribute(attribute) == flagStart || element.GetAttribute(attribute) == "")
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                    timeoutCount += 100;
                    if (timeoutCount >= timeout)
                        return false;
                }
            }
            else
            {
                while (element.GetAttribute(attribute) == flagStart)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                    timeoutCount += 100;
                    if (timeoutCount >= timeout)
                        return false;
                }
            }

            return true;
        }

        public static void WaitForInnerTextValue(IWebElement element, string value, int timeoutInSeconds = 5, bool contains = false)
        {
            if (contains)
                new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds)).Until((x) => element.Text.Contains(value));
            else
                new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds)).Until((x) => element.Text == value);
        }

        public static void WaitForXPathElement(IWebElement element, string xpath, int timeoutInSeconds = 20)
        {
            new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds)).Until((x) => element.FindElement(By.XPath(xpath)));
        }

        public static void WaitForIdElement(IWebElement element, int id, int timeoutInSeconds = 20)
        {
            new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds)).Until((x) => element.FindElement(By.Id(id.ToString())));
        }

        public static void WaitForCssElement(IWebElement element, string css, int timeoutInSeconds = 20)
        {
            new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds)).Until((x) => element.FindElement(By.CssSelector(css)));
        }

        //this function is used to convert time into seconds
        public static int ConvertIntoSec(string time, char Separator)
        {
            string[] str = time.Split(Separator);
            return Convert.ToInt32(str[0]) * 3600 + Convert.ToInt32(str[1]) * 60 + Convert.ToInt32(str[2].Substring(0, 2));
        }

        public static void PrintAllIds()
        {
            var idElements = Driver.Instance.FindElements(By.XPath("//*[@id]"));
            foreach (var element in idElements)
            {
                Console.WriteLine(element.TagName);
            }
        }

        public static System.Drawing.Image GetScreenImage(string screenName)
        {
            var screenImage = $"{Directory.GetCurrentDirectory()}\\" + screenName + ".png";
            Driver.Instance.TakeScreenshot().SaveAsFile(screenImage);

            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(screenImage);
            MemoryStream ms = new MemoryStream(bytes);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }

        public static void CEFWaitPageLoad()
        {
            bool waiter(IWebDriver driver)
            {
                try
                {
                    return Driver.Instance.FindElements(By.TagName("p")).Any();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            var fiveSecs = new TimeSpan(0, 0, 5);
            Page.Wait(waiter, fiveSecs);
        }

        public static void WaitBodyContentContains(string content, TimeSpan? timeout = null)
        {
            if (!string.IsNullOrEmpty(content))
            {
                Page.Wait(
                    (IWebDriver driver) => GetBodyContent().Contains(content),
                    timeout);
            }
        }

        public static void GotoHyperlink(int idx, Action wait = null)
        {
            var hyperlinks = Page.GetAllHyperlinks();

            try
            {
                var url = hyperlinks[idx].GetAttribute("href");
                Page.GoToUrl(url);
                wait?.Invoke();
            }
            catch (Exception)
            {
                throw new Exception("Could not access the CEF webpage requested");
            }
        }

        public static string GetBodyContent()
        {
            try
            {
                return Page.GetElementByTagName("body").Text;
            }
            catch (Exception)
            {
                throw new Exception("Could not get the html body content");
            }
        }
    }
}