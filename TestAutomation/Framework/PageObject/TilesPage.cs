using OpenQA.Selenium;

namespace Framework
{
    public class TilesPage
    {
        public static bool IsLogged
        {
            get
            {
                var page = DriverManager.Instance.FindElements(By.Id("_x_ma_view_view")).Count > 0;
                if (page)
                    return true;
                return false;
            }
        }
    }
}