using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;

namespace CascadePass.TrailBot.UI
{
    public class StatusStripViewModel : ObservableObject
    {
        private string statusText;
        private bool isRequestingPage, isSleeping;
        private WebProviderManager webProviderManager;

        public StatusStripViewModel()
        {
            if (Application.Current is App tripReportReaderApplication)
            {
                this.Resources ??= Application.Current.Resources;
                this.WebProviderManager = ApplicationData.WebProviderManager;
            }

            Timer t = new(333);
            t.Elapsed += Timer_Elapsed;
            t.Start();
        }

        public double SelectedFontSize
        {
            get => (double)this.Resources["Font.Normal"];
            set
            {
                this.Resources["Font.Small"] = value / 12D * 10D;
                this.Resources["Font.Normal"] = value;
                this.Resources["Font.Large"] = value / 12D * 14D;
                this.Resources["Font.Larger"] = value / 12D * 16D;
            }
        }

        public ResourceDictionary Resources { get; set; }

        public WebProviderManager WebProviderManager
        {
            get => this.webProviderManager;
            set
            {
                if (this.webProviderManager != value)
                {
                    if (this.webProviderManager != null)
                    {
                        this.webProviderManager.Sleeping -= this.WebProviderManager_Sleeping;
                    }

                    if (this.webProviderManager?.DataProviders != null)
                    {
                        foreach (var provider in this.webProviderManager.DataProviders)
                        {
                            provider.RequestingPage -= this.Provider_RequestingPage;
                            provider.Sleeping -= this.Provider_Sleeping;
                        }
                    }

                    this.webProviderManager = value;

                    if (this.webProviderManager != null)
                    {
                        this.webProviderManager.Sleeping += this.WebProviderManager_Sleeping;
                    }

                    if (this.webProviderManager?.DataProviders != null)
                    {
                        foreach (var provider in this.webProviderManager.DataProviders)
                        {
                            provider.RequestingPage += this.Provider_RequestingPage;
                            provider.Sleeping += this.Provider_Sleeping;
                        }
                    }

                    this.OnPropertyChanged(nameof(this.WebProviderManager));
                }
            }
        }

        public bool IsRequestingPage
        {
            get => this.isRequestingPage;
            set
            {
                if (this.isRequestingPage != value)
                {
                    this.isRequestingPage = value;
                    this.OnPropertyChanged(nameof(this.IsRequestingPage));
                }
            }
        }

        public bool IsSleeping
        {
            get => this.isSleeping;
            set
            {
                if (this.isSleeping != value)
                {
                    this.isSleeping = value;
                    this.OnPropertyChanged(nameof(this.IsSleeping));
                }
            }
        }

        public int TripReportsRead => this.WebProviderManager?.DataProviders?.Sum(m => m.CompletedUrls.Count) ?? 0;

        public int PendingCount => this.WebProviderManager?.DataProviders?.Sum(m => m.PendingUrls.Count) ?? 0;

        public int MatchedCount => this.WebProviderManager?.Found?.Count ?? 0;

        public string MemoryAllocated
        {
            get
            {
                if (!this.ShowMemoryInfo)
                {
                    return string.Empty;
                }

                try
                {
                    using var currentProcess = Process.GetCurrentProcess();
                    currentProcess.Refresh();

                    return currentProcess.PrivateMemorySize64.ToString("#,##0");
                }
                catch (InvalidOperationException)
                {
                    return string.Empty;
                }
            }
        }

        //TODO: Create local settings reference, subscribe to changes, raise notifications for this
        public bool ShowMemoryInfo => ApplicationData.Settings.DebugMode;

        public string StatusText
        {
            get => this.statusText;
            set
            {
                if (!string.Equals(this.statusText, value, StringComparison.Ordinal))
                {
                    this.statusText = value;
                    this.OnPropertyChanged(nameof(this.StatusText));
                }
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.MatchedCount));
            this.OnPropertyChanged(nameof(this.PendingCount));
            this.OnPropertyChanged(nameof(this.TripReportsRead));
            this.OnPropertyChanged(nameof(this.MemoryAllocated));
        }

        private void WebProviderManager_Sleeping(object sender, SleepEventArgs e)
        {
            this.IsSleeping = true;
            this.IsRequestingPage = false;

            this.StatusText = $"Sleeping {e.Duration}";
        }

        private void Provider_Sleeping(object sender, SleepEventArgs e)
        {
            this.IsSleeping = true;
            this.IsRequestingPage = false;
            this.StatusText = $"Sleeping {e.Duration}";
        }

        private void Provider_RequestingPage(object sender, PageRequestEventArgs e)
        {
            this.IsSleeping = false;
            this.IsRequestingPage = true;
            this.StatusText = $"Requesting {e.Uri}";
        }

    }
}
