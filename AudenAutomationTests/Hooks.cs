using System;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using AudenAutomationTests.Helper;
using TechTalk.SpecFlow;

namespace AudenAutomationTests
{
    [Binding]
    public sealed class Hooks
    {
        private static readonly string BrowserType = ConfigHelper.AppSetting("browserType");
        private static readonly string AudenUrl = ConfigHelper.AppSetting("AudenUrl");
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static AventStack.ExtentReports.ExtentReports extent;
        private static IWebDriver driver;

        public static IWebDriver GetDriver()
        {
            return driver;
        }

        public static Uri GetTestUrl()
        {
            return new Uri(AudenUrl);
        }

        public static void GetUrl()
        {
            driver.Navigate().GoToUrl(GetTestUrl());
        }

        [BeforeTestRun]

        public static void BeforeTestRun()
        {
            string path1 = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            string path = path1 + @"Report\";
            ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(path);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            extent = new AventStack.ExtentReports.ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            // Create dynamic feature name.
            featureName = extent.CreateTest<Feature>("Feature: " + FeatureContext.Current.FeatureInfo.Title);
            Console.WriteLine("BeforeFeature");
        }

        [BeforeScenario]
        public static void BeforeScenario()
        {
            driver = BrowserType switch
            {
                "Firefox" => new FirefoxDriver(),
                _ => new ChromeDriver(),
            };

            Console.WriteLine(BrowserType);
            driver.Manage().Window.Maximize();

            GetUrl();
            Console.WriteLine("BeforeScenario");
            scenario = featureName.CreateNode<Scenario>("Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        }

        [AfterStep]
        public static void InsertReportingSteps()
        {
            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            if (ScenarioContext.Current.TestError == null)
            {
                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.StepInstance.Keyword + ScenarioStepContext.Current.StepInfo.Text);
                }
                else if (stepType == "When")
                {
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.StepInstance.Keyword + ScenarioStepContext.Current.StepInfo.Text);
                }
                else if (stepType == "Then")
                {
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.StepInstance.Keyword + ScenarioStepContext.Current.StepInfo.Text);
                }
                else if (stepType == "And")
                {
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.StepInstance.Keyword + ScenarioStepContext.Current.StepInfo.Text);
                }
            }
            else if (ScenarioContext.Current.TestError != null)
            {
                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.StepInstance.Keyword + ScenarioStepContext.Current.StepInfo.Text)
                        .Fail(ScenarioContext.Current.TestError.InnerException + ScenarioContext.Current.TestError.StackTrace, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenshot()).Build());
                }
                else if (stepType == "When")
                {
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.StepInstance.Keyword + ScenarioStepContext.Current.StepInfo.Text)
                        .Fail(ScenarioContext.Current.TestError.InnerException + ScenarioContext.Current.TestError.StackTrace, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenshot()).Build());
                }
                else if (stepType == "Then")
                {
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.StepInstance.Keyword + ScenarioStepContext.Current.StepInfo.Text)
                        .Fail(ScenarioContext.Current.TestError.InnerException + ScenarioContext.Current.TestError.StackTrace, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenshot()).Build());
                }
                else if (stepType == "And")
                {
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.StepInstance.Keyword + ScenarioStepContext.Current.StepInfo.Text)
                        .Fail(ScenarioContext.Current.TestError.InnerException + ScenarioContext.Current.TestError.StackTrace, MediaEntityBuilder.CreateScreenCaptureFromPath(TakeScreenshot()).Build());
                }
            }
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            Console.WriteLine("AfterScenario");
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Close();
            driver.Dispose();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Flush report once test completes
            extent.Flush();
        }

        public static string TakeScreenshot()
        {
            string name = "FailStep_Screenshot";
            var date = DateTime.Now;
            string path1 = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            string path = $"{path1}Report\\{name}{date:dd-MM-yyyy-HHmmss}.png";
            Screenshot screenshot = ((ITakesScreenshot)GetDriver()).GetScreenshot();
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
            return path;
        }
    }
}