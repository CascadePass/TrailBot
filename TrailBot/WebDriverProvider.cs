using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using System;

namespace CascadePass.TrailBot
{
    public static class WebDriverProvider
    {
        #region GetWebDriver

        public static IWebDriver GetWebDriver()
        {
            return WebDriverProvider.GetWebDriver(SupportedBrowser.ValueNotSet);
        }

        public static IWebDriver GetWebDriver(SupportedBrowser browser)
        {
            switch (browser)
            {
                case SupportedBrowser.Firefox:
                    return WebDriverProvider.GetFirefoxDriver();

                case SupportedBrowser.Edge:
                    return WebDriverProvider.GetEdgeDriver();

                case SupportedBrowser.InternetExplorer:
                    return WebDriverProvider.GetInternetExplorerDriver();

                case SupportedBrowser.Safari:
                    return WebDriverProvider.GetSafariDriver();

                case SupportedBrowser.ValueNotSet:
                case SupportedBrowser.Chrome:
                default:
                    return WebDriverProvider.GetChromeDriver();
            };
        }

        #endregion

        #region Get specific drivers

        public static ChromeDriver GetChromeDriver()
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            ChromeOptions chromeOptions = new();
            chromeOptions.AddArgument("--headless=new");

            chromeDriverService.HideCommandPromptWindow = true;

            var result = new ChromeDriver(chromeDriverService, chromeOptions);

            result.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);

            return result;
        }

        public static FirefoxDriver GetFirefoxDriver()
        {
            FirefoxOptions firefoxOptions = new();
            firefoxOptions.AddArgument("--headless=new");

            var result = new FirefoxDriver(firefoxOptions);

            result.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);

            return result;
        }

        public static EdgeDriver GetEdgeDriver()
        {
            EdgeOptions edgeOptions = new();
            edgeOptions.AddArgument("--headless=new");

            var result = new EdgeDriver(edgeOptions);

            result.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);

            return result;
        }

        public static InternetExplorerDriver GetInternetExplorerDriver()
        {
            InternetExplorerOptions ieOptions = new();

            var result = new InternetExplorerDriver(ieOptions);

            result.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);

            return result;
        }

        public static SafariDriver GetSafariDriver()
        {
            SafariOptions safariOptions = new();

            var result = new SafariDriver(safariOptions);
            result.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);

            return result;
        }

        #endregion

        /// <summary>
        /// Closes a <see cref="WebDriver"/>, then launches another of
        /// the same type.
        /// </summary>
        /// <param name="webDriver">The <see cref="WebDriver"/> to recycle.</param>
        /// <returns>A new <see cref="WebDriver"/> targetting the same browser.</returns>
        public static IWebDriver RecycleWebDriver(IWebDriver webDriver)
        {
            try
            {
                webDriver.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            SupportedBrowser browserType = SupportedBrowser.ValueNotSet;

            if (webDriver is ChromeDriver)
            {
                browserType = SupportedBrowser.Chrome;
            } else if (webDriver is FirefoxDriver)
            {
                browserType = SupportedBrowser.Firefox;
            }
            else if (webDriver is InternetExplorerDriver)
            {
                browserType = SupportedBrowser.InternetExplorer;
            }
            else if (webDriver is SafariDriver)
            {
                browserType = SupportedBrowser.Safari;
            }
            else if (webDriver is EdgeDriver)
            {
                browserType = SupportedBrowser.Edge;
            }

            return WebDriverProvider.GetWebDriver(browserType);
        }

        /// <summary>
        /// Gets a value indicating whether a <see cref="WebDriverException"/> is
        /// a result of a lost or broken connection.
        /// </summary>
        /// <param name="webDriverException">An exception to check.</param>
        /// <returns>True if the connection has been broken, false otherwise.</returns>
        /// <remarks>
        /// A <see cref="WebDriver"/> with a broken connection will never recover.  A
        /// new driver must replace it for anything to get done.  Use
        /// <see cref="RecycleWebDriver(IWebDriver)"/> to attempt to close a lost
        /// driver and get a new one in its place.
        /// </remarks>
        public static bool IsWebDriverConnectionLost(WebDriverException webDriverException)
        {
            if (webDriverException == null)
            {
                return false;
            }

            return webDriverException.Message.StartsWith("disconnected: not connected to DevTools");
        }

    }
}
