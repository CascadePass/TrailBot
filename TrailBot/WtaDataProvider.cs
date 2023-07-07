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

            WtaTripReport report = this.ParseTripReport(webDriver, uri);

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

        public WtaTripReport ParseTripReport(IWebDriver webDriver, Uri uri)
        {
            WtaTripReport report = new() { Url = uri.ToString() };

            report.ReportText = this.ParseReportText(webDriver);
            report.Title = this.ParseReportTitle(webDriver);
            report.Region = this.ParseReportRegion(webDriver);
            report.Author = this.ParseReportAuthor(webDriver);
            report.TripDate = WtaDataProvider.ParseReportDate(uri);
            report.Trails = this.ParseReportTrails(webDriver);
            report.TrailConditions = this.ParseReportConditions(webDriver);
            report.Feature = this.ParseReportFeature(webDriver);
            report.HikeType = this.FindHikeType(report);

            report.RoadConditions = this.FindHikeCondition(report, "ROAD");
            report.Bugs = this.FindHikeCondition(report, "BUGS");
            report.Snow = this.FindHikeCondition(report, "SNOW");

            return report;
        }

        private string ParseReportText(IWebDriver webDriver)
        {
            IWebElement reportBody = webDriver.FindElement(By.Id("tripreport-body-text"));
            string bodyText = reportBody?.Text;

            return bodyText;
        }

        public static DateTime ParseReportDate(Uri uri)
        {
            if (uri == null)
            {
                return DateTime.MinValue;
            }

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

        private string ParseReportTitle(IWebDriver webDriver)
        {
            IWebElement reportHeaderSection = webDriver.FindElement(By.Id("trip-report-heading"));
            string title = reportHeaderSection?.FindElement(By.ClassName("documentFirstHeading"))?.Text;

            if (title.Contains("—"))
            {
                title = title.Substring(0, title.LastIndexOf("—")).Trim();
            }

            return title;
        }

        private string ParseReportRegion(IWebDriver webDriver)
        {
            IWebElement reportHeaderSection = webDriver.FindElement(By.Id("trip-report-heading"));
            string region = reportHeaderSection?.FindElement(By.Id("hike-region")).Text;

            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(region.ToLower()); ;
        }

        private string ParseReportAuthor(IWebDriver webDriver)
        {
            IWebElement creatorInfo = webDriver.FindElement(By.ClassName("CreatorInfo"));
            IWebElement bylineText = creatorInfo?.FindElement(By.ClassName("wta-icon-headline__text"));
            string author = bylineText?.Text;

            return author;
        }

        private List<string> ParseReportTrails(IWebDriver webDriver)
        {
            List<string> found = new();
            IWebElement trailsLinks = webDriver.FindElement(By.ClassName("related-hike-links"));

            foreach (var ele in trailsLinks?.FindElements(By.TagName("a")))
            {
                found.Add(ele.Text);
            }

            return found;
        }

        private List<WtaTrailCondition> ParseReportConditions(IWebDriver webDriver)
        {
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

        private List<string> ParseReportFeature(IWebDriver webDriver)
        {
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

        private string FindHikeType(WtaTripReport tripReport)
        {
            var hikeType = tripReport.TrailConditions?.FirstOrDefault(m => string.Equals(m.Title, "TYPE OF HIKE", StringComparison.OrdinalIgnoreCase));

            if (hikeType != null)
            {
                return hikeType.Description;
            }

            return null;
        }

        private string FindHikeCondition(WtaTripReport tripReport, string conditionLabel)
        {
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
