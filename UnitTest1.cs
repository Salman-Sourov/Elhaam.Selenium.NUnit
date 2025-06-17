using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumDemo
{
    public class Tests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void LoginTest()
        {
            driver.Navigate().GoToUrl("https://elhaambd.com/login");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            // Fill in credentials
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("email")))
                .SendKeys("admin@gmail.com");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("password")))
                .SendKeys("123456789");

            // Submit form
            wait.Until(
                    ExpectedConditions.ElementToBeClickable(
                        By.XPath("//button[@type='submit' and normalize-space()='Log In']")
                    )
                )
                .Click();

            // Wait for dashboard URL
            wait.Until(driver => driver.Url.Contains("dashboard"));

            Assert.IsTrue(driver.Url.Contains("dashboard"));
        }

        [Test]
        public void InvalidLoginTest()
        {
            driver.Navigate().GoToUrl("https://elhaambd.com/login");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Enter invalid credentials
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("email")))
                .SendKeys("wronguser@gmail.com");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("password")))
                .SendKeys("wrongpassword");

            // Click the login button
            wait.Until(
                    ExpectedConditions.ElementToBeClickable(
                        By.XPath("//button[@type='submit' and normalize-space()='Log In']")
                    )
                )
                .Click();

            // Wait and check for error message (adjust selector based on actual HTML)
            IWebElement errorMessage = wait.Until(
                ExpectedConditions.ElementIsVisible(
                    By.CssSelector(".alert-danger") // Change this selector if your message has a different class
                )
            );

            // Assert error is visible
            Assert.IsTrue(errorMessage.Text.Contains("Invalid") || errorMessage.Displayed);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
