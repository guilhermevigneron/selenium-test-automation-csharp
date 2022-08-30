using Framework.Selenium;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading;

namespace Framework
{
    public class Page
    {
        public static void Logon(string username, string password)
        {
            Driver.Instance.Navigate().GoToUrl(Driver.BaseAddress);
            Login.UserName.Set(username);
            Login.Password.Set(password);
            Login.Click();
        }

        public class Inventory
        {
            public static bool Exist()
            {
                return Common.Exist(Driver.Instance.FindElement(By.Id("inventory_container")));
            }
        }

        public class Products
        {
            public static IWebElement GetElement(string id)
            {
                Common.WaitForElementToBeVisible(Driver.Instance, By.Id(id));
                return Driver.Instance.FindElement(By.Id(id));
            }

            public static void AddToCart(string id)
            {
                Common.WaitForElementToBeVisible(Driver.Instance, By.Id(id));
                Driver.Instance.FindElement(By.Id(id)).Click();
            }
        }

        public class Login
        {
            public class UserName
            {
                public static IWebElement GetElement()
                {
                    Common.WaitForElementToBeVisible(Driver.Instance, By.Id("user-name"));
                    return Driver.Instance.FindElement(By.Id("user-name"));
                }

                public static void Set(string userName)
                {
                    IWebElement field = GetElement();
                    field.SendKeys(userName);
                }
            }

            public class Password
            {
                public static IWebElement GetElement()
                {
                    Common.WaitForElementToBeVisible(Driver.Instance, By.Id("password"));
                    return Driver.Instance.FindElement(By.Id("password"));
                }

                public static void Set(string password)
                {
                    IWebElement field = GetElement();
                    field.SendKeys(password);
                }
            }

            public static void Click()
            {
                Common.WaitForElementToBeVisible(Driver.Instance, By.Id("login-button"));
                Driver.Instance.FindElement(By.Id("login-button")).Click();
            }
        }

        public class Menu
        {
            public static IWebElement GetElement()
            {
                return Driver.Instance.FindElement(By.Id("react-burger-menu-btn"));
            }
        }

        public class Cart
        {
            public static IWebElement GetElement()
            {
                return Driver.Instance.FindElement(By.Id("shopping_cart_container"));
            }

            public class Continue_Shopping
            {
                public static IWebElement GetElement()
                {
                    return Driver.Instance.FindElement(By.Id("continue-shopping"));
                }
            }

            public class Checkout
            {
                public static IWebElement GetElement()
                {
                    return Driver.Instance.FindElement(By.Id("checkout"));
                }

                public static void Click()
                {
                    GetElement().Click();
                }

                public static void FillInformation(string first_name, string last_name, string zip)
                {
                    Driver.Instance.FindElement(By.Id("first-name")).SendKeys(first_name);
                    Driver.Instance.FindElement(By.Id("last-name")).SendKeys(last_name);
                    Driver.Instance.FindElement(By.Id("postal-code")).SendKeys(zip);
                }

                public class Continue
                {
                    public static void Click()
                    {
                        Driver.Instance.FindElement(By.Id("continue")).Click();
                    }
                }

                public class Finish
                {
                    public static void Click()
                    {
                        Driver.Instance.FindElement(By.Id("finish")).Click();
                    }
                }
            }

            public class Remove
            {
                public static IWebElement GetElement(string id)
                {
                    return Driver.Instance.FindElement(By.Id(id));
                }
            }

            public static void Click()
            {
                Common.WaitForElementToBeVisible(Driver.Instance, By.Id("shopping_cart_container"));
                GetElement().Click();
            }
        }

        public static void Wait(Func<IWebDriver, bool> waiter = null, TimeSpan? timeout = null)
        {
            timeout = timeout ?? new TimeSpan(0, 0, 10);
            var timeoutDatetime = DateTime.Now.Add(timeout.Value);
            while (DateTime.Now <= timeoutDatetime && waiter(Driver.Instance) != true)
            { }
        }

        public static ReadOnlyCollection<IWebElement> GetAllHyperlinks()
            => Driver.Instance.FindElements(By.TagName("a"));

        public static IWebElement GetElementByTagName(string tagName)
            => Driver.Instance.FindElement(By.TagName(tagName));

        public static object FindElementById(string id)
        {
            var byId = Driver.Instance.FindElement(By.Id(id));
            return byId;
        }

        public static void WaitForSeconds(int seconds)
        {
            int miliseconds = seconds * 1000;
            Thread.Sleep(miliseconds);
        }

        public static void Refresh()
        {
            Driver.Refresh();
        }

        public static void GoToUrl(string urlPath)
        {
            Driver.Instance.Navigate().GoToUrl(urlPath);
        }

        public static void GoToUrl(string urlPath, Action wait)
        {
            GoToUrl(urlPath);
            wait?.Invoke();
        }

        public static void SetScreenSize(int width, int height)
        {
            Driver.Instance.Manage().Window.Size = new Size(width, height);
        }
    }
}