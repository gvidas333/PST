using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Selenium;

public class TestThree
{
    public static void RunTestThree()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        
        try
        {
            driver.Manage().Window.Maximize();
            
            // 1. open the website
            driver.Navigate().GoToUrl("https://demoqa.com");
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
            
            // 3. select elements tab
            IWebElement elementsLink = driver.FindElement(By.XPath("//div[contains(@class, 'card-body')]//h5[normalize-space()='Elements']"));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", elementsLink);
            elementsLink.Click();
            Console.WriteLine("Navigated to elements tab");
            
            // 4. select menu item web tables
            IWebElement webTablesLink = driver.FindElement(By.XPath("//span[contains(text(), 'Web Tables')]"));
            webTablesLink.Click();
            Console.WriteLine("Navigated to web tables");
            
            // 5. add enough items to create a second page in pagination
            IWebElement addButton = 
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(@id, 'addNewRecordButton')]")));

            for (int i = 0; i < 11; i++)
            {
                addButton.Click();
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[contains(@id, 'firstName')]")));
                
                FillRegistrationForm(driver, i);
                
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(@id, 'submit')]"))).Click();

                if (IsPaginationPresent(driver))
                {
                    Console.WriteLine("Second pagination page has appeared");
                    break;
                }
            }
            
            // 6. move to the second page
            Thread.Sleep(1000);
            IWebElement nextButton = driver.FindElement(By.XPath("//button[contains(text(), 'Next')]"));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", nextButton);
            nextButton.Click();
            Console.WriteLine("Navigated to the second page");
            
            // 7. delete an element on the second page
            IWebElement deleteButton = driver.FindElement(By.XPath("//span[contains(@title, 'Delete')]"));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", deleteButton);
            deleteButton.Click();
            Console.WriteLine("Item from the second page deleted");
            
            // 8. ensure that the pagination moves to the first page ..
            if (VerifyPaginationState(driver, wait))
            {
                Console.WriteLine("Test passed, there is one pagination page");
            }
            else
            {
                Console.WriteLine("Test failed, unexpected pagination behaviour");
            }
            
            // helper methods for 5th step
            void FillRegistrationForm(IWebDriver driver, int index)
            {
                driver.FindElement(By.XPath("//input[contains(@id, 'firstName')]")).SendKeys($"Test{index}");
                driver.FindElement(By.XPath("//input[contains(@id, 'lastName')]")).SendKeys($"User{index}");
                driver.FindElement(By.XPath("//input[contains(@id, 'userEmail')]")).SendKeys($"Test{index}@example.com");
                driver.FindElement(By.XPath("//input[contains(@id, 'age')]")).SendKeys("22");
                driver.FindElement(By.XPath("//input[contains(@id, 'salary')]")).SendKeys("2000");
                driver.FindElement(By.XPath("//input[contains(@id, 'department')]")).SendKeys("IT");
            }

            bool IsPaginationPresent(IWebDriver driver)
            {
                try
                {
                    return driver.FindElement(By.XPath("//button[contains(text(), 'Next')]")).Enabled;
                }
                catch(NoSuchElementException)
                {
                    return false;
                }
            }
            
            // helper method for 8th step
            bool VerifyPaginationState(IWebDriver driver, WebDriverWait wait)
            {
                try
                {
                    var currentPage = wait
                        .Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@class, 'pageJump')]")))
                        .GetAttribute("value");
            
                    var totalPages = driver.FindElement(By.XPath("//span[contains(@class, '-totalPages')]"))
                        .Text;
            
                    bool isCorrect = currentPage == "1" && totalPages == "1";
            
                    return isCorrect;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error verifying pagination: {ex.Message}");
                    return false;
                }
            }
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