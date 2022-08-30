using Framework;
using Framework.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Tests
{
    [TestClass]
    public class SauceDemo : PreDefinedProperties
    {
        [TestMethod]
        [TestCategory("Login")]
        public void LoginTest()
        {
            Page.GoToUrl("https://www.saucedemo.com/");
            Page.Logon("standard_user", "secret_sauce");

            Assert.IsTrue(Page.Inventory.Exist());
        }

        [TestMethod]
        [TestCategory("Buy_Product")]
        public void BuyProductTest()
        {
            Page.GoToUrl("https://www.saucedemo.com/");
            Page.Logon("standard_user", "secret_sauce");

            Page.Products.AddToCart("add-to-cart-sauce-labs-backpack");
            Page.Cart.Click();
            Page.Cart.Checkout.Click();
            Page.Cart.Checkout.FillInformation("John", "Oliver", "78660");

            Page.Cart.Checkout.Continue.Click();

            Page.Cart.Checkout.Finish.Click();

            Assert.IsTrue(Driver.Instance.FindElement(By.XPath("//h2[contains(text(),'THANK YOU FOR YOUR ORDER')]")).Exist());
        }
    }
}