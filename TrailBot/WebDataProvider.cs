using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace CascadePass.TrailBot
{
    [Serializable]
    public abstract class WebDataProvider
    {
        public event EventHandler<PageRequestEventArgs> RequestingPage;
        public event EventHandler<TripReportCompletedEventArgs> TripReportCompleted;
        public event EventHandler<SleepEventArgs> Sleeping;

        protected RandomRange sleepRange;

        public WebDataProvider()
        {
            this.PendingUrls = new();
            this.CompletedUrls = new();
            this.Statistics = new();
        }

        #region Properties

        public virtual RandomRange SleepRange => new(20000, 30000, 180000, 600000);

        public abstract SupportedTripReportSource TripReportSource { get; }

        public virtual string SourceName => this.TripReportSource.ToString();

        [XmlAttribute]
        public SupportedBrowser Browser { get; set; }

        public List<string> PendingUrls { get; set; }

        public List<string> CompletedUrls { get; set; }

        public ProviderStatistics Statistics { get; set; }

        [XmlAttribute]
        public string DestinationFolder { get; set; }

        [XmlAttribute]
        public PreservationRule PreservationRule { get; set; }

        public virtual bool CanRun => this.PreservationRule == PreservationRule.None || Directory.Exists(this.DestinationFolder);

        #endregion

        protected void Sleep()
        {
            int msDelay = new Random().Next(this.SleepRange.Minimum, this.SleepRange.Maximum);
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(msDelay);

            this.OnSleeping(this, new SleepEventArgs() { Duration = timeSpan });
            Thread.Sleep(msDelay);
        }

        protected void OnTripReportCompleted(object sender, TripReportCompletedEventArgs e)
        {
            string url = e.TripReport.Url.ToString();

            if (!this.CompletedUrls.Contains(url))
            {
                this.CompletedUrls.Add(url);
            }

            this.TripReportCompleted?.Invoke(sender, e);
        }

        protected void OnRequestingPage(object sender, PageRequestEventArgs e)
        {
            this.RequestingPage?.Invoke(sender, e);
            this.Statistics.RequestsMade += 1;
        }

        protected void OnSleeping(object sender, SleepEventArgs e)
        {
            this.Statistics.SleepTime += e.Duration;
            this.Sleeping?.Invoke(sender, e);
        }
    }
}
