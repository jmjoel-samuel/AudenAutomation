using AudenAutomationTests.Pages;
using TechTalk.SpecFlow;

namespace AudenAutomationTests.Steps
{
    [Binding]
    public class SettingLoansSteps
    {
        private readonly ShorttermLoanPage shorttermLoanPage = new ShorttermLoanPage();

        [Given(@"slider by default is set to Min loan value")]
        public void GivenSliderByDefaultIsSetToMinLoanValue()
        {
            shorttermLoanPage.GetMinValueOnSlider();
        }

        [When(@"user set loan amount using slider to ""(.*)""")]
        public void WhenUserSetLoanAmountUsingSliderTo(string loanValue)
        {
            shorttermLoanPage.SetLoanUsingSlider(loanValue);
        }

        [Then(@"the loan amount should matches the amount set on slider")]
        public void ThenTheLoanAmountShouldMatchesTheAmountSetOnSlider()
        {
            shorttermLoanPage.CorrectLoanAmountDisplayed();
        }

        [When(@"select payment date as Sunday date")]
        public void WhenSelectPaymentDateAsSundayDate()
        {
            shorttermLoanPage.SelectPaumentDateAsSunday();
        }

        [Then(@"Friday date is suggested as First repayment date")]
        public void ThenFridayDateIsSuggestedAsFirstRepaymentDate()
        {
            shorttermLoanPage.AssertPaymentDate();
        }

        [Then(@"'(.*)' loan amount value should be '(.*)' on the slider")]
        public void ThenLoanAmountValueShouldBeOnTheSlider(string minOrMaxValues, string value)
        {
            shorttermLoanPage.AssertMinMaxLimit(minOrMaxValues, value);
        }
    }
}
