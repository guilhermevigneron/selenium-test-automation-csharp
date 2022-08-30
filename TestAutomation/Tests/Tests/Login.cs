using Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Login : PreDefinedProperties
    {
        [TestMethod]
        [TestCategory("Login")]
        public void LoginTest()
        {
            Page.GoToUrl("https://www.saucedemo.com/");
            Page.Login.Go("standard_user", "secret_sauce");

            Assert.IsTrue(Page.Inventory.Exist());
        }
    }
}