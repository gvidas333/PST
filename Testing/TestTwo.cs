using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;

namespace Selenium;

class TestTwo
{
    
    public static void RunTestTwo()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        try
        {
            driver.Manage().Window.Maximize();
            
            // 1. open the website
            driver.Navigate().GoToUrl("http://demoqa.com");
            Console.WriteLine("Successfully opened the website");
            
            // 2. close the cookies consent window
            try
            {
                IWebElement cookieConsent = driver.FindElement(By.XPath("//div[contains(@class, 'cookie-consent')]"));
                if (cookieConsent.Displayed)
                {
                    IWebElement acceptCookies = driver.FindElement(By.XPath("//button[contains(text(), 'Accept')]"));
                    acceptCookies.Click();
                    Console.WriteLine("Cookies accepted");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("No cookie consent window present");
            }
            
            // 3. select widgets tab
            IWebElement widgetsLink = driver.FindElement(By.XPath("//div[contains(@class, 'card-body')]//h5[normalize-space()='Widgets']"));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", widgetsLink);
            widgetsLink.Click();
            Console.WriteLine("Navigated to widgets tab");

            // 4. select progress bar
            IWebElement progressBarLink = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//span[text()='Progress Bar']")));
            
            js.ExecuteScript("window.scrollBy(0, 100)"); // scrolls down 200 pixels
            
            progressBarLink.Click();
            Console.WriteLine("Navigated to progress bar");
            
            // 5. click start
            IWebElement startButton = driver.FindElement(By.XPath("//button[contains(@id, 'startStopButton')]"));
            startButton.Click();
            Console.WriteLine("Start button clicked");
            
            // 6. wait until 100% and click reset
            IWebElement progressBar = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@class, 'progress-bar bg-info')]")));

            wait.Until(driver =>
            {
                var progressValue = progressBar.GetAttribute("aria-valuenow");
                return progressValue == "100";
            });

            IWebElement resetButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(@id, 'resetButton')]")));
            resetButton.Click();
            Console.WriteLine("Reset clicked");
            
            // 7. verify that progress bar is at 0%
            wait.Until(d => {
                var value = progressBar.GetAttribute("aria-valuenow");
                return value == "0";
            });
            
            var progressValue = progressBar.GetAttribute("aria-valuenow");
            if (progressValue == "0")
                Console.WriteLine("Progress bar is at 0%");
            else 
                Console.WriteLine("Progress bar is not at 0%");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            driver.Quit();
        }
    }
}