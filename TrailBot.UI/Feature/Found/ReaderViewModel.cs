using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace CascadePass.TrailBot.UI.Feature.Found
{
    public class ReaderViewModel : ObservableObject
    {
        private WebProviderManager webProviderManager;
        private MatchedTripReportViewModel selectedMatch;

        public ReaderViewModel()
        {
            this.MatchedTripReports = new();
            BindingOperations.EnableCollectionSynchronization(this.MatchedTripReports, new object());
        }

        public Settings Settings { get; set; }

        public ObservableCollection<MatchedTripReportViewModel> MatchedTripReports { get; set; }

        public WebProviderManager WebProviderManager {
            get => this.webProviderManager;
            set
            {
                if (this.webProviderManager != null)
                {
                    this.webProviderManager.FoundMatch -= this.WebProviderManager_FoundMatch;
                }

                this.webProviderManager = value;
                this.MatchedTripReports.Clear();
                this.GetFoundTripReports();

                this.OnPropertyChanged(nameof(this.MatchedTripReports));
                this.OnPropertyChanged(nameof(this.SearchTerms));
            }
        }

        public MatchedTripReportViewModel SelectedMatch {
            get => this.selectedMatch;
            set
            {
                if (this.selectedMatch != value)
                {
                    this.selectedMatch = value;
                    this.OnPropertyChanged(nameof(this.SelectedMatch));
                    this.OnPropertyChanged(nameof(this.HasSelectedMatch));
                    this.OnPropertyChanged(nameof(this.ShowPreviewPane));
                }
            }
        }

        public bool HasSelectedMatch => this.SelectedMatch != null;

        public bool ShowPreviewPane => this.selectedMatch != null && this.Settings.ShowPreviewPane;

        public string[] SearchTerms => this.WebProviderManager?.Topics[0]?.MatchAny?.Split('\n');

        public void RemoveSelectedMatch()
        {
            if (this.SelectedMatch == null)
            {
                return;
            }

            this.WebProviderManager.RemoveMatch(this.SelectedMatch.MatchedTripReport);
        }

        public void MarkRead()
        {
            if (this.SelectedMatch == null)
            {
                return;
            }

            this.SelectedMatch.HasBeenSeen = true;
        }

        public void MarkUnread()
        {
            if (this.SelectedMatch == null)
            {
                return;
            }

            this.SelectedMatch.HasBeenSeen = false;
        }

        private void GetFoundTripReports()
        {
            if (this.webProviderManager != null)
            {
                this.webProviderManager.FoundMatch += this.WebProviderManager_FoundMatch;

                if (this.webProviderManager.Found != null)
                {
                    foreach (var item in this.webProviderManager.Found)
                    {
                        this.MatchedTripReports.Add(new MatchedTripReportViewModel() { MatchedTripReport = item });
                    }
                }
            }
        }

        private void WebProviderManager_FoundMatch(object sender, MatchingTripReportEventArgs e)
        {
            this.MatchedTripReports.Add(new() { MatchedTripReport = e.MatchedTripReport });
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShowPreviewPane))
            {
                this.OnPropertyChanged(nameof(ShowPreviewPane));
            }
        }
    }
}
