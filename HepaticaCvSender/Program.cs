using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace HepaticaCvSender;

internal class Program
{
        public static void Main(string[] args)
        {
                ChromeOptions options = new ChromeOptions();

                options.AddArgument(@"user-data-dir=C:\Users\PUTYOURWINDOWSUSERNAME\AppData\Local\Google\Chrome\User Data");
                //options.AddArgument(@"user-data-dir=C:\Users\Danil\AppData\Local\Google\Chrome\User Data");

                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");

                IWebDriver driver = new ChromeDriver(options);

                driver.Navigate().GoToUrl("https://hh.ru/search/vacancy?text=C%23&area=1&hhtmFrom=main&hhtmFromLabel=vacancy_search_line");

                Thread.Sleep(5000);

                ClickAllApplyButtonsOnCurrentPage(driver);

                driver.Quit();
        }

        private static void ClickAllApplyButtonsOnCurrentPage(IWebDriver driver)
        {
                while (true)
                {
                        driver.Navigate().GoToUrl("https://hh.ru/search/vacancy?text=C%23&area=1&hhtmFrom=main&hhtmFromLabel=vacancy_search_line");

                        string expectedUrl = driver.Url;

                        var buttons = driver.FindElements(By.XPath("//span[contains(text(), 'Откликнуться')]")).ToList();

                        for (int i = 0; i < buttons.Count - 1; i++)
                        {
                                try
                                {
                                        buttons[i].Click();

                                        Thread.Sleep(1000);

                                        try
                                        {
                                                IWebElement closeButton = driver.FindElement(By.CssSelector("button[data-qa='vacancy-response-popup-close-button']"));

                                                closeButton.Click();
                                        }
                                        catch (Exception ex)
                                        {
                                                Console.WriteLine($"Ошибка при клике на элемент: {ex.Message}");
                                        }

                                        string currentUrl = driver.Url;

                                        if (expectedUrl != currentUrl)
                                        {
                                                driver.Navigate().GoToUrl(expectedUrl);

                                                buttons = driver
                                                        .FindElements(By.XPath("//span[contains(text(), 'Откликнуться')]"))
                                                        .ToList();

                                                buttons.RemoveRange(0, i + 1);
                                        }

                                        Thread.Sleep(1000);
                                }
                                catch (Exception ex)
                                {
                                        Console.WriteLine($"Ошибка при клике на элемент: {ex.Message}");
                                }
                        }
                }
        }
}