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

        private int minSleepTime, maxSleepTime;

        public WebDataProvider()
        {
            this.PendingUrls = new();
            this.CompletedUrls = new();
            this.Statistics = new();
        }

        #region Properties

        public virtual Range MinSleepRange => new(30000, 90000);
        public virtual Range MaxSleepRange => new(60000, 180000);

        public virtual int MinimumAllowedSleep => 45000;

        public virtual int MaximumRandomSleep => 600000;

        public abstract SupportedTripReportSource TripReportSource { get; }

        public virtual string SourceName => this.TripReportSource.ToString();

        [XmlAttribute]
        public SupportedBrowser Browser { get; set; }

        public List<string> PendingUrls { get; set; }

        public List<string> CompletedUrls { get; set; }

        [XmlAttribute]
        public virtual int MinimumSleep
        {
            // Get runs through ForceIntoRange so that if a child class doesn't call the base
            // constructor or provide a default value, zero will not be returned.
            get => WebDataProvider.ForceIntoRange(this.minSleepTime, this.MinimumAllowedSleep, this.MaximumRandomSleep);
            set
            {
                this.minSleepTime = WebDataProvider.ForceIntoRange(value, this.MinimumAllowedSleep, this.MaximumRandomSleep);
            }
        }

        [XmlAttribute]
        public virtual int MaximumSleep
        {
            // Get runs through ForceIntoRange so that if a child class doesn't call the base
            // constructor or provide a default value, zero will not be returned.
            get => WebDataProvider.ForceIntoRange(this.maxSleepTime, this.MinimumAllowedSleep, this.MaximumRandomSleep);
            set
            {
                this.maxSleepTime = WebDataProvider.ForceIntoRange(value, this.MinimumAllowedSleep, this.MaximumRandomSleep);
            }
        }

        public ProviderStatistics Statistics { get; set; }

        [XmlAttribute]
        public string DestinationFolder { get; set; }

        [XmlAttribute]
        public PreservationRule PreservationRule { get; set; }

        public virtual bool CanRun => this.PreservationRule == PreservationRule.None || Directory.Exists(this.DestinationFolder);

        #endregion

        public static int ForceIntoRange(int value, int minimum, int maximum)
        {
            if (value < minimum)
            {
                return minimum;
            }

            if (value > maximum)
            {
                return maximum;
            }

            return value;
        }

        protected void Sleep()
        {
            int msDelay = new Random().Next(this.MinimumSleep, this.MaximumSleep);
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
