using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Serialization;

namespace CascadePass.TrailBot
{
    /// <summary>
    /// Abstract base class providing core functionality for collecting data
    /// from a web page.
    /// </summary>
    [Serializable]
    public abstract class WebDataProvider
    {
        #region Events

        /// <summary>
        /// Raised when a <see cref="WebDataProvider"/> is about to request a web page.
        /// </summary>
        public event EventHandler<PageRequestEventArgs> RequestingPage;

        /// <summary>
        /// Raised when a trip report has finished being processed.
        /// </summary>
        public event EventHandler<TripReportCompletedEventArgs> TripReportCompleted;

        /// <summary>
        /// Raised when the <see cref="WebDataProvider"/> goes to sleep.
        /// </summary>
        public event EventHandler<SleepEventArgs> Sleeping;

        #endregion

        protected RandomRange sleepRange;

        public WebDataProvider()
        {
            this.PendingUrls = new();
            this.CompletedUrls = new();
            this.ErrorUrls = new();
            this.Statistics = new();
        }

        #region Properties

        public virtual RandomRange SleepRange => new(20000, 30000, 180000, 600000);

        /// <summary>
        /// Gets the data source a provider is able to collect from.
        /// </summary>
        public abstract SupportedTripReportSource TripReportSource { get; }

        /// <summary>
        /// Gets the name of the data source, this is the same information
        /// returned by <see cref="TripReportSource"/>.
        /// </summary>
        public virtual string SourceName => this.TripReportSource.ToString();

        /// <summary>
        /// Gets or sets the <see cref="SupportedBrowser"/> to use to collect
        /// information from the web.
        /// </summary>
        [XmlAttribute]
        public SupportedBrowser Browser { get; set; }

        public List<string> PendingUrls { get; set; }

        public List<string> CompletedUrls { get; set; }

        public List<string> ErrorUrls { get; set; }

        /// <summary>
        /// Gets or sets the provider's <see cref="ProviderStatistics"/> object.
        /// </summary>
        public ProviderStatistics Statistics { get; set; }

        [XmlAttribute]
        public string DestinationFolder { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PreservationRule"/> for the provider.
        /// </summary>
        [XmlAttribute]
        public PreservationRule PreservationRule { get; set; }

        /// <summary>
        /// Gets a value indicating whether TrailBot is able to run this
        /// <see cref="WebDataProvider"/>.
        /// </summary>
        public virtual bool CanRun => this.PreservationRule == PreservationRule.None || this.CanWrite();

        #endregion

        #region Methods

        /// <summary>
        /// Tests for TrailBot's ability to save trip reports.
        /// </summary>
        /// <returns>True if TrailBot is able to write to storage, false if not.</returns>
        public bool CanWrite() => Directory.Exists(this.DestinationFolder);

        /// <summary>
        /// Stops executing and yields access to the CPU for a random amount
        /// of time, as specified in <see cref="SleepRange"/>.
        /// </summary>
        protected void Sleep()
        {
            int msDelay = new Random().Next(this.SleepRange.Minimum, this.SleepRange.Maximum);
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(msDelay);

            this.OnSleeping(this, new SleepEventArgs() { Duration = timeSpan });
            Thread.Sleep(msDelay);
        }

        #region Event Handlers

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

        #endregion

        #endregion
    }
}
