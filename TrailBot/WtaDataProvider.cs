using OpenQA.Selenium;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;

namespace CascadePass.TrailBot
{
    public class WtaDataProvider : WebDataProvider
    {
        public override SupportedTripReportSource TripReportSource => SupportedTripReportSource.WashingtonTrailsAssociation;

        public override string SourceName => "WTA";

        [XmlAttribute]
        public DateTime LastTripReportRequest { get; set; }

        [XmlAttribute]
        public DateTime LastGetRecentRequest { get; set; }

        [XmlIgnore]
        public TimeSpan AgeOfLastTripReportRequest => DateTime.Now - this.LastTripReportRequest;

        [XmlIgnore]
        public TimeSpan AgeOfLastRecentReportsRequest => DateTime.Now - this.LastGetRecentRequest;

        #region GetTripReport

        public WtaTripReport GetTripReport(string uri)
        {
            return this.GetTripReport(new Uri(uri));
        }

        public WtaTripReport GetTripReport(Uri uri)
        {
            IWebDriver webDriver = WebDriverProvider.GetWebDriver(this.Browser);

            WtaTripReport result = this.GetTripReport(webDriver, uri);
            webDriver.Quit();

            return result;
        }

        public WtaTripReport GetTripReport(IWebDriver webDriver, string uri)
        {
            return this.GetTripReport(webDriver, new Uri(uri));
        }

        public WtaTripReport GetTripReport(IWebDriver webDriver, Uri uri)
        {
            if (this.AgeOfLastTripReportRequest.TotalMilliseconds < this.SleepRange.Minimum)
            {
                this.Sleep();
            }

            this.OnRequestingPage(this, new PageRequestEventArgs() { Uri = uri });

            webDriver.Navigate().GoToUrl(uri);
            this.LastTripReportRequest = DateTime.Now;

            if (WtaDataProvider.IsPageNotFound(webDriver))
            {
                this.ErrorUrls.Add(uri.ToString());
                return null;
            }

            WtaTripReport report = WtaDataProvider.ParseTripReport(webDriver, uri);

            this.OnTripReportCompleted(this, new TripReportCompletedEventArgs() { TripReport = report });

            return report;
        }

        #endregion

        #region GetRecentTripReportAddresses

        public List<string> GetRecentTripReportAddresses()
        {
            IWebDriver webDriver = WebDriverProvider.GetWebDriver(this.Browser);
            var found = this.GetRecentTripReportAddresses(webDriver, 21);

            webDriver.Quit();

            return found;
        }

        public List<string> GetRecentTripReportAddresses(int maxPages)
        {
            IWebDriver webDriver = WebDriverProvider.GetWebDriver(this.Browser);
            var found = this.GetRecentTripReportAddresses(webDriver, maxPages);

            webDriver.Quit();

            return found;
        }

        public List<string> GetRecentTripReportAddresses(IWebDriver webDriver)
        {
            return this.GetRecentTripReportAddresses(webDriver, new Uri("https://www.wta.org/go-outside/trip-reports"));
        }

        public List<string> GetRecentTripReportAddresses(IWebDriver webDriver, int maxPages)
        {
            List<string> result = new();

            result.AddRange(this.GetRecentTripReportAddresses(webDriver, new Uri("https://www.wta.org/go-outside/trip-reports")));

            if (maxPages > 1)
            {
                for (int i = 1; i < maxPages + 1; i++)
                {
                    try
                    {
                        result.AddRange(this.GetRecentTripReportAddresses(webDriver, new Uri($"https://www.wta.org/@@search_tripreport_listing?b_size=100&b_start:int={i}00")));
                    }
                    catch (WebDriverException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        System.Diagnostics.Debug.WriteLine(ex.InnerException?.Message);
                        //System.Diagnostics.Debugger.Break();
                    }
                }
            }

            return result;
        }

        public List<string> GetRecentTripReportAddresses(IWebDriver webDriver, Uri uri)
        {
            if (this.AgeOfLastRecentReportsRequest.TotalMilliseconds < this.SleepRange.Minimum)
            {
                this.Sleep();
            }

            this.OnRequestingPage(this, new PageRequestEventArgs() { Uri = uri });
            webDriver.Navigate().GoToUrl(uri);
            this.LastGetRecentRequest = DateTime.Now;

            List<string> found = new();

            foreach (var item in webDriver.FindElements(By.ClassName("listitem-title")))
            {
                IWebElement link = item.FindElement(By.TagName("a"));
                string url = new Uri(uri, link.GetAttribute("href")).ToString();

                if (!this.CompletedUrls.Contains(url))
                {
                    found.Add(url);

                    if (!this.PendingUrls.Contains(url))
                    {
                        this.PendingUrls.Add(url);
                    }
                }
            }

            return found;
        }

        #endregion

        #region Get Next

        public WtaTripReport GetNextTripReport()
        {
            IWebDriver webDriver = WebDriverProvider.GetWebDriver(this.Browser);
            WtaTripReport found = this.GetNextTripReport(webDriver);

            webDriver.Quit();

            return found;
        }

        public WtaTripReport GetNextTripReport(IWebDriver webDriver)
        {
            if (this.PendingUrls.Count == 0)
            {
                return null;
            }

            WtaTripReport found = null;
            string tripReportAddress = this.PendingUrls[0];

            try
            {
                this.PendingUrls.Remove(tripReportAddress);

                found = this.GetTripReport(webDriver, tripReportAddress);
            }
            catch (WebDriverException ex)
            {
                this.PendingUrls.Add(tripReportAddress);
                this.Statistics.FailedRequests += 1;

                throw;
            }

            return found;
        }

        #endregion

        #region Get the info from the page

        public static WtaTripReport ParseTripReport(IWebDriver webDriver, Uri uri)
        {
            #region Sanity check

            if (webDriver == null || uri == null)
            {
                return null;
            }

            #endregion

            WtaTripReport report = new() { Url = uri.ToString() };

            report.ReportText = WtaDataProvider.ParseReportText(webDriver);
            report.Title = WtaDataProvider.ParseReportTitle(webDriver);
            report.Region = WtaDataProvider.ParseReportRegion(webDriver);
            report.Author = WtaDataProvider.ParseReportAuthor(webDriver);
            report.TripDate = WtaDataProvider.ParseReportDate(webDriver);
            report.PublishDate = WtaDataProvider.ParseReportDate(uri);
            report.Trails = WtaDataProvider.ParseReportTrails(webDriver);
            report.TrailConditions = WtaDataProvider.ParseReportConditions(webDriver);
            report.Feature = WtaDataProvider.ParseReportFeature(webDriver);
            report.HikeType = WtaDataProvider.FindHikeType(report);

            report.RoadConditions = WtaDataProvider.FindHikeCondition(report, "ROAD");
            report.Bugs = WtaDataProvider.FindHikeCondition(report, "BUGS");
            report.Snow = WtaDataProvider.FindHikeCondition(report, "SNOW");

            return report;
        }

        public static bool IsPageNotFound(IWebDriver webDriver)
        {
            #region Sanity check

            if (webDriver == null)
            {
                return false;
            }

            #endregion

            try
            {
                IWebElement reportBody = webDriver.FindElement(By.ClassName("documentFirstHeading"));

                if (reportBody.Text.Contains("does not seem to exist"))
                {
                    return true;
                }

                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static string ParseReportText(IWebDriver webDriver)
        {
            #region Sanity check

            if (webDriver == null)
            {
                return null;
            }

            #endregion

            IWebElement reportBody = webDriver.FindElement(By.Id("tripreport-body-text"));
            string bodyText = reportBody?.Text;

            return bodyText;
        }

        public static DateTime ParseReportDate(Uri uri)
        {
            #region Sanity check

            if (uri == null)
            {
                return DateTime.MinValue;
            }

            #endregion

            if (uri.LocalPath.Contains("."))
            {
                string dateText = uri.LocalPath.Substring(1 + uri.LocalPath.IndexOf("."));

                if (dateText.Contains("."))
                {
                    dateText = dateText.Substring(0, dateText.IndexOf("."));
                }

                if (DateTime.TryParse(dateText, out DateTime dt))
                {
                    return dt;
                }
            }

            return default;
        }

        public static DateTime ParseReportDate(IWebDriver webDriver)
        {
            #region Sanity check

            if (webDriver == null)
            {
                return DateTime.MinValue;
            }

            #endregion

            IWebElement reportHeaderSection = webDriver.FindElement(By.Id("trip-report-heading"));
            string tripDate = reportHeaderSection?.FindElement(By.ClassName("documentFirstHeading"))?.Text;

            if (tripDate.Contains("—"))
            {
                tripDate = tripDate.Substring(1 + tripDate.LastIndexOf("—")).Trim();
            }

            if (DateTime.TryParse(tripDate, out DateTime result))
            {
                return result;
            }

            return default;
        }

        public static string ParseReportTitle(IWebDriver webDriver)
        {
            #region Sanity check

            if (webDriver == null)
            {
                return null;
            }

            #endregion

            IWebElement reportHeaderSection = webDriver.FindElement(By.Id("trip-report-heading"));
            string title = reportHeaderSection?.FindElement(By.ClassName("documentFirstHeading"))?.Text;

            if (title.Contains("—"))
            {
                title = title.Substring(0, title.LastIndexOf("—")).Trim();
            }

            return title;
        }

        public static string ParseReportRegion(IWebDriver webDriver)
        {
            #region Sanity check

            if (webDriver == null)
            {
                return null;
            }

            #endregion

            IWebElement reportHeaderSection = webDriver.FindElement(By.Id("trip-report-heading"));
            string region = reportHeaderSection?.FindElement(By.Id("hike-region")).Text;

            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(region.ToLower()); ;
        }

        public static string ParseReportAuthor(IWebDriver webDriver)
        {
            #region Sanity check

            if (webDriver == null)
            {
                return null;
            }

            #endregion

            IWebElement creatorInfo = webDriver.FindElement(By.ClassName("CreatorInfo"));
            IWebElement bylineText = creatorInfo?.FindElement(By.ClassName("wta-icon-headline__text"));
            string author = bylineText?.Text;

            return author;
        }

        public static List<string> ParseReportTrails(IWebDriver webDriver)
        {
            #region Sanity check

            if (webDriver == null)
            {
                return null;
            }

            #endregion

            List<string> found = new();
            IWebElement trailsLinks = webDriver.FindElement(By.ClassName("related-hike-links"));

            foreach (var ele in trailsLinks?.FindElements(By.TagName("a")))
            {
                found.Add(ele.Text);
            }

            return found;
        }

        public static List<WtaTrailCondition> ParseReportConditions(IWebDriver webDriver)
        {
            #region Sanity check

            if (webDriver == null)
            {
                return null;
            }

            #endregion

            char[] breaks = new char[] { '\r', '\n' };
            List<WtaTrailCondition> found = new();
            IWebElement trailConditions = webDriver.FindElement(By.Id("trip-conditions"));

            foreach (var ele in trailConditions?.FindElements(By.ClassName("trip-condition")))
            {
                string rawText = ele.Text;
                WtaTrailCondition condition = new() { Text = rawText };

                if (rawText.Contains('\r') || rawText.Contains('\n'))
                {
                    condition.Title = rawText.Substring(0, rawText.IndexOfAny(breaks)).Trim();
                    condition.Description = rawText.Substring(rawText.IndexOfAny(breaks)).Trim();

                    condition.Text = $"{condition.Title}: {condition.Description}";
                }

                found.Add(condition);
            }

            return found;
        }

        public static List<string> ParseReportFeature(IWebDriver webDriver)
        {
            #region Sanity check

            if (webDriver == null)
            {
                return null;
            }

            #endregion

            List<string> found = new();

            try
            {
                IWebElement tripHighlights = webDriver.FindElement(By.ClassName("wta-icon-list"));

                foreach (var ele in tripHighlights?.FindElements(By.ClassName("wta-icon")))
                {
                    found.Add(ele.Text);
                }
            }
            catch (NoSuchElementException)
            {
            }

            return found;
        }

        public static string FindHikeType(WtaTripReport tripReport)
        {
            return WtaDataProvider.FindHikeCondition(tripReport, "TYPE OF HIKE");
        }

        public static string FindHikeCondition(WtaTripReport tripReport, string conditionLabel)
        {
            #region Sanity check

            if (tripReport == null || string.IsNullOrWhiteSpace(conditionLabel))
            {
                return null;
            }

            #endregion

            var result = tripReport.TrailConditions?.FirstOrDefault(m => string.Equals(m.Title, conditionLabel, StringComparison.OrdinalIgnoreCase));

            if (result != null)
            {
                return result.Description;
            }

            return null;
        }

        #endregion
    }
}
