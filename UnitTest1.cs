using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumTests
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver? _driver;
        private WebDriverWait? _wait;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run Chrome in headless mode
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage"); // To reduce memory issues in headless mode

            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [TestMethod]
        public void TestGoogleSearch()
        {
            _driver.Navigate().GoToUrl("https://www.google.com");
            Console.log("Test Started");
            // Ensure the search box is present and interactable
            var searchBox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("q")));

            // Dismiss any overlays or pop-ups that could be blocking the element
            DismissOverlays();

            // Use JavaScript to directly set the value to avoid interception issues
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value='Selenium';", searchBox);

            // Optionally, submit the form using JavaScript to avoid element interception issues
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].form.submit();", searchBox);

            // Wait for the title to contain "Selenium"
            _wait.Until(driver => driver.Title.Contains("Selenium"));

            Assert.IsTrue(_driver.Title.Contains("Selenium"), "The page title did not contain 'Selenium'");
        }

        private void DismissOverlays()
        {
            try
            {
                // Example: Dismiss Google search suggestion dropdown if it's present
                var suggestionBox = _driver.FindElement(By.CssSelector("ul[role='listbox']"));
                if (suggestionBox.Displayed)
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].style.display='none';", suggestionBox);
                }
            }
            catch (NoSuchElementException)
            {
                // No overlay found, continue
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            _driver?.Quit();
        }
    }
}
