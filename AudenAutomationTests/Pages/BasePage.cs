using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace AudenAutomationTests.Pages
{
    public class BasePage
    {
        private readonly WebDriverWait wait;

        public BasePage()
        {
            Driver = Hooks.GetDriver();
            PageFactory.InitElements(Driver, this);
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
        }

        public IWebDriver Driver { get; set; }

        public void WaitForElementVisible(By element)
        {
           wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(element));
        }

        public void WaitForElementToBeClickable(IWebElement element)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }

        public void ClickTheButton(IWebElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            WaitForElementToBeClickable(element);
            element.Click();
        }

        public void WaitForUrlContains(string element)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains(element));
        }

        public static string GetvalueAfter(string name, string seperator)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (seperator is null)
            {
                throw new ArgumentNullException(nameof(seperator));
            }

            var start = name.LastIndexOf(seperator);
            if (start == -1)
            {
                return string.Empty;
            }

            var adjustedString = start + seperator.Length;
            if (adjustedString >= name.Length)
            {
                return string.Empty;
            }

            var finalString = name.Substring(adjustedString);
            return finalString;
        }
    }
}
