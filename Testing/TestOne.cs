using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class TestOne
{
    public static void RunTestOne()
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        try
        {
            driver.Manage().Window.Maximize();
            
            driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
            Console.WriteLine("Successfully opened the website");
            
            driver.FindElement(By.XPath("//div[@class='block block-category-navigation']//a[normalize-space()='Gift Cards']")).Click();
            Console.WriteLine("Navigated to gift cards page");

            var products = driver.FindElements(By.XPath("//div[contains(@class, 'product-item')]"));
            IWebElement selectedProduct = null;

            foreach (var product in products)
            {
                var priceElement = product.FindElement(By.XPath(".//span[@class='price actual-price']"));
                var priceText = priceElement.Text;

                if (Decimal.Parse(priceText, System.Globalization.CultureInfo.InvariantCulture) > 99)
                {
                    selectedProduct = product;
                    break;
                }
            }

            if (selectedProduct == null)
            {
                throw new Exception("No product with the price over 99 was found");
            }
            
            selectedProduct.FindElement(By.XPath(".//h2[@class='product-title']/a")).Click();
            Console.WriteLine("Selected a gift card with price > 99");

            var productPrice = driver.FindElement(By.XPath("//span[contains(@class, 'price-value-')]")).Text;
            Console.WriteLine($"Selected product price: {productPrice}");

            var recipientField =
                driver.FindElement(By.XPath("//label[contains(text(), 'Recipient')]/following-sibling::input"));
            recipientField.Clear();
            recipientField.SendKeys("Test Recipient");
            Console.WriteLine("Filled recipient name");

            var senderField =
                driver.FindElement(By.XPath("//label[contains(text(), 'Your Name')]/following-sibling::input"));
            senderField.Clear();
            senderField.SendKeys("Test Sender");
            Console.WriteLine("Filled sender name");

            var quantityField = driver.FindElement(By.XPath("//label[contains(@class, 'qty-label')]/following-sibling::input"));
            quantityField.Clear();
            quantityField.SendKeys("5000");
            Console.WriteLine("Quantity set to 5000");

            var addToCartButton = driver.FindElement(By.XPath("//input[contains(@class, 'add-to-cart-button')]"));
            addToCartButton.Click();
            Console.WriteLine("Clicked add to cart");
            
            Thread.Sleep(1000);

            var addToWishlistButton = driver.FindElement(By.XPath("//input[contains(@class, 'add-to-wishlist-button')]"));
            addToWishlistButton.Click(); 
            Console.WriteLine("Added to wishlist");
            
            Thread.Sleep(1000);

            var jewelryLink = driver.FindElement(By.XPath("//div[@class='block block-category-navigation']//a[normalize-space()='Jewelry']"));
            jewelryLink.Click();
            Console.WriteLine("Navigated to Jewelry page");

            var createJewelryLink = driver.FindElement(By.XPath("//a[contains(text(), 'Create Your Own Jewelry')]"));
            createJewelryLink.Click();
            Console.WriteLine("Navigated to Create Your Own Jewelry page");
            
            var materialDropdown = new SelectElement(driver.FindElement(By.XPath("//select[contains(@id, 'product_attribute_71_9_15')]")));
            materialDropdown.SelectByText("Silver (1 mm)");
            Console.WriteLine("Selected Silver 1mm material");

            var lengthInput = driver.FindElement(By.XPath("//input[contains(@id, 'product_attribute_71_10_16')]"));
            lengthInput.SendKeys("80");
            Console.WriteLine("Length set to 80cm");

            var starPendant = driver.FindElement(By.Id("product_attribute_71_11_17_50"));
            starPendant.Click();
            Console.WriteLine("Selected Star pendant");
            
            var quantityInput = driver.FindElement(By.XPath("//input[contains(@class, 'qty-input')]"));
            quantityInput.Clear();
            quantityInput.SendKeys("26");
            Console.WriteLine("Set quantity to 26");

            var addJewelryToCartButton = driver.FindElement(By.XPath("//input[contains(@class, 'add-to-cart-button')]"));
            addJewelryToCartButton.Click();
            Console.WriteLine("Added jewelry to cart");
            
            Thread.Sleep(1000);

            var addJewelryToWishlistButton = driver.FindElement(By.XPath("//input[contains(@class, 'add-to-wishlist-button')]"));
            addJewelryToWishlistButton.Click();
            Console.WriteLine("Added jewelry to wishlist");
            
            Thread.Sleep(1000);

            var wishListLink = driver.FindElement(By.XPath("//div[contains(@class, 'header-links')]//a[contains(@class, 'ico-wishlist')]"));
            wishListLink.Click();
            Console.WriteLine("Navigated to Wishlist page");

            Thread.Sleep(2000);
            
            var checkboxes = driver.FindElements(By.Name("addtocart"));
            foreach (var checkbox in checkboxes)
            {
                if (!checkbox.Selected)
                {
                    checkbox.Click();
                    Console.WriteLine("Checked a checkbox");
                }
            }

            var addSelectedToCart = driver.FindElement(By.XPath("//input[contains(@name, 'addtocartbutton')]"));
            addSelectedToCart.Click();
            Console.WriteLine("Selected items have been added to cart");

            var subtotalElement = driver.FindElement(By.XPath("//span[contains(@class, 'product-price')]"));
            var subtotal = subtotalElement.Text.Trim();
            Console.WriteLine($"Found subtotal: {subtotal}");
            
            if (subtotal == "1002600.00")
            {
                Console.WriteLine("Subtotal verification successful!");
            }
            else
            {
                Console.WriteLine($"Subtotal verification failed. Expected: 1002600.00, Found: {subtotal}");
            }
            
            Console.WriteLine("Test completed. Press any key to exit...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
        }
        finally
        {
            driver.Quit();
        }
    }
}