using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading;
using SeleniumExtras.WaitHelpers;

namespace Selenium
{
    [TestFixture]
    public class TestFour
    {
        private string userEmail;
        private string userPassword;
        private IWebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("Creating a new user...");
            (userEmail, userPassword) = UserCreation.RegisterUser();
            Console.WriteLine($"User created with email: {userEmail}");
        }

        [Test, Order(1)]
        public void TestScenario1()
        {
            Console.WriteLine("Starting test scenario 1");
            PerformTest("data1.txt");
        }

        [Test, Order(2)]
        public void TestScenario2()
        {
            Console.WriteLine("Starting test scenario 2");
            PerformTest("data2.txt");
        }

        private void PerformTest(string dataFile)
        {
            try
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // 1. open the website 
                driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
                Console.WriteLine("Website opened successfully");
                
                // 2. click log in
                driver.FindElement(By.XPath("//a[contains(@class, 'ico-login')]")).Click();
                Console.WriteLine("Clicked log in");
                
                // 3. fill in the email and password fields and click log in
                driver.FindElement(By.XPath("//input[contains(@class, 'email')]")).SendKeys(userEmail);
                driver.FindElement(By.XPath("//input[contains(@class, 'password')]")).SendKeys(userPassword);
                driver.FindElement(By.XPath("//input[contains(@class, 'button-1 login-button')]")).Click();
                Console.WriteLine("Logged in with the newly created user");
                
                // 4. select digital downloads
                driver.FindElement(By.XPath("//a[normalize-space()='Digital downloads']")).Click();
                Console.WriteLine("Navigated to digital downloads");
                
                // 5. add products to the cart by reading from a text file
                string[] productNames = File.ReadAllLines(dataFile);
                foreach (string productName in productNames)
                {
                    try
                    {
                        string productXPath = $"//a[contains(text(),'{productName}')]/../..//input[@value='Add to cart']";
                        IWebElement addToCartButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(productXPath)));
                        addToCartButton.Click();
                        Console.WriteLine($"Added product {productName} to cart");
                        
                        Thread.Sleep(1000);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error adding product {productName}: {ex.Message}");
                    }
                }
                
                // 6. open shopping cart
                driver.FindElement(By.XPath("//span[contains(@class, 'cart-label')]")).Click();
                Console.WriteLine("Navigated to shopping cart");
                
                // 7. check i agree and click continue
                driver.FindElement(By.XPath("//input[contains(@id, 'termsofservice')]")).Click();
                driver.FindElement(By.XPath("//button[contains(@id, 'checkout')]")).Click();
                Console.WriteLine("Checked I agree and clicked continue");
                
                // 8. fill in an address
                try
                {
                    IWebElement countryDropdown =
                        driver.FindElement(By.XPath("//select[@id='BillingNewAddress_CountryId']"));
                    countryDropdown.Click();
                    driver.FindElement(
                        By.XPath("//select[@id='BillingNewAddress_CountryId']/option[text()='Lithuania']")).Click();
                    driver.FindElement(By.XPath("//input[contains(@id, 'BillingNewAddress_City')]"))
                        .SendKeys("Vilnius");
                    driver.FindElement(By.XPath("//input[contains(@id, 'BillingNewAddress_Address1')]"))
                        .SendKeys("Vilniaus 11");
                    driver.FindElement(By.XPath("//input[contains(@id, 'BillingNewAddress_ZipPostalCode')]"))
                        .SendKeys("12345");
                    driver.FindElement(By.XPath("//input[contains(@id, 'BillingNewAddress_PhoneNumber')]"))
                        .SendKeys("864746346");
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[contains(@class, 'button-1 new-address-next-step-button')]"))).Click();
                    Console.WriteLine("Entered an address and clicked continue");
                }
                catch (ElementNotInteractableException)
                {
                    Console.WriteLine("Address form not shown - using existing address");
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[contains(@class, 'button-1 new-address-next-step-button')]"))).Click();
                }
                
                // 9,10,11
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[contains(@class, 'button-1 payment-method-next-step-button')]"))).Click();
                Console.WriteLine("Selected payment method and clicked continue");
                
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[contains(@class, 'button-1 payment-info-next-step-button')]"))).Click();
                Console.WriteLine("Confirmed payment info and clicked continue");
                
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[contains(@class, 'button-1 confirm-order-next-step-button')]"))).Click();
                Console.WriteLine("Confirmed order successfully");
                
                // 12.
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@class, 'title')]/strong")));
                string confirmationText = driver.FindElement(By.XPath("//div[contains(@class, 'title')]/strong")).Text;
                Assert.That(confirmationText.Trim(), Is.EqualTo("Your order has been successfully processed!"));

                Console.WriteLine("Order completed successfully with confirmation: " + confirmationText);
            }
            finally
            {
                //driver?.Quit();
                Console.WriteLine("Test run is done");
            }
        }
        
        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
            Console.WriteLine("Driver quit in teardown");
        }
    }
}