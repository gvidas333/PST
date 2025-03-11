using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Selenium;

public class UserCreation
{
    public static (string email, string password) RegisterUser()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        string uniqueEmail = $"test_{DateTime.Now.Ticks}@example.com";
        string password = "password";
        
        try
        {
            driver.Manage().Window.Maximize();

            // 1. open the website
            driver.Navigate().GoToUrl("https://demowebshop.tricentis.com");
            Console.WriteLine("Successfully opened the website");

            // 2. click log in
            IWebElement logInButton = driver.FindElement(By.XPath("//a[contains(@class, 'ico-login')]"));
            logInButton.Click();
            Console.WriteLine("Log in button clicked");

            // 3. click register in the new client section
            IWebElement registerNavigation =
                driver.FindElement(By.XPath("//input[contains(@class, 'button-1 register-button')]"));
            registerNavigation.Click();
            Console.WriteLine("Register button clicked");

            //4. fill in the registration form fields
            IWebElement selectGender = driver.FindElement(By.XPath("//input[contains(@id,'gender-male')]"));
            selectGender.Click();

            IWebElement firstNameField = driver.FindElement(By.XPath("//input[contains(@id, 'FirstName')]"));
            firstNameField.SendKeys("firstname");

            IWebElement lastNameField = driver.FindElement(By.XPath("//input[contains(@id, 'LastName')]"));
            lastNameField.SendKeys("lastname");

            IWebElement emailField = driver.FindElement(By.XPath("//input[contains(@id, 'Email')]"));
            emailField.SendKeys(uniqueEmail);

            IWebElement passwordField = driver.FindElement(By.XPath("//input[contains(@id, 'Password')]"));
            passwordField.SendKeys(password);

            IWebElement confirmPasswordField =
                driver.FindElement(By.XPath("//input[contains(@id, 'ConfirmPassword')]"));
            confirmPasswordField.SendKeys(password);

            // 5. click register
            IWebElement registerButton = driver.FindElement(By.XPath("//input[contains(@id, 'register-button')]"));
            registerButton.Click();
            Console.WriteLine("User registered");

            // 6. click continue
            IWebElement continueButton =
                driver.FindElement(By.XPath("//input[contains(@class, 'button-1 register-continue-button')]"));
            continueButton.Click();

            Console.WriteLine("User was created successfully");

            return (uniqueEmail, password);
        }
        finally
        {
            driver.Quit();
        }
    }
}