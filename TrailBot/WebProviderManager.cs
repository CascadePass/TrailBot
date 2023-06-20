using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace CascadePass.TrailBot
{
    [Serializable]
    public class WebProviderManager
    {
        public event EventHandler<SleepEventArgs> Sleeping;
        public event EventHandler<MatchingTripReportEventArgs> FoundMatch;

        private bool isRunning;
        private int exceptionsInRowCount;

        public WebProviderManager()
        {
            this.DataProviders = new();
        }

        #region Properties

        [XmlAttribute]
        public bool IsRunning
        {
            get => this.isRunning;
            set
            {
                if (this.isRunning != value)
                {
                    if (value)
                    {
                        this.Start();
                    }

                    this.isRunning = value;
                    //this.OnPropertyChanged(nameof(this.IsRunning));
                }
            }
        }

        public bool CanStart => this.DataProviders.Any(m => m.CanRun);

        public List<WebDataProvider> DataProviders { get; set; }

        [XmlIgnore]
        public string IndexFile { get; set; }

        [XmlIgnore]
        public DateTime LastSaved { get; set; }

        [XmlIgnore]
        public List<Topic> Topics { get; set; }

        public List<MatchedTripReport> Found { get; set; }

        [XmlAttribute]
        public SupportedBrowser Browser { get; set; }

        #endregion

        public void Start()
        {
            if (this.isRunning)
            {
                return;
            }

            this.isRunning = true;

            Thread worker = new(new ThreadStart(this.GetTripReports_WorkLoop))
            {
                IsBackground = true
            };

            worker.Start();
            //this.OnPropertyChanged(nameof(this.IsRunning));
        }

        public void Stop()
        {
            this.IsRunning = false;
        }

        #region Save/Load

        #region WebProviderManager (this)

        public static WebProviderManager ReadProvidersFromFile(string filename)
        {
            WebProviderManager result;

            if (File.Exists(filename))
            {
                XmlSerializer serializer = new(typeof(WebProviderManager), new Type[] { typeof(WtaDataProvider) });
                using StreamReader streamReader = new(filename);

                result = (WebProviderManager)serializer.Deserialize(streamReader);
            }
            else
            {
                result = new();
            }

            result.AddDefaultProviders();
            result.LastSaved = DateTime.Now;

            return result;
        }

        public void SaveToFile()
        {
            //XmlWriterSettings settings = new() { OmitXmlDeclaration = true };
            XmlSerializer serializer = new(typeof(WebProviderManager), new Type[] { typeof(WtaDataProvider) });
            using StreamWriter writer = new(this.IndexFile);
            //using XmlWriter xmlWriter = XmlWriter.Create(writer, settings);

            serializer.Serialize(writer, this);
            writer.Flush();
            writer.Close();

            this.LastSaved = DateTime.Now;
        }

        #endregion

        #region TRs

        public void SaveTripReport(TripReport tripReport, string filename)
        {
            XmlSerializer serializer = new(tripReport.GetType());
            using StreamWriter writer = new(filename);

            serializer.Serialize(writer, tripReport);
            writer.Flush();
            writer.Close();
        }

        public void SaveTripReport(MatchedTripReport matchedTripReport, TripReport tripReport, WebDataProvider provider)
        {
            if (provider.PreservationRule == PreservationRule.None)
            {
                return;
            }

            Uri sourceUri = new(tripReport.Url);

            string filename = $"{Path.Combine(provider.DestinationFolder, Path.GetFileName(sourceUri.AbsolutePath))}.xmltr";

            if (matchedTripReport != null)
            {
                matchedTripReport.Filename = filename;
            }

            //TODO: Use FileStore instead of:
            this.SaveTripReport(tripReport, filename);
        }

        #endregion

        #endregion

        public void AddDefaultProviders()
        {
            if (!this.DataProviders.Any(m => m.TripReportSource == SupportedTripReportSource.WashingtonTrailsAssociation))
            {
                this.DataProviders.Add(new WtaDataProvider());
            }
        }

        public void RemoveMatch(MatchedTripReport matchedTripReport)
        {
            if (matchedTripReport == null)
            {
                return;
            }

            if (this.Found.Contains(matchedTripReport))
            {
                this.Found.Remove(matchedTripReport);
            }

            if (File.Exists(matchedTripReport.Filename))
            {
                File.Delete(matchedTripReport.Filename);
            }
        }

        private void GetTripReports_WorkLoop()
        {
            var webBrowser = WebDriverProvider.GetWebDriver();

            while (this.isRunning)
            {
                bool didAnything = false;
                foreach (var provider in this.DataProviders)
                {
                    if (provider.TripReportSource == SupportedTripReportSource.WashingtonTrailsAssociation)
                    {
                        WtaDataProvider wtaProvider = (WtaDataProvider)provider;
                        DateTime lastGetRecent = wtaProvider.LastGetRecentRequest;

                        TimeSpan ageOfWorklist = DateTime.Now - lastGetRecent;

                        if (ageOfWorklist.TotalMinutes > 90)
                        {
                            try
                            {
                                int depth = wtaProvider.LastGetRecentRequest == DateTime.MinValue ? 5 : 3 * (int)ageOfWorklist.TotalDays;

                                wtaProvider.GetRecentTripReportAddresses(webBrowser, depth);
                                lastGetRecent = DateTime.Now;
                                didAnything = true;
                                this.exceptionsInRowCount = 0;
                            }
                            catch (WebDriverException wEx)
                            {
                                this.exceptionsInRowCount += 1;
                                wtaProvider.Statistics.FailedRequests += 1;

                                if (WebDriverProvider.IsWebDriverConnectionLost(wEx))
                                {
                                    webBrowser = WebDriverProvider.RecycleWebDriver(webBrowser);
                                    this.exceptionsInRowCount += 0;
                                }
                            }
                        }

                        if (wtaProvider.PendingUrls.Count > 0)
                        {
                            try
                            {
                                WtaTripReport tr = wtaProvider.GetNextTripReport(webBrowser);
                                didAnything = true;
                                this.exceptionsInRowCount = 0;

                                if (tr != null)
                                {
                                    if (this.ProcessTripReport(tr, wtaProvider) != null)
                                    {
                                        wtaProvider.Statistics.MatchesFound += 1;
                                    }
                                }
                            }
                            catch (WebDriverException wEx)
                            {
                                this.exceptionsInRowCount += 1;

                                if (WebDriverProvider.IsWebDriverConnectionLost(wEx))
                                {
                                    webBrowser = WebDriverProvider.RecycleWebDriver(webBrowser);
                                    this.exceptionsInRowCount += 0;
                                }
                            }
                        }
                    }
                }

                if (this.exceptionsInRowCount > 50)
                {
                    webBrowser = WebDriverProvider.RecycleWebDriver(webBrowser);
                    this.exceptionsInRowCount += 0;
                }

                if ((DateTime.Now - this.LastSaved).Minutes > 5)
                {
                    this.SaveToFile();
                }

                if (!didAnything)
                {
                    this.Sleep(TimeSpan.FromSeconds(15));
                }
            }
        }

        private MatchedTripReport ProcessTripReport(TripReport tripReport, WebDataProvider dataProvider)
        {
            if (tripReport is null)
            {
                throw new ArgumentNullException(nameof(tripReport));
            }

            List<MatchInfo> matches = new();
            foreach (Topic topic in this.Topics)
            {
                var matchTerms = topic.GetMatchInfo(tripReport);
                matches.Add(matchTerms);
            }

            if (!matches.Any(m => !m.IsEmpty))
            {
                return null;
            }

            MatchedTripReport matchedTripReport = MatchedTripReport.Create(tripReport, matches);

            if (!this.Found.Any(m => string.Equals(m.SourceUri, tripReport.Url, StringComparison.Ordinal)))
            {
                this.Found.Add(matchedTripReport);
            }

            this.SaveTripReport(matchedTripReport, tripReport, dataProvider);

            this.OnMatchFound(this, new() { MatchedTripReport = matchedTripReport });
            return matchedTripReport;
        }

        internal void Sleep(TimeSpan napDuration)
        {
            this.OnSleeping(this, new() { Duration = napDuration });
            Thread.Sleep(napDuration);
        }

        internal void OnSleeping(object sender, SleepEventArgs e)
        {
            this.Sleeping?.Invoke(sender, e);
        }

        internal void OnMatchFound(object sender, MatchingTripReportEventArgs e)
        {
            this.FoundMatch?.Invoke(sender, e);
        }
    }
}
