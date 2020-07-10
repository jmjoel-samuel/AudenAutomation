using System;
using AventStack.ExtentReports.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using RazorEngine.Compilation.ImpromptuInterface;
using SeleniumExtras.PageObjects;

namespace AudenAutomationTests.Pages
{
    public class ShorttermLoanPage : BasePage
    {
        private const string PageURL = "credit/shorttermloan";
        [FindsBy(How = How.CssSelector, Using = "[class='range-input loan-amount__range-slider']")]
        private readonly IWebElement slider;
        private readonly string valueSetOnSliderCssSelector = "[class='loan-amount__range-slider__input']";
        private readonly string loanValueInPoundsCssSelector = "div:nth-child(1)>div>span.loan-summary__column__amount__value";
        private readonly string loanValueInPenceCssSelector = "div:nth-child(1)>div>span.loan-summary__column__amount__decimal";
        private string dayValueFornextFriday;
        private string loanValueOnSlide;
        private string minValueOnSlider;
        private int x;
        private int y;

        public ShorttermLoanPage()
        {
            PageFactory.InitElements(Driver, this);
        }

        public void GetMinValueOnSlider()
        {
            WaitForUrlContains(PageURL);
            WaitForElementToBeClickable(slider);
            minValueOnSlider = GetValuedSetOnSlider();
            Console.WriteLine(minValueOnSlider);
            for (int i = 0; i <= 8; i++)
            {
                if (minValueOnSlider.IsNullOrEmpty() || minValueOnSlider.Equals("0"))
                {
                    WaitForElementToBeClickable(slider);
                    minValueOnSlider = GetValuedSetOnSlider();
                    Console.WriteLine(minValueOnSlider);
                }
                else
                {
                    break;
                }
            }
        }

        public void SetLoanUsingSlider(string loanValue)
        {
            WaitForElementToBeClickable(slider);
            slider.Click();
            switch (loanValue)
            {
                case "differentValueToTheDefault":
                    x = 200;
                    y = 250;
                    break;
                case "Max":
                    x = 350;
                    y = 350;
                    break;
                default:
                    Assert.Fail("No Assertion parameter available");
                    break;
            }

            try
            {
                // setting loan value to £ 450
                var act = new Actions(Driver);
                act.DragAndDropToOffset(slider, x, y).Perform();
            }
            catch (ElementNotInteractableException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void CorrectLoanAmountDisplayed()
        {
            // Get loan value from Loan field
            WaitForElementVisible(By.CssSelector(loanValueInPoundsCssSelector));
            var loanValue = Driver.FindElement(By.CssSelector(loanValueInPoundsCssSelector)).Text;

            // Getting only int values from Loan text
            loanValue = GetvalueAfter(loanValue, "£");
            WaitForElementVisible(By.CssSelector(valueSetOnSliderCssSelector));

            // Getting decimal values from Loan text
            var loneValueInPence = Driver.FindElement(By.CssSelector(loanValueInPenceCssSelector)).Text;

            // Getting Loan amount set on slider
            loanValueOnSlide = Driver.FindElement(By.CssSelector(valueSetOnSliderCssSelector)).GetAttribute("value");

            // Converting loan amount to actual int values
            var loneValueWithDecimal = decimal.Parse($"{loanValue}{loneValueInPence}");
            var loanValueOnSlideDecimal = decimal.Parse(loanValueOnSlide);

            // Asserting actual loan amount matches to the amount sent on the slider
            Assert.AreEqual(loanValueOnSlideDecimal, loneValueWithDecimal, "Loan Amount set on slider Not matches to the loan amount displayed on loan section");
            Assert.AreNotEqual(minValueOnSlider, loneValueWithDecimal.ToString(), "Loan amount displayed on loan section Matches to the default loan Amount");
        }

        public void SelectPaumentDateAsSunday()
        {
            var date = DateTime.Now;
            var nextSunday = date.AddDays(14 - (int)date.DayOfWeek);
            var nextFriday = date.AddDays(12 - (int)date.DayOfWeek);

            // getting day value for Sunday in the month
            var dayValueForNextSunday = nextSunday.ToString("dd");

            // getting date value for Friday in the month
            dayValueFornextFriday = nextFriday.ToString("yyyy-MM-dd");

            // creating Dynamic element to select payment day as Sunday
            var selectPaymentDate = Driver.FindElement(By.CssSelector($"[id='monthly'][value='{dayValueForNextSunday}']"));
            ClickTheButton(selectPaymentDate);
        }

        public void AssertPaymentDate()
        {
            // Creating Dynamic element locater for Friday as First payment date
            WaitForElementVisible(By.CssSelector($"label[class='loan-schedule__tab__panel__detail__tag__label'][for='{dayValueFornextFriday}']"));
            var firstPaymentDate = Driver.FindElement(By.CssSelector($"label[class='loan-schedule__tab__panel__detail__tag__label'][for='{dayValueFornextFriday}']"));

            // Asserting Friday is displayed as a First payment date
            Assert.IsTrue(firstPaymentDate.Displayed);
        }

        public void AssertMinMaxLimit(string minOrMaxValues, string value)
        {
            switch (minOrMaxValues)
            {
                case "Min":
                    Assert.AreEqual(value, minValueOnSlider);
                    break;
                case "Max":
                    var maxLoanValue = GetValuedSetOnSlider();
                    Assert.AreEqual(value, maxLoanValue);
                    break;
                default:
                    Assert.Fail("No Assertion parameter available");
                    break;
            }
        }

        public string GetValuedSetOnSlider()
        {
            // Getting Loan amount set on slider
            WaitForElementVisible(By.CssSelector(valueSetOnSliderCssSelector));
            loanValueOnSlide = Driver.FindElement(By.CssSelector(valueSetOnSliderCssSelector)).GetAttribute("value");
            return loanValueOnSlide;
        }
    }
}
